using Microsoft.EntityFrameworkCore;

public class EmployeeImporter : IDataImporter
{
    private const int BatchSize = 1000;

    public async Task ImportData(IEnumerable<string[]> data, DatabaseContext context)
    {
        var skippedTitleData = data.Skip(1).ToList();

        for (var i = 0; i < skippedTitleData.Count; i += BatchSize)
        {
            var batch = skippedTitleData.Skip(i).Take(BatchSize).ToList();
            var existingEmployeeNames = batch.Select(row => row[1].ToLower()).ToList();
            var existingEmployees = await context.Employees
                .Where(e => existingEmployeeNames.Contains(e.FullName.ToLower()))
                .ToListAsync();

            var existingEmployeeCollection = existingEmployees.ToDictionary(e => e.FullName.ToLower());
            var employeesToAdd = new List<Employee>();

            foreach (var row in batch)
            {
                var fullName = row[1];

                if (existingEmployeeCollection.TryGetValue(fullName.ToLower(), out var existingEmployee))
                    await UpdateEmployee(existingEmployee, context, row);
                else
                {
                    var departmentName = row[0];
                    var login = row[2];
                    var password = row[3];
                    var jobTitle = row[4];

                    var employee = new Employee
                    {
                        FullName = fullName,
                        Login = login,
                        Password = password,
                        JobTitle = jobTitle
                    };

                    var department = await DepartmentImporter.GetOrCreateDepartment(context, departmentName);
                    if (department != null && !string.IsNullOrEmpty(department.Name))
                        employee.DepartmentID = department.ID;

                    employeesToAdd.Add(employee);
                }
            }

            context.Employees.AddRange(employeesToAdd);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }
    }

    private static async Task UpdateEmployee(Employee existingEmployee, DatabaseContext context, string[] row)
    {
        existingEmployee.Login = row[2];
        existingEmployee.Password = row[3];
        existingEmployee.JobTitle = row[4];

        var departmentName = row[0];
        var department = await DepartmentImporter.GetOrCreateDepartment(context, departmentName);
        if (department != null)
            existingEmployee.DepartmentID = department.ID;
    }
}