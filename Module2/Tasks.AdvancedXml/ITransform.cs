using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.AdvancedXml
{
    public interface ITransform
    {
        void TransformToRss(string sourceXml, string pathToXsltTemplate, string pathToResultFile);
        void TransformToHtmlReport(string pathToXsltTemplate, FileStream input, FileStream output);
    }
}
