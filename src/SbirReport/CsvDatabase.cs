using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace SbirReport;
public class CsvDatabase
{
    public readonly List<Award> Awards = [];

    public CsvDatabase(string csvFilePath)
    {
        Console.WriteLine($"Reading {Path.GetFileName(csvFilePath)}...");
        string csvText = File.ReadAllText(csvFilePath);

        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            BadDataFound = null,
            LineBreakInQuotedFieldIsBadData = false
        };

        using var reader = new StringReader(csvText);
        using var csv = new CsvReader(reader, config);

        // read column names
        csv.Read();
        string[] columnNames = Enumerable.Range(0, csv.ColumnCount).Select(x => csv[x] ?? "unknown").ToArray();
        const int expectedColumnCount = 41;
        if (columnNames.Length != expectedColumnCount)
            throw new InvalidDataException($"Expected {expectedColumnCount} columns");
        Console.WriteLine($"Identified {columnNames.Length} columns");

        // parse awards
        Console.WriteLine($"Parsing awards...");
        while (csv.Read())
        {
            string[] parts = Enumerable.Range(0, csv.ColumnCount).Select(x => csv[x] ?? "unknown").ToArray();
            Award award = new(parts);
            Awards.Add(award);
        }

        Console.WriteLine($"Sorting by date...");
        Awards = Awards.OrderByDescending(x => x.Date).ToList();

        Console.WriteLine($"Loaded {Awards.Count:N0} awards from the database.");
    }

    private static void ShowLabeledParts(string[] parts, string[] columnNames)
    {
        Console.WriteLine("");
        for (int i = 0; i < parts.Length; i++)
        {
            Console.WriteLine($"[{i:00}:{columnNames[i]}] {parts[i]}");
        }
    }

    private List<string> keywords = [];
    public void FilterKeyword(string matchText)
    {
        keywords.Add(matchText);

        List<Award> keep = [];
        foreach (Award award in Awards)
        {
            bool isMatch = award.Company.Contains(matchText, StringComparison.OrdinalIgnoreCase) ||
                 award.Title.Contains(matchText, StringComparison.OrdinalIgnoreCase) ||
                 award.Abstract.Contains(matchText, StringComparison.OrdinalIgnoreCase);

            if (isMatch)
            {
                keep.Add(award);
            }
        }
        Console.WriteLine($"Keeping only records containing '{matchText}' reduced count from {Awards.Count:N0} to {keep.Count:N0}");
        Awards.Clear();
        Awards.AddRange(keep);
    }

    public void FilterDate(DateOnly minDate)
    {

        List<Award> keep = [];
        foreach (Award award in Awards)
        {
            if (award.Date >= minDate)
                keep.Add(award);
        }

        Console.WriteLine($"Keeping {keep.Count:N0} of {Awards.Count:N0} awards newer than {minDate}");
        Awards.Clear();
        Awards.AddRange(keep);
    }

    public void SaveReport(string filename = "report.html")
    {
        Html.SaveReport(Awards, filename, keywords);
    }
}
