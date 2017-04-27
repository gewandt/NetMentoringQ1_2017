using System.IO;
using Tasks.AdvancedXml;
using Tasks.AdvancedXML;

namespace Module2.UI
{
    class Program
    {
        public static void ValidationValidXml()
        {
            XmlValidator.Validate(PathHelper.ValidXmlPath, PathHelper.BooksXsdPath);
        }

        public static void ValidationInvalidXml()
        {
            XmlValidator.Validate(PathHelper.InvalidXmlPath, PathHelper.BooksXsdPath);
        }

        public static void TransformToRss()
        {
            Transformation.TransformToRss(PathHelper.ValidXmlPath, PathHelper.RssTransformXsltPath, PathHelper.ResultRss);
        }

        public static void TransformToHtmlReport()
        {
            var output = new FileStream(PathHelper.ResultHtmlReport, FileMode.Create);
            var input = new FileStream(PathHelper.ValidXmlPath, FileMode.Open, FileAccess.Read);
            Transformation.TransformToHtmlReport(PathHelper.HtmlReportXsltPath, input, output);
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
