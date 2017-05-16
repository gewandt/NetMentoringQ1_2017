using System.Configuration;
using System.IO;

namespace ScanService
{
    public class ConfigHelper
    {
        private static readonly string _rootPath = ConfigurationManager.AppSettings["rootPath"];

        public static readonly int AttemtsCount = int.Parse(ConfigurationManager.AppSettings["attemtsCount"]);

        public static readonly string ImageDir = Path.Combine(_rootPath, "image");
        public static readonly string PdfDir = Path.Combine(_rootPath, "pdf");
        public static readonly string TmpDir = Path.Combine(_rootPath, "tmp");
    }
}
