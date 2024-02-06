using SGTest.Forms;

namespace SGTest;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        FormBorderStyle = FormBorderStyle.FixedSingle;
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void DataImportButton_Click(object sender, EventArgs e)
    {
        var importDataForm = new ImportDataForm();
        importDataForm.Show();
    }
}
