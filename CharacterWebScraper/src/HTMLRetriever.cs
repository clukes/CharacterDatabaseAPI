using HtmlAgilityPack;

namespace CharacterWebScraper;
public class HTMLRetriever
{
    public const string BaseWikiUrl = "https://en.wikipedia.org/wiki/";

    public static HtmlDocument GetDocument(string fullUrl)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(fullUrl);
        return doc;
    }
    public static HtmlDocument GetWikiDocument(string pageUrl)
    {
        return GetDocument(BaseWikiUrl + pageUrl);
    }
}