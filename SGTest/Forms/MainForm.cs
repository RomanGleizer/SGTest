using Microsoft.Extensions.Configuration;
using SGTest.Forms;

namespace SGTest
{
    public partial class MainForm : Form
    {
        private readonly DatabaseContext? _context;

        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;

            var databaseSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("DatabaseSettings")
                .Get<DatabaseSettings>();

            if (databaseSettings is not null)
                _context = new DatabaseContext(databaseSettings);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DataImportButton_Click(object sender, EventArgs e)
        {
            var importDataForm = new ImportDataForm();
            importDataForm.Show();
        }

        private void PrintDatabaseData_Click(object sender, EventArgs e)
        {
            var outputDataForm = new OutputForm(_context);
            outputDataForm.Show();
        }
    }
}
