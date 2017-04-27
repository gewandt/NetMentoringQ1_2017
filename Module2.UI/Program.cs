using System.Configuration;
using System.IO;
using Tasks.AdvancedXML;

namespace Module2.UI
{
    class Program
    {
        private static readonly string RootPath = ConfigurationManager.AppSettings["path"];

        private static readonly string InvalidXmlPath = Path.Combine(RootPath, "xml\\books.invalid.xml");
        private static readonly string ValidXmlPath = Path.Combine(RootPath, "xml\\books.valid.xml");

        private static readonly string HtmlReportXsltPath = Path.Combine(RootPath, "xslt\\HtmlReport.xslt");
        private static readonly string RssTransformXsltPath = Path.Combine(RootPath, "xslt\\RssTransform.xslt");
        private static readonly string ResultRss = Path.Combine(RootPath, "result\\rss.xml");
        private static readonly string ResultHtmlReport = Path.Combine(RootPath, "result\\report.html");
        
        private static readonly string BooksXsdPath = Path.Combine(RootPath, "xsd\\books.xsd");
        private static readonly string SchemeTargetUri = "http://library.by/catalog";

        public static void ValidationValidXml()
        {
            XmlValidator.Validation(ValidXmlPath, SchemeTargetUri, BooksXsdPath);
        }

        public static void ValidationInvalidXml()
        {
            XmlValidator.Validation(InvalidXmlPath, SchemeTargetUri, BooksXsdPath);
        }

        public static void TransformToRss()
        {
            Transformation.TransformToRss(ValidXmlPath, RssTransformXsltPath, ResultRss);
        }

        public static void TransformToHtmlReport()
        {
            var output = new FileStream(ResultHtmlReport, FileMode.Create);
            var input = new FileStream(ValidXmlPath, FileMode.Open, FileAccess.Read);
            Transformation.TransformToHtmlReport(HtmlReportXsltPath, input, output);
        }

        static void Main(string[] args)
        {
            TransformToRss();
            TransformToHtmlReport();
            ValidationValidXml();
            ValidationInvalidXml();
        }
    }
}
