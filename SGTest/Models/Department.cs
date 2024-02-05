using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Department
{
    [Key]
    public int ID { get; set; }

    [Key]
    [Column(Order = 0)]
    public string Name { get; set; }

    [Key]
    [Column(Order = 1)]
    public int ParentID { get; set; }

    public Department Parent { get; set; }

    public int ManagerID { get; set; }

    public Employee Manager { get; set; }

    public string Phone { get; set; }

    public List<Department> Children { get; set; } = new List<Department>();
}