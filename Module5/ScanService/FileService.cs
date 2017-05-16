using System.IO;
using System.Threading;

namespace ScanService
{
    public class FileService
    {
        public DirectoryInfo ImgDir { get; }
        public DirectoryInfo TmpDir { get; }
        public FileService()
        {
            ImgDir = new DirectoryInfo(ConfigHelper.ImageDir);
            TmpDir = new DirectoryInfo(ConfigHelper.TmpDir);
        }
        public PdfDoc CreateNewDocument() => new PdfDoc();

        public void Delete(FileInfo file)
        {
            if(TryOpen(file, ConfigHelper.AttemtsCount))
            {
                file.Delete();
            }
        }

        public static bool TryOpen(FileInfo fileInfo, int countAttempts)
        {
            try
            {
                var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                fileStream.Close();
                return true;
            }
            catch (IOException)
            {
                Thread.Sleep(1000);
                if(countAttempts > 0) TryOpen(fileInfo, countAttempts--);
            }
            return false;
        }

        public string GetNextFilename()
        {
            var documentIndex = Directory.GetFiles(ConfigHelper.PdfDir).Length + 1;
            return Path.Combine(ConfigHelper.PdfDir, $"out_{documentIndex}.pdf");
        }
    }
}
