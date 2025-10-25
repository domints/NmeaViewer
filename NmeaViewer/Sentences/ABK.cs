using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Sentences.Enums;

namespace NmeaViewer.Sentences
{
    [SentenceType("ABK")]
    public class ABK : NmeaSentence
    {
        public override string? SentenceId => "ABK";
        public string MMSI { get; set; }
        public char AISChannel { get; set; }
        public decimal MessageId { get; set; }
        public int SequenceNumber { get; set; }
        public ABKAckType AckType { get; set; }


        public ABK(string sentence) : base(sentence)
        {
            MMSI = NextString();
            AISChannel = NextChar();
            MessageId = NextDecimal();
            SequenceNumber = NextInt();
            AckType = (ABKAckType)NextInt();
        }
    }
}