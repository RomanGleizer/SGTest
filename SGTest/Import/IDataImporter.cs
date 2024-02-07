public interface IDataImporter
{
    Task ImportData(IEnumerable<string[]> data, DatabaseContext context);
}