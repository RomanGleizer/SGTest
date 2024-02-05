using Microsoft.EntityFrameworkCore;

public class DataImporter
{
    public static async Task ImportData(string filePath, string importType)
    {
        var lines = new List<string>();
        using (var file = new StreamReader(filePath))
            lines = file.ReadToEnd().Split('\n').ToList();

        var skippedTitleData = lines.Select(ParseTabSeparatedLine).Skip(1).ToList();
        using (var context = new DatabaseContext())
            switch (importType.ToLower())
            {
                case "подразделение":
                    await ImportDepartments(context, skippedTitleData);
                    break;
                case "сотрудник":
                    await ImportEmployees(context, skippedTitleData);
                    break;
                case "должность":
                    await ImportJobTitles(context, skippedTitleData);
                    break;
            }
    }

    private static async Task ImportEmployees(DatabaseContext context, List<string[]> data)
    {
        var skippedTitleData = data.Skip(1).ToList();
        foreach (var row in skippedTitleData)
        {
            var fullName = row[1];
            var existingEmployee = await context.Employees
                .FirstOrDefaultAsync(e => e.FullName.ToLower() == fullName.ToLower());

            if (existingEmployee is not null)
            {
                existingEmployee.Login = row[2];
                existingEmployee.Password = row[3];
                existingEmployee.JobTitle = row[4];
                continue;
            }

            var departmentName = row[0];
            var department = await context.Department.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower());

            if (department is not null)
            {
                var login = row[2];
                var password = row[3];
                var jobTitle = row[4];

                await context.Employees.AddAsync(
                    new Employee
                    {
                        DepartmentID = department.ID,
                        FullName = fullName,
                        Login = login,
                        Password = password,
                        JobTitle = jobTitle
                    });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task ImportDepartments(DatabaseContext context, List<string[]> data)
    {
        foreach (var row in data)
        {
            var departmentName = row[0];
            var parentDepartmentName = row[1];
            var managerName = row[2];
            var phone = row[3];

            var manager = await context.Employees
                .FirstOrDefaultAsync(e => e.FullName.ToLower() == managerName.ToLower());

            var parentDepartment = await context.Department
                .FirstOrDefaultAsync(d => d.Name.ToLower() == parentDepartmentName.ToLower());

            if (manager is not null && parentDepartment is not null)
            {
                var existingDepartment = await context.Department
                    .FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.ParentID == parentDepartment.ID);

                if (existingDepartment is not null)
                {
                    existingDepartment.ManagerID = manager.ID;
                    existingDepartment.Manager = manager;
                    existingDepartment.Phone = phone;
                    continue;
                }

                await context.Department.AddAsync(
                    new Department
                    {
                        Name = departmentName,
                        ParentID = parentDepartment.ID,
                        Parent = parentDepartment,
                        ManagerID = manager.ID,
                        Manager = manager,
                        Phone = phone
                    });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task ImportJobTitles(DatabaseContext context, List<string[]> data)
    {
        foreach (var row in data)
        {
            var jobTitleName = row[0];
            var jobTitle = new JobTitle { Name = jobTitleName };
            await context.JobTitles.AddAsync(jobTitle);
        }

        await context.SaveChangesAsync();
    }

    private static string[] ParseTabSeparatedLine(string line)
    {
        return line.Split('\t')
                   .Select(field => field.Trim().ToLower())
                   .ToArray();
    }
}
