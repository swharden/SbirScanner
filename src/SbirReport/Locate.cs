namespace SbirReport;

// Return the root folder of the repository we are currently in
public static class Locate
{
    public static readonly string RepoFolder = GetRepoFolder();

    public static string GetRepoFolder(string filename = "LICENSE")
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

    public static string DatabaseFile()
    {
        string dataFolderPath = Path.Combine(RepoFolder, "data");

        if (!Directory.Exists(dataFolderPath))
            Directory.CreateDirectory(dataFolderPath);

        string csvFilePath = Path.Combine(dataFolderPath, "award_data.csv");
        if (File.Exists(csvFilePath))
            return csvFilePath;

        string url = "https://data.www.sbir.gov/awarddatapublic/award_data.csv";
        DownloadFileAsync(url, csvFilePath).Wait();

        return csvFilePath;
    }

    public static async Task DownloadFileAsync(string url, string savePath)
    {
        Console.WriteLine($"Downloading {url}");
        using HttpClient client = new();
        using HttpResponseMessage response = await client.GetAsync(url);
        using HttpContent content = response.Content;
        byte[] bytes = await content.ReadAsByteArrayAsync();
        Console.WriteLine($"Downloaded {bytes.Length / 1e6 : N2} MB");
        File.WriteAllBytes(savePath, bytes);
    }
}