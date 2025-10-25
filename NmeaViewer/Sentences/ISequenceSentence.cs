using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Sentences
{
    public interface ISequenceSentence
    {
        int TotalMessages { get; set; }
        int MessageNumber { get; set; }
    }
}