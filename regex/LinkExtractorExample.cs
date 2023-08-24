using System.Text.RegularExpressions;

public class LinkExtractorExample
{
    public static async Task Run()
    {

        var client = new HttpClient();
        var content = await client.GetStringAsync("https://www.nasa.gov/rss/dyn/breaking_news.rss");
        var matches = Regex.Matches(content, @"<link[^>]*>(?<LinkUrl>[^<]*)", RegexOptions.Compiled);
        var links = matches
            .Select(x => Uri.TryCreate(x.Groups["LinkUrl"].Value, UriKind.Absolute, out Uri result) ? result : null)
            .Where(x => x != null)
            .ToList();

        links.ForEach(Console.WriteLine);
    }
}
