namespace SbirReport;

internal static class Program
{
    public static void Main()
    {
        string dbFilePath = Locate.DatabaseFile();
        CsvDatabase db = new(dbFilePath);
        db.FilterDate(new DateOnly(2023, 01, 01));
        db.FilterKeyword("analytics");
        db.SaveReport();
    }
}
