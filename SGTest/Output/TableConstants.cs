public static class TableConstants
{
    public const int IdColumnWidth = 6;
    public const int FullNameColumnWidth = 30;
    public const int LoginColumnWidth = 15;
    public const int DepartmentColumnWidth = 17;

    public static readonly string IdHeader = "ID".PadRight(IdColumnWidth);
    public static readonly string FullNameHeader = "ФИО".PadRight(FullNameColumnWidth);
    public static readonly string LoginHeader = "Логин".PadRight(LoginColumnWidth);
    public static readonly string DepartmentHeader = "ID Подразделения".PadRight(DepartmentColumnWidth);
}
