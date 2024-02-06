using System.ComponentModel;
using System.Diagnostics;

namespace SGTest.Forms;

public partial class ImportDataForm : Form
{
    public ImportDataForm()
    {
        InitializeComponent();
        FormBorderStyle = FormBorderStyle.FixedSingle;
    }

    private void ImportDataForm_Load(object sender, EventArgs e)
    {
        var importTypes = (ImportType[])Enum.GetValues(typeof(ImportType));

        foreach (var importType in importTypes)
        {
            var description = GetEnumDescription(importType);
            ImportTypeDropdown.Items.Add(description);
        }
    }

    private void DataImportTitle_Click(object sender, EventArgs e)
    {

    }

    private void SelectFileButton_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "TSV файлы (*.tsv)|*.tsv|Все файлы (*.*)|*.*";
        var result = openFileDialog.ShowDialog();

        if (result == DialogResult.OK)
            FilePathTextBox.Text = openFileDialog.FileName;
    }

    private void ImportTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute is null ? value.ToString() : attribute.Description;
    }

    private async void ImportDataToDatabaseButton_Click(object sender, EventArgs e)
    {
        try
        {
            var filePath = FilePathTextBox.Text;
            var importType = ImportTypeDropdown?.SelectedItem?.ToString();

            await DataImporter.ImportData(filePath, importType);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
