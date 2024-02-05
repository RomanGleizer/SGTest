using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DataImporter
{
    public static async Task ImportData(string filePath, string importType)
    {
        try
        {
            var lines = File.ReadAllLines(filePath).ToList();
            var skippedTitleData = lines.Where(line => !string.IsNullOrWhiteSpace(line))
                                        .Select(ParseTabSeparatedLine)
                                        .Skip(1)
                                        .ToList();

            var databaseSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("DatabaseSettings")
                .Get<DatabaseSettings>();

            if (databaseSettings is not null)
                using (var context = new DatabaseContext(databaseSettings))
                {
                    switch (importType.ToLowerInvariant())
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

                    await context.SaveChangesAsync();
                }
        }
        catch (Exception ex) 
        {
            Console.Error.WriteLine($"Произошла ошибка: {ex}");
        }
    }

    private static async Task ImportEmployees(DatabaseContext context, List<string[]> data)
    {
        var skippedTitleData = data.Skip(1).ToList();
        foreach (var row in skippedTitleData)
        {
            var fullName = row[1];
            var existingEmployee = await context.Employees
                .FirstOrDefaultAsync(e => e.FullName.ToLowerInvariant() == fullName.ToLowerInvariant());

            if (existingEmployee is not null)
            {
                UpdateEmployee(existingEmployee, row);
                continue;
            }

            var departmentName = row[0];
            var department = await context.Department
                .FirstOrDefaultAsync(d => d.Name.ToLowerInvariant() == departmentName.ToLowerInvariant());

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
                .FirstOrDefaultAsync(e => e.FullName.ToLowerInvariant() == managerName.ToLowerInvariant());

            var parentDepartment = await context.Department
                .FirstOrDefaultAsync(d => d.Name.ToLowerInvariant() == parentDepartmentName.ToLowerInvariant());

            if (manager is null || parentDepartment is null)
                continue;

            var existingDepartment = await context.Department
                .FirstOrDefaultAsync(d => 
                    d.ParentID == parentDepartment.ID && 
                    d.Name.ToLowerInvariant() == departmentName.ToLowerInvariant());

            if (existingDepartment is not null)
            {
                UpdateDepartment(existingDepartment, manager, phone);
                continue;
            }

            var newDepartment = new Department
            {
                Name = departmentName,
                ParentID = parentDepartment.ID,
                Parent = parentDepartment,
                ManagerID = manager.ID,
                Manager = manager,
                Phone = phone,
                Children = new List<Department>()
            };

            await context.Department.AddAsync(newDepartment);
            await ImportChildDepartments(context, newDepartment, data);
        }
    }

    private static async Task ImportChildDepartments(DatabaseContext context, Department parentDepartment, List<string[]> data)
    {
        var childDepartments = data.Where(r => r[1].ToLowerInvariant() == parentDepartment.Name.ToLowerInvariant()).ToList();
        foreach (var childRow in childDepartments)
        {
            var childDepartmentName = childRow[0];
            var childManagerName = childRow[2];
            var childPhone = childRow[3];

            var manager = await context.Employees
                .FirstOrDefaultAsync(e => e.FullName.ToLowerInvariant() == childManagerName.ToLowerInvariant());

            if (manager is not null)
            {
                var existingChildDepartment = await context.Department
                    .FirstOrDefaultAsync(d => 
                        d.ParentID == parentDepartment.ID && 
                        d.Name.ToLowerInvariant() == childDepartmentName.ToLowerInvariant());

                if (existingChildDepartment is not null)
                {
                    UpdateDepartment(existingChildDepartment, manager, childPhone);
                    continue;
                }

                var childDepartment = new Department
                {
                    Name = childDepartmentName,
                    ParentID = parentDepartment.ID,
                    Parent = parentDepartment,
                    ManagerID = manager.ID,
                    Manager = manager,
                    Phone = childPhone,
                    Children = new List<Department>()
                };

                parentDepartment.Children.Add(childDepartment);

                await context.Department.AddAsync(childDepartment);
                await ImportChildDepartments(context, childDepartment, data);
            }
        }
    }

    private static async Task ImportJobTitles(DatabaseContext context, List<string[]> data)
    {
        foreach (var row in data)
        {
            var jobTitleName = row[0];
            var jobTitle = new JobTitle { Name = jobTitleName };
            await context.JobTitles.AddAsync(jobTitle);
        }
    }

    private static string[] ParseTabSeparatedLine(string line)
    {
        return line.Split('\t')
                   .Select(field => field.Trim().ToLowerInvariant())
                   .ToArray();
    }

    private static void UpdateEmployee(Employee existingEmployee, string[] row)
    {
        existingEmployee.Login = row[2];
        existingEmployee.Password = row[3];
        existingEmployee.JobTitle = row[4];
    }

    private static void UpdateDepartment(Department existingDepartment, Employee manager, string phone)
    {
        existingDepartment.ManagerID = manager.ID;
        existingDepartment.Manager = manager;
        existingDepartment.Phone = phone;
    }
}
