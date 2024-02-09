using Microsoft.EntityFrameworkCore;

public class DepartmentImporter : IDataImporter
{
    public async Task ImportData(IEnumerable<string[]> data, DatabaseContext context)
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
                .ToListAsync()
                .ContinueWith(departments => departments.Result
                    .FirstOrDefault(d => d.Name.ToLower().RemoveExtraSpaces() == parentDepartmentName.ToLower().RemoveExtraSpaces()));

            var allDepartments = await context.Departments.ToListAsync();
            var existingDepartment = allDepartments
                .FirstOrDefault(d => d.Name.ToLower().RemoveExtraSpaces() == departmentName.ToLower().RemoveExtraSpaces());

            if (existingDepartment is not null)
            {
                UpdateDepartment(existingDepartment, manager, phone);
                existingDepartment.Parent = parentDepartment;
                existingDepartment.ParentID = parentDepartment?.ID;
            }
            else
            {
                var newDepartment = new Department
                {
                    Name = departmentName,
                    ParentID = parentDepartment?.ID,
                    Parent = parentDepartment,
                    Phone = phone,
                    Children = new List<Department>()
                };

                var managerEmployee = await context.Employees
                    .FirstOrDefaultAsync(e => e.FullName.ToLower() == managerName.ToLower());
                if (managerEmployee != null)
                {
                    managerEmployee.DepartmentID = newDepartment.ID;
                    newDepartment.ManagerID = managerEmployee.ID;
                    newDepartment.Manager = managerEmployee;
                }

                departmentsToAdd.Add(newDepartment);
                await ImportChildDepartments(context, newDepartment, data);
            }
        }

        context.Departments.AddRange(departmentsToAdd);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        Console.WriteLine("Департаменты успешно импортированы.");
    }

    private static void UpdateDepartment(Department existingDepartment, Employee manager, string phone)
    {
        existingDepartment.ManagerID = manager?.ID;
        existingDepartment.Manager = manager;
        existingDepartment.Phone = phone;
    }

    private static async Task ImportChildDepartments(DatabaseContext context, Department parentDepartment, IEnumerable<string[]> data)
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

            if (manager != null)
            {
                var existingChildDepartment = await context.Departments
                    .FirstOrDefaultAsync(d =>
                        d.ParentID == parentDepartment.ID &&
                        d.Name.ToLower() == childDepartmentName.ToLower());

                if (existingChildDepartment != null)
                    UpdateDepartment(existingChildDepartment, manager, childPhone);
                else
                {
                    var existingDepartmentToAdd = childDepartmentsToAdd.FirstOrDefault(d =>
                        d.ParentID == parentDepartment.ID &&
                        d.Name.ToLower() == childDepartmentName.ToLower());

                    if (existingDepartmentToAdd == null)
                    {
                        var childDepartment = new Department
                        {
                            Name = childDepartmentName,
                            ParentID = parentDepartment.ID,
                            Parent = parentDepartment,
                            ManagerID = manager?.ID,
                            Manager = manager,
                            Phone = childPhone,
                            Children = new List<Department>()
                        };

                        parentDepartment.Children.Add(childDepartment);
                        childDepartmentsToAdd.Add(childDepartment);
                    }
                    else
                        UpdateDepartment(existingDepartmentToAdd, manager, childPhone);
                }
            }
        }

        context.Departments.AddRange(childDepartmentsToAdd);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    public static async Task<Department> GetOrCreateDepartment(DatabaseContext context, string departmentName)
    {
        var allDepartments = await context.Departments.ToListAsync();
        var existingDepartment = allDepartments
            .FirstOrDefault(d => d.Name.ToLower().RemoveExtraSpaces() == departmentName.ToLower().RemoveExtraSpaces());

        if (existingDepartment != null)
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
}