using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace ScanService
{
    public class PdfDoc
    {
        private readonly Document _doc;
        private readonly PdfDocumentRenderer _renderer;
        private readonly Section _section;

        public PdfDoc()
        {
            _doc = new Document();
            _section = _doc.AddSection();
            _renderer = new PdfDocumentRenderer();
        }

        public void AddImage(string filePath)
        {
            var image = _section.AddImage(filePath);
            image.Height = _doc.DefaultPageSetup.PageHeight;
            image.Width = _doc.DefaultPageSetup.PageWidth;
            _section.AddPageBreak();
        }

        public void Save(string filePath)
        {
            _renderer.Document = _doc;
            _renderer.RenderDocument();
            _renderer.Save(filePath);
        }
    }
}
