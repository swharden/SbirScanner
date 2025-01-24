using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace SbirReport;
public static class CsvDatabase
{
    public static void Parse()
    {
        StringBuilder sb = new();

        string csvPath = Path.Combine(Locate.RepoFolder, "dev/sample-data/awards_search_1737143737.csv");
        string csvText = File.ReadAllText(csvPath);

        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            BadDataFound = null,
            LineBreakInQuotedFieldIsBadData = false
        };

        using var reader = new StringReader(csvText);
        using var csv = new CsvReader(reader, config);

        csv.Read(); // skip first line

        List<Award> awards = [];
        while (csv.Read())
        {
            string[] parts = Enumerable.Range(0, csv.ColumnCount).Select(x => csv[x] ?? "unknown").ToArray();
            awards.Add(new Award(parts));
        }

        Html.SaveReport(awards.OrderBy(x => x.Date).Reverse(), "report.html");
    }
}
