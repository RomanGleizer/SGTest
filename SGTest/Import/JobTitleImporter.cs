using Microsoft.EntityFrameworkCore;

public class JobTitleImporter : IDataImporter
{
    public async Task ImportData(IEnumerable<string[]> data, DatabaseContext context)
    {
        var jobTitlesToAdd = data.Select(row => new JobTitle { Name = row[0] }).ToList();
        context.JobTitle.AddRange(jobTitlesToAdd);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        Console.WriteLine("Должности успешно импортированы.");
    }

    public static async Task<int?> GetJobTitleIdByName(string jobTitleName, DatabaseContext context)
    {
        var allTitles = await context.JobTitle.ToListAsync();
        var jobTitle = allTitles.FirstOrDefault(
            jt => jt.Name.ToLower().RemoveExtraSpaces() == jobTitleName.ToLower().RemoveExtraSpaces());
        return jobTitle is not null ? jobTitle.ID : null;
    }
}