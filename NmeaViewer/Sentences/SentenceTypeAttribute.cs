using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Sentences
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SentenceTypeAttribute : Attribute
    {
        public string SequenceType { get; set; }
        public SentenceTypeAttribute(string type)
        {
            SequenceType = type;  
        }
    }
}