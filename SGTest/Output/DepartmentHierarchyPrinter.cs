using Microsoft.EntityFrameworkCore;

public class DepartmentHierarchyPrinter
{
    public static async Task PrintDepartmentHierarchy(DatabaseContext context, int? departmentId = null, int level = 1)
    {
        var departments = await GetDepartments(context, departmentId);
        foreach (var department in departments)
        {
            Console.WriteLine($"{new string('=', level)} Подразделение ID={department.ID}");

            if (department.ManagerID != null)
            {
                var manager = await context.Employees.FindAsync(department.ManagerID);
                Console.WriteLine($"{new string(' ', level)} * Сотрудник ID={manager.ID}");
            }

            await PrintEmployeesForDepartments(context, department.ID, level + 1);
            await PrintDepartmentHierarchy(context, department.ID, level + 1);
        }
    }

    private static async Task<List<Department>> GetDepartments(DatabaseContext context, int? parentId = null)
    {
        return await context.Departments
            .Where(d => d.ParentID == parentId)
            .OrderBy(d => d.ID)
            .ToListAsync();
    }

    private static async Task PrintEmployeesForDepartments(DatabaseContext context, int departmentId, int level)
    {
        var employees = await context.Employees
            .Where(e => e.DepartmentID == departmentId)
            .ToListAsync();

        foreach (var employee in employees)
            Console.WriteLine($"{new string(' ', level)}- Сотрудник ID={employee.ID}");
    }

    public static async Task PrintEmployees(DatabaseContext context)
    {
        var employees = await context.Employees.ToListAsync();

        Console.WriteLine($"| {TableConstants.IdHeader} | {TableConstants.FullNameHeader} | {TableConstants.LoginHeader} | {TableConstants.DepartmentHeader} |");
        Console.WriteLine(new string('-', TableConstants.IdColumnWidth + TableConstants.FullNameColumnWidth + TableConstants.LoginColumnWidth + TableConstants.DepartmentColumnWidth + 9));

        foreach (var employee in employees)
        {
            Console.WriteLine(
                $"| {employee.ID.ToString().PadRight(TableConstants.IdColumnWidth)} " +
                $"| {employee.FullName.PadRight(TableConstants.FullNameColumnWidth)} " +
                $"| {employee.Login.PadRight(TableConstants.LoginColumnWidth)} " +
                $"| {employee.DepartmentID.ToString().PadRight(TableConstants.DepartmentColumnWidth)} |");
        }
    }


    public static async Task PrintJobTitles(DatabaseContext context)
    {
        var titles = await context.JobTitle.ToListAsync();
        Console.WriteLine("Должности: ");

        foreach (var title in titles)
            Console.WriteLine(title.Name);
    }
}
