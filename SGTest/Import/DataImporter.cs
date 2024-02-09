using Microsoft.Extensions.Configuration;

public class DataImporter
{
    public static async Task ImportData(string filePath, string importType)
    {
        try
        {
            var lines = File.ReadAllLines(filePath).ToList();

            if (lines.Count == 0)
            {
                Console.Error.WriteLine("Ошибка: Файл имеет неверный формат.");
                return;
            }

            var header = ParseTabSeparatedLine(lines[0]);
            switch (importType.ToLower())
            {
                case "подразделение":
                    if (header.Length != 4 || header[0] != "название" || header[1] != "родительское подразделение" || header[2] != "руководитель" || header[3] != "телефон")
                    {
                        MessageBox.Show("Ошибка: Файл с данными о подразделениях имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case "сотрудник":
                    if (header.Length != 5 || header[0] != "подразделение" || header[1] != "фио" || header[2] != "логин" || header[3] != "пароль" || header[4] != "должность")
                    {
                        MessageBox.Show("Ошибка: Файл с данными о сотрудниках имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case "должность":
                    if (header.Length != 1)
                    {
                        MessageBox.Show("Ошибка: Файл с данными о должностях имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                default:
                    MessageBox.Show($"Неизвестный тип импорта: {importType}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            var skippedTitleData = lines.Skip(1)
                                        .Where(line => !string.IsNullOrWhiteSpace(line))
                                        .Select(ParseTabSeparatedLine)
                                        .ToList();

            var databaseSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("DatabaseSettings")
                .Get<DatabaseSettings>();

            if (databaseSettings is null || string.IsNullOrWhiteSpace(databaseSettings.ConnectionString))
            {
                Console.Error.WriteLine("Ошибка: Некорректные настройки базы данных.");
                return;
            }

            using (var context = new DatabaseContext(databaseSettings))
            {
                var importer = GetDataType(importType);
                if (importer is not null)
                    await importer.ImportData(skippedTitleData, context);
                else
                    Console.Error.WriteLine($"Неизвестный тип импорта: {importType}");

                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public static IDataImporter GetDataType(string importType)
    {
        switch (importType.ToLower())
        {
            case "подразделение":
                return new DepartmentImporter();
            case "сотрудник":
                return new EmployeeImporter();
            case "должность":
                return new JobTitleImporter();
            default:
                return null;
        }
    }

    private static string[] ParseTabSeparatedLine(string line)
    {
        return line.Split('\t')
            .Select(field => field.Trim().ToLower())
            .ToArray();
    }
}
