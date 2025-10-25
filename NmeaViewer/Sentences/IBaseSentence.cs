using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Sentences
{
    public interface IBaseSentence
    {
        public string? SentenceId { get; }
    }
}