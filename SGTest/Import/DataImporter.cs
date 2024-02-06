using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

public class DataImporter
{
    private const int BatchSize = 1000;

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
                        default:
                            Console.Error.WriteLine($"Неизвестный тип импорта: {importType}");
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
        var newDepartments = new List<Department>();

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
                    UpdateEmployee(existingEmployee, row);
                else
                {
                    var departmentName = row[0];
                    var department = await GetOrCreateDepartment(context, departmentName);
                    if (!newDepartments.Any(d => d.Name.ToLower() == departmentName.ToLower()))
                        newDepartments.Add(department);

                    var login = row[2];
                    var password = row[3];
                    var jobTitle = row[4];

                    employeesToAdd.Add(new Employee
                    {
                        DepartmentID = department.ID,
                        FullName = fullName,
                        Login = login,
                        Password = password,
                        JobTitle = jobTitle
                    });
                }
            }

            context.Employees.AddRange(employeesToAdd);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }
    }

    private static async Task ImportDepartments(DatabaseContext context, List<string[]> data)
    {
        var departmentsToAdd = new List<Department>();

        foreach (var row in data)
        {
            var departmentName = row[0];
            var parentDepartmentName = row[1];
            var managerName = row[2];
            var phone = row[3];

            var manager = await context.Employees
                .FirstOrDefaultAsync(e => e.FullName.ToLower() == managerName.ToLower());

            var parentDepartment = await context.Departments
                .FirstOrDefaultAsync(d => d.Name.ToLower() == parentDepartmentName.ToLower());

            var allDepartments = await context.Departments.ToListAsync();
            var existingDepartment = allDepartments
                .FirstOrDefault(d => RemoveExtraSpaces(d.Name).ToLower() == RemoveExtraSpaces(departmentName).ToLower());

            if (existingDepartment is not null)
                UpdateDepartment(existingDepartment, manager, phone);
            else
            {
                var newDepartment = new Department
                {
                    Name = departmentName,
                    ParentID = parentDepartment?.ID,
                    Parent = parentDepartment,
                    ManagerID = manager.ID,
                    Manager = manager,
                    Phone = phone,
                    Children = new List<Department>()
                };

                departmentsToAdd.Add(newDepartment);
                await ImportChildDepartments(context, newDepartment, data);
            }
        }

        context.Departments.AddRange(departmentsToAdd);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }


    private static async Task ImportChildDepartments(DatabaseContext context, Department parentDepartment, List<string[]> data)
    {
        var childDepartments = data.Where(r => r[1].ToLower() == parentDepartment.Name.ToLower()).ToList();
        var childDepartmentsToAdd = new List<Department>();

        foreach (var childRow in childDepartments)
        {
            var childDepartmentName = childRow[0];
            var childManagerName = childRow[2];
            var childPhone = childRow[3];

            var manager = await context.Employees
                .FirstOrDefaultAsync(e => e.FullName.ToLower() == childManagerName.ToLower());

            if (manager is not null)
            {
                var existingChildDepartment = await context.Departments
                    .FirstOrDefaultAsync(d =>
                        d.ParentID == parentDepartment.ID &&
                        d.Name.ToLower() == childDepartmentName.ToLower());

                if (existingChildDepartment is not null)
                    UpdateDepartment(existingChildDepartment, manager, childPhone);
                else
                {
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
                    childDepartmentsToAdd.Add(childDepartment);
                }
            }
        }

        context.Departments.AddRange(childDepartmentsToAdd);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    private static async Task ImportJobTitles(DatabaseContext context, List<string[]> data)
    {
        var jobTitlesToAdd = data.Select(row => new JobTitle { Name = row[0] }).ToList();
        context.JobTitle.AddRange(jobTitlesToAdd);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    private static string[] ParseTabSeparatedLine(string line)
    {
        return line.Split('\t')
            .Select(field => field.Trim().ToLower())
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
        existingDepartment.ManagerID = manager?.ID;
        existingDepartment.Manager = manager;
        existingDepartment.Phone = phone;
    }

    private static async Task<Department> GetOrCreateDepartment(DatabaseContext context, string departmentName)
    {
        var allDepartments = await context.Departments.ToListAsync();
        var existingDepartment = allDepartments
            .FirstOrDefault(d => RemoveExtraSpaces(d.Name).ToLower() == RemoveExtraSpaces(departmentName).ToLower());

        if (existingDepartment is not null)
            return existingDepartment;

        var newDepartment = new Department
        {
            Name = departmentName,
            Children = new List<Department>()
        };

        context.Departments.Add(newDepartment);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        return newDepartment;
    }

    private static string RemoveExtraSpaces(string input)
    {
        return Regex.Replace(input, @"\s+", " ").Trim();
    }
}
