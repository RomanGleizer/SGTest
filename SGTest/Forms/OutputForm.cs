using Microsoft.EntityFrameworkCore;

namespace SGTest.Forms;

public partial class OutputForm : Form
{
    private readonly DatabaseContext _context;

    public OutputForm(DatabaseContext context)
    {
        InitializeComponent();
        FormBorderStyle = FormBorderStyle.FixedSingle;
        _context = context;
    }

    private void OutputTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void OutputForm_Load(object sender, EventArgs e)
    {
        var importDataForm = new ImportDataForm();
        var importTypes = (ImportType[])Enum.GetValues(typeof(ImportType));

        foreach (var importType in importTypes)
        {
            var description = importDataForm.GetEnumDescription(importType);
            OutputTypeDropdown.Items.Add(description);
        }
    }

    private async void OutputDataFromDatabaseButton_Click(object sender, EventArgs e)
    {
        Console.Clear();

        var importType = OutputTypeDropdown?.SelectedItem?.ToString()?.ToLower();
        switch (importType)
        {
            case "подразделение":
                await DepartmentHierarchyPrinter.PrintDepartmentHierarchy(_context);
                return;
            case "сотрудник":
                await DepartmentHierarchyPrinter.PrintEmployees(_context);
                return;
            case "должность":
                await DepartmentHierarchyPrinter.PrintJobTitles(_context);
                return;
            default:
                return;
        }
    }
}
