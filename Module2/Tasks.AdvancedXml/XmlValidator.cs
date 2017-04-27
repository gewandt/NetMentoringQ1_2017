using System;
using System.Xml;
using System.Xml.Schema;
using Tasks.AdvancedXml;

namespace Tasks.AdvancedXML
{
    public static class XmlValidator
    {
        public static void Validate(string xmlPath, string schemaUri)
        {
            var xmlReader = XmlReader.Create(xmlPath, CreateReaderSettings(schemaUri));
            while (xmlReader.Read()) { }
        }

        public static bool ValidateXml(string xmlPath, string schemaUri)
        {
            try
            {
                var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                settings.Schemas.Add(PathHelper.SchemeTargetUri, PathHelper.BooksXsdPath);
                var reader = XmlReader.Create(xmlPath, settings);
                var doc = new XmlDocument();
                doc.Load(reader);
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }
            return true;
        }

        private static XmlReaderSettings CreateReaderSettings(string schemaUri)
        {
            var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Schemas.Add(PathHelper.SchemeTargetUri, schemaUri);
            settings.ValidationEventHandler += (sender, args) => 
            {
                Console.WriteLine($"{args.Exception.LineNumber}, {args.Exception.LinePosition}, {args.Message}");
            };
            return settings;
        }
    }
}
