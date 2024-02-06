using System.ComponentModel;

public enum ImportType
{
    [Description("Подразделение")]
    Department,

    [Description("Сотрудник")]
    Employee,

    [Description("Должность")]
    JobTitle,
}