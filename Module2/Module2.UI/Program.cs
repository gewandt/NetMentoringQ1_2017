using Castle.DynamicProxy;
using Logger;
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
            var transform = new Transformation();
            transform.TransformToRss(PathHelper.ValidXmlPath, PathHelper.RssTransformXsltPath, PathHelper.ResultRss);
        }

        public static void TransformToHtmlReport()
        {
            var output = new FileStream(PathHelper.ResultHtmlReport, FileMode.Create);
            var input = new FileStream(PathHelper.ValidXmlPath, FileMode.Open, FileAccess.Read);
            var transform = new Transformation();
            transform.TransformToHtmlReport(PathHelper.HtmlReportXsltPath, input, output);
        }

        static void Main(string[] args)
        {
            var proxy = new ProxyGenerator();
            var transformator = proxy.CreateInterfaceProxyWithTarget<ITransform>(new Transformation(), new InterceptorLog());

            transformator.TransformToRss(PathHelper.ValidXmlPath, PathHelper.RssTransformXsltPath, PathHelper.ResultRss);

            var output = new FileStream(PathHelper.ResultHtmlReport, FileMode.Create);
            var input = new FileStream(PathHelper.ValidXmlPath, FileMode.Open, FileAccess.Read);
            transformator.TransformToHtmlReport(PathHelper.HtmlReportXsltPath, input, output);

            ValidationValidXml();
            ValidationInvalidXml();
        }
    }
}
