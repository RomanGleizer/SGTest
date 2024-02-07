using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DepartmentHierarchyPrinter
{
    public static async Task PrintDepartmentHierarchy(DatabaseContext context, int? departmentId = null, int level = 0)
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

            await PrintEmployees(context, department.ID, level + 1);
            await PrintDepartmentHierarchy(context, department.ID, level + 1);
        }
    }

    private static async Task<List<Department>> GetDepartments(DatabaseContext context, int? parentId = null)
    {
        return await context.Departments
            .Where(d => d.ParentID == parentId)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    private static async Task PrintEmployees(DatabaseContext context, int departmentId, int level)
    {
        var employees = await context.Employees
            .Where(e => e.DepartmentID == departmentId)
            .ToListAsync();

        foreach (var employee in employees)
        {
            Console.WriteLine($"{new string(' ', level)}- Сотрудник ID={employee.ID}");
        }
    }
}
