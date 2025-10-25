using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Sentences.Enums;
using Serilog;

namespace NmeaViewer.Sentences
{
    [SentenceType("GSA")]
    public class GSA : NmeaSentence
    {
        public override string? SentenceId => "GSA";

        public GSASelectionMode SelectionMode { get; set; }
        public GSAMode Mode { get; set; }
        public int? Sat01Id { get; set; }
        public int? Sat02Id { get; set; }
        public int? Sat03Id { get; set; }
        public int? Sat04Id { get; set; }
        public int? Sat05Id { get; set; }
        public int? Sat06Id { get; set; }
        public int? Sat07Id { get; set; }
        public int? Sat08Id { get; set; }
        public int? Sat09Id { get; set; }
        public int? Sat10Id { get; set; }
        public int? Sat11Id { get; set; }
        public int? Sat12Id { get; set; }
        public decimal? PDOP { get; set; }
        public decimal? HDOP { get; set; }
        public decimal? VDOP { get; set; }
        
        public GSA(string sentence) : base(sentence)
        {
            Log.Debug("GSA parsing sentence: {sentence}", sentence);
            var selMode = NextChar();
            SelectionMode = selMode == 'M' ? GSASelectionMode.Manual : GSASelectionMode.Automatic;
            Mode = (GSAMode)NextInt();
            Sat01Id = NextNullableInt();
            Sat02Id = NextNullableInt();
            Sat03Id = NextNullableInt();
            Sat04Id = NextNullableInt();
            Sat05Id = NextNullableInt();
            Sat06Id = NextNullableInt();
            Sat07Id = NextNullableInt();
            Sat08Id = NextNullableInt();
            Sat09Id = NextNullableInt();
            Sat10Id = NextNullableInt();
            Sat11Id = NextNullableInt();
            Sat12Id = NextNullableInt();
            PDOP = NextNullableDecimal();
            HDOP = NextNullableDecimal();
            VDOP = NextNullableDecimal();
        }
    }
}