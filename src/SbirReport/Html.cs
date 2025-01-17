using System.Text;

namespace SbirReport;

internal static class Html
{
    public static string WrapInBootstrap(string html)
    {
        return """
            <!doctype html>
            <html lang="en">
              <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <title>SBIR Report</title>
                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
                <style>
                a {text-decoration: none}
                a:hover {text-decoration: underline}
                </style>
            </head>
              <body>
                <div class='container'>{{BODY}}</div>
                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
              </body>
            </html>
            """.Replace("{{BODY}}", html);
    }

    public static void SaveReport(IEnumerable<Award> awards, string saveAs = "report.html")
    {
        StringBuilder sb = new();

        int count = 1;
        foreach (Award award in awards)
        {
            string highlightedAbstract = award.Abstract.Replace("medical", "<mark>medical</mark>", StringComparison.InvariantCultureIgnoreCase);

            string html = $"""
                <h1 class='mt-5'>{count++}. {award.Company}</h1> 
                <div><a href='{award.CompanyURL}'>{award.CompanyURL}</a></div>
                <div><strong>{award.Title}</strong></div>
                <div>{award.Employees} people Awarded ${award.Thousand}k on {award.Date} <a href='{award.AwardURL}'>{award.TrackingNumber}</a></div>
                <div class='bg-light border rounded text-muted p-3'>{highlightedAbstract}</div>
                <hr class='my-4 invisible' />
                """;
            sb.AppendLine(html);
        }

        saveAs = Path.GetFullPath(saveAs);
        File.WriteAllText(saveAs, Html.WrapInBootstrap(sb.ToString()));
        Console.WriteLine(saveAs);
    }
}
