using System.Configuration;
using System.IO;

namespace Tasks.AdvancedXml
{
    public class PathHelper
    {
        public static readonly string RootPath = ConfigurationManager.AppSettings["path"];

        public static readonly string InvalidXmlPath = Path.Combine(RootPath, "xml\\books.invalid.xml");
        public static readonly string ValidXmlPath = Path.Combine(RootPath, "xml\\books.valid.xml");
        public static readonly string HtmlReportXsltPath = Path.Combine(RootPath, "xslt\\HtmlReport.xslt");
        public static readonly string RssTransformXsltPath = Path.Combine(RootPath, "xslt\\RssTransform.xslt");
        public static readonly string ResultRss = Path.Combine(RootPath, "result\\rss.xml");
        public static readonly string ResultHtmlReport = Path.Combine(RootPath, "result\\report.html");
        public static readonly string BooksXsdPath = Path.Combine(RootPath, "xsd\\books.xsd");

        public static readonly string SchemeTargetUri = "http://library.by/catalog";
    }
}
