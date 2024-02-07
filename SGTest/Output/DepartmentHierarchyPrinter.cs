using Microsoft.EntityFrameworkCore;

public class DepartmentHierarchyPrinter
{
    public static async Task PrintDepartmentHierarchy(DatabaseContext context, int? departmentId = null, string prefix = "")
    {
        var departments = await GetDepartments(context, departmentId);
        foreach (var department in departments)
        {
            Console.WriteLine($"{prefix}= Подразделение ID={department.ID}");

            if (department.ManagerID != null)
            {
                var manager = await context.Employees.FindAsync(department.ManagerID);
                Console.WriteLine($"{prefix}* Сотрудник ID={manager.ID} ({manager.JobTitle})");
            }

            await PrintEmployees(context, department.ID, prefix);
            await PrintDepartmentHierarchy(context, department.ID, prefix + " ");
        }
    }

    private static async Task<List<Department>> GetDepartments(DatabaseContext context, int? parentId = null)
    {
        return await context.Departments
            .Where(d => d.ParentID == parentId)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    private static async Task PrintEmployees(DatabaseContext context, int departmentId, string prefix)
    {
        var employees = await context.Employees
            .Where(e => e.DepartmentID == departmentId)
            .ToListAsync();

        foreach (var employee in employees)
            Console.WriteLine($"{prefix}- Сотрудник ID={employee.ID} ({employee.JobTitle})");
    }
}
