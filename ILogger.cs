using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Abramchik
{
    public interface ILogger
    {
        void Info(Source source, string text);
        void Warn(Source source, string text);
        void Debug(Source source, string text);
        void Error(Source source, string text);
    }
}
