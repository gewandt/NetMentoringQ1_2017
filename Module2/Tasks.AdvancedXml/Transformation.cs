using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Tasks.AdvancedXml;

namespace Tasks.AdvancedXML
{
    public class Transformation : ITransform
    {
        public void TransformToRss(string sourceXml, string pathToXsltTemplate, string pathToResultFile)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(pathToXsltTemplate);
            var fileStream = new FileStream(pathToResultFile, FileMode.Create);
            xslt.Transform(sourceXml, new XsltArgumentList(), fileStream);
        }

        public void TransformToHtmlReport(string pathToXsltTemplate, FileStream input, FileStream output)
        {
            var xsltSettings = new XsltSettings
            {
                EnableScript = true
            };
            var xslt = new XslCompiledTransform();
            var xmlReader = XmlReader.Create(pathToXsltTemplate);
            xslt.Load(xmlReader, xsltSettings, null);
            var document = new XPathDocument(input);
            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                CloseOutput = false
            };
            var writer = XmlWriter.Create(output, xmlWriterSettings);
            var argList = new XsltArgumentList();
            argList.AddParam("Date", "", DateTime.Now.ToString("f"));
            xslt.Transform(document, argList, writer);
        }
    }
}
