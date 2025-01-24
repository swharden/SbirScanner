namespace SbirReport;

internal static class Program
{
    public static void Main()
    {
        string dbFilePath = Locate.DatabaseFile();
        Console.WriteLine(dbFilePath);
    }
}
