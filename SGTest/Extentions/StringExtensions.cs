using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string RemoveExtraSpaces(this string input)
    {
        return Regex.Replace(input, @"\s+", " ").Trim();
    }
}
