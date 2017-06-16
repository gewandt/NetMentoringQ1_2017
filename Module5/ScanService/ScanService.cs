using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Topshelf;

namespace ScanService
{
    public class ScanService : ServiceControl
    {
        protected readonly TimeSpan TimePending = TimeSpan.FromSeconds(10);

        protected readonly CancellationTokenSource _cancelToken;
        protected readonly FileService _fileService;

        protected PdfDoc _pdf;

        protected int currentIndex = -1;
        protected int imageCount = 0;
        protected DateTimeOffset _lastImageAddedAt;

        public ScanService()
        {

            _cancelToken = new CancellationTokenSource();
            _fileService = new FileService(_cancelToken.Token);

            _fileService.NewData += _fileService_NewData;

            _pdf = CreateNewDocument();
        }

        protected virtual void _fileService_NewData(IMessage message)
        {
            //todo cancellation token support
            if (message is NoNewData)
            {
                //message arrives after max pause interval
                if (message.CreatedAt > _lastImageAddedAt + TimePending)
                {
                    SavePdf();
                    _pdf = CreateNewDocument();
                }
                return;
            }
            else if (message is Data)
            {
                var data = (Data)message;
                var imageIndex = GetImageIndex(Path.GetFileName(data.Location));

                if (_pdf != null)
                {
                    if (_cancelToken.Token.IsCancellationRequested)
                    {
                        SavePdf();
                        return;
                    }

                    if (_lastImageAddedAt - data.CreatedAt > TimePending)
                    {
                        SavePdf();
                        _pdf = CreateNewDocument();
                    }

                    if (imageIndex != currentIndex + 1 && currentIndex != -1)
                    {
                        SavePdf();
                        _pdf = CreateNewDocument();
                    }
                }

                Debug.Assert(_pdf != null);

                _pdf.AddImage(data.Location);
                currentIndex = imageIndex;
                imageCount++;
                _lastImageAddedAt = DateTimeOffset.UtcNow;
                _fileService.Delete(new FileInfo(data.Location));
            }
        }

        public bool Start(HostControl hostControl)
        {
            _fileService.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _cancelToken.Cancel();
            _fileService.Stop();
            return true;
        }

        protected void SavePdf()
        {
            _pdf.Save(GetNextFilename());
            _pdf = null;
        }

        public string GetNextFilename()
        {
            var documentIndex = Directory.GetFiles(ConfigHelper.PdfDir).Length + 1;
            return Path.Combine(ConfigHelper.PdfDir, $"out_{documentIndex}.pdf");
        }

        protected int GetImageIndex(string fileName)
        {
            var match = Regex.Match(fileName, @"[0-9]{3}");
            return match.Success ? int.Parse(match.Value) : -1;
        }

        public PdfDoc CreateNewDocument() => new PdfDoc();
    }
}
