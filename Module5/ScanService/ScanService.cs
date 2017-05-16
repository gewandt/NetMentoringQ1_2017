using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace ScanService
{
    public class ScanService : ServiceControl
    {
        private const int TimePending = 10000;

        private readonly FileSystemWatcher _fsWatcher;
        private readonly Task _task;
        private readonly CancellationTokenSource _cancelToken;
        private readonly AutoResetEvent _fileCreatedEvent;

        private PdfDoc _pdf;
        private FileService FileService { get; } = new FileService();

        public ScanService()
        {
            _fileCreatedEvent = new AutoResetEvent(false);
            _fsWatcher = new FileSystemWatcher();
            _cancelToken = new CancellationTokenSource();
            _pdf = CreateNewDocument();
            _fsWatcher.Created += (sender, args) => _fileCreatedEvent.Set();
            _task = new Task(() => Work(_cancelToken.Token));
        }

        public bool Start(HostControl hostControl)
        {
            _task.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _cancelToken.Cancel();
            _task.Wait();
            return true;
        }

        public void Work(CancellationToken token)
        {
            var currentIndex = -1;
            var imageCount = 0;
            var nextPageWaiting = false;
            do
            {
                foreach (var file in FileService.ImgDir.GetFiles().Skip(imageCount))
                {
                    var fileName = file.Name;
                    if (IsValid(fileName))
                    {
                        var imageIndex = GetIndex(fileName);
                        if (imageIndex != currentIndex + 1 && currentIndex != -1 && nextPageWaiting)
                        {
                            _pdf = SavePdf(_pdf);
                            nextPageWaiting = false;
                        }

                        if (FileService.TryOpen(file, ConfigHelper.AttemtsCount))
                        {
                            _pdf.AddImage(file.FullName);
                            currentIndex = imageIndex;
                            nextPageWaiting = true;
                            imageCount++;
                        }
                    }
                }

                if (!_fileCreatedEvent.WaitOne(TimePending) && nextPageWaiting)
                {
                    _pdf = SavePdf(_pdf);
                    nextPageWaiting = false;
                }

                if (token.IsCancellationRequested)
                {
                    if (nextPageWaiting)
                    {
                        _pdf.Save(FileService.GetNextFilename());
                    }

                    foreach (var file in FileService.ImgDir.GetFiles())
                    {
                        FileService.Delete(file);
                    }
                }
            } while (!token.IsCancellationRequested);
        }

        private PdfDoc SavePdf(PdfDoc pdf)
        {
            pdf.Save(FileService.GetNextFilename());
            return CreateNewDocument();
        }

        private int GetIndex(string fileName)
        {
            var match = Regex.Match(fileName, @"[0-9]{3}");
            return match.Success ? int.Parse(match.Value) : -1;
        }

        private bool IsValid(string fileName) => Regex.IsMatch(fileName, @"^img_[0-9]{3}.(jpg|png|jpeg)$");

        private PdfDoc CreateNewDocument() => FileService.CreateNewDocument();
    }
}
