using System.Messaging;
using ScanService;
using System.Diagnostics;
using System;
using System.IO;

namespace MessageQueueService
{
    public class MessageQueueAdapter : ScanService.ScanService
    {
        public MessageQueueAdapter() : base() { }

        protected override void _fileService_NewData(IMessage message)
        {
            using (var msgQueue = new MessageQueue(QueueSettings.SERVER))
            {
                var messages = msgQueue.GetAllMessages();
                foreach(var msg in messages)
                {
                    var content = msg.Body as IMessage;
                    if (content is NoNewData)
                    {
                        if (content.CreatedAt > _lastImageAddedAt + TimePending)
                        {
                            SavePdf();
                            _pdf = CreateNewDocument();
                        }
                        return;
                    }
                    else if (content is Data)
                    {
                        var data = (Data)content;
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

                for (int i = 0; i < messages.Length; i++)
                {
                    msgQueue.Receive();
                }
            }
        }
    }
}
