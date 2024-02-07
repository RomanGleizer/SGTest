public class JobTitleImporter : IDataImporter
{
    public async Task ImportData(IEnumerable<string[]> data, DatabaseContext context)
    {
        var jobTitlesToAdd = data.Select(row => new JobTitle { Name = row[0] }).ToList();
        context.JobTitle.AddRange(jobTitlesToAdd);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}
