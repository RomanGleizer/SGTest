using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Department
{
    [Key]
    public int ID { get; set; }

    public string Name { get; set; }

    public int? ParentID { get; set; }

    [ForeignKey("ParentID")]
    public Department Parent { get; set; }

    public int? ManagerID { get; set; }

    [ForeignKey("ManagerID")]
    public Employee Manager { get; set; }

    public string? Phone { get; set; }

    public List<Department> Children { get; set; } = new List<Department>();
}
