using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Types;
using Serilog;

namespace NmeaViewer.Sentences
{
    [SentenceType("GLL")]
    public class GLL : NmeaSentence
    {
        public override string? SentenceId => "GLL";

        public Latitude? Latitude { get; private set; }
        public Longitude? Longitude { get; private set; }
        public TimeOnly Time { get; private set; }
        public bool IsValid { get; private set; }

        public GLL(string sentence) : base(sentence)
        {
            Log.Debug("GLL parsing sentence: {sentence}", sentence);
            var lat = NextLatitude();
            var lon = NextLongitude();
            Time = NextTime();
            var valid = NextBool();
            if (!valid.HasValue || lat is null || lon is null)
                return;

            IsValid = valid.Value;
            Latitude = lat.Value;
            Longitude = lon.Value;
        }
    }
}