namespace SbirReport;

// Return the root folder of the repository we are currently in
public static class Locate
{
    public static string RepositoryFolder(string filename = "LICENSE")
    {
        string? path = Path.GetFullPath("./");
        while (path is not null)
        {
            if (File.Exists(Path.Combine(path, filename)))
                return path;
            path = Path.GetDirectoryName(path);
        }
        throw new DirectoryNotFoundException();
    }
}