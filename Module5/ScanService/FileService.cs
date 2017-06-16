using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ScanService
{
    public interface IMessage
    {
        DateTimeOffset CreatedAt { get; }
    }

    public class Data : IMessage
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string Location { get; set; }
    }
    public class NoNewData : IMessage
    {
        public DateTimeOffset CreatedAt { get; set; }
    }

    public delegate void NewDataHandler(IMessage obj);

    public interface IDataProvider
    {
        event NewDataHandler NewData;
        void Start();
        void Stop();
    }

    public class FileService : IDataProvider
    {
        private const int FileRefreshRate = 10000;
        private readonly Task _task;

        public DirectoryInfo ImgDir { get; }
        public DirectoryInfo TmpDir { get; }

        private readonly Regex fileNamePattern = new Regex(@"^img_[0-9]{3}\.(jpg|png|jpeg)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly FileSystemWatcher _fsWatcher;
        private readonly AutoResetEvent _fileCreatedEvent;
        private CancellationToken _token;
        private int _retryFileOpenAttempt;

        public event NewDataHandler NewData;

        public FileService(CancellationToken token)
        {
            _token = token;
            ImgDir = new DirectoryInfo(ConfigHelper.ImageDir); // todo: remove public access, provide folders from outside
            TmpDir = new DirectoryInfo(ConfigHelper.TmpDir);
            _fileCreatedEvent = new AutoResetEvent(false);
            _fsWatcher = new FileSystemWatcher();
            _fsWatcher.Created += (sender, args) => _fileCreatedEvent.Set();
            _task = new Task(() => Work());
            _retryFileOpenAttempt = ConfigHelper.AttemtsCount;

        }

        private void Work()
        {
            while(!_token.IsCancellationRequested)
            {
                foreach (var file in ImgDir.GetFiles())
                {
                    var fileName = file.Name;
                    if (IsValid(fileName))
                    {
                        if (TryOpen(file, _retryFileOpenAttempt))
                        {
                            var newFilePath = Move(file);
                            TriggerNewDataEvent(new Data { CreatedAt = DateTime.UtcNow, Location = newFilePath });
                        }
                    }
                }

                if (!_fileCreatedEvent.WaitOne(FileRefreshRate))
                {
                    TriggerNewDataEvent(new NoNewData { CreatedAt = DateTime.UtcNow });
                }
            }
        }

        private string Move(FileInfo file)
        {
            var moved = Path.Combine(TmpDir.FullName, file.Name);
            if (File.Exists(moved))
                File.Delete(moved);
            File.Move(file.FullName, moved);
            return moved;
        }

        private void TriggerNewDataEvent(IMessage data)
        {
            var ev = NewData;
            if (ev != null)
            {
                ev.Invoke(data);
            }
        }

        private bool IsValid(string fileName) => fileNamePattern.IsMatch(fileName);

        public void Delete(FileInfo file)
        {
            if(TryOpen(file, _retryFileOpenAttempt))
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

        public void Start()
        {
            _task.Start();
        }

        public void Stop()
        {
            _task.Wait();
        }
    }
}
