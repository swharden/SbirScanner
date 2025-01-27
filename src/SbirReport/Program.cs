namespace SbirReport;

internal static class Program
{
    public static void Main()
    {
        string dbFilePath = Locate.DatabaseFile();
        CsvDatabase db = new(dbFilePath);
        db.FilterKeyword("analytics");
        db.SaveReport();
    }
}
