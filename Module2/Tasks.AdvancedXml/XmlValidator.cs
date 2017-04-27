using System;
using System.Xml;
using System.Xml.Schema;

namespace Tasks.AdvancedXML
{
    public static class XmlValidator
    {
        public static void Validation(string xmlPath, string targetNamespace, string schemaUri)
        {
            var xmlReader = XmlReader.Create(xmlPath, CreateReaderSettings(targetNamespace, schemaUri));
            while (xmlReader.Read()) { }
        }

        private static XmlReaderSettings CreateReaderSettings(string targetNamespace, string schemaUri)
        {
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
            };
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Schemas.Add(targetNamespace, schemaUri);
            settings.ValidationEventHandler += (sender, args) => 
            {
                Console.WriteLine($"{args.Exception.LineNumber}, {args.Exception.LinePosition}, {args.Message}");
            };
            return settings;
        }
    }
}
