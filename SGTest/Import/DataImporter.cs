using Microsoft.Extensions.Configuration;

public class DataImporter
{
    public static async Task ImportData(string filePath, string importType)
    {
        try
        {
            var lines = File.ReadAllLines(filePath).ToList();
            var skippedTitleData = lines.Where(line => !string.IsNullOrWhiteSpace(line))
                                        .Select(ParseTabSeparatedLine)
                                        .Skip(1)
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
                var importer = GetImporter(importType);
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

    private static IDataImporter GetImporter(string importType)
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