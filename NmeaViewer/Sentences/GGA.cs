using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Sentences.Enums;
using NmeaViewer.Types;
using Serilog;

namespace NmeaViewer.Sentences
{
    [SentenceType("GGA")]
    public class GGA : NmeaSentence
    {
        public override string? SentenceId => "GGA";
        public TimeOnly Time { get; set; }
        public Latitude? Latitude { get; set; }
        public Longitude? Longitude { get; set; }
        public GGAFixQuality FixQuality { get; set; }
        public int SatellitesInView { get; set; }
        public decimal HorizontalDilution { get; set; }
        public decimal? Altitude { get; set; }
        public string AltitudeUnit => "m";
        public decimal? GeoidalSeparation { get; set; }
        public string GeoidalSeparationUnit => "m";
        public int? DifferentialDataAge { get; set; }
        public int? DifferentialStationId { get; set; }

        public GGA(string sentence) : base(sentence)
        {
            Log.Debug("GGA parsing sentence: {sentence}", sentence);
            Time = NextTime();
            Log.Information("Found time, {time}", Time);
            Latitude = NextLatitude();
            Longitude = NextLongitude();
            FixQuality = (GGAFixQuality)NextInt();
            SatellitesInView = NextInt();
            HorizontalDilution = NextDecimal();
            Altitude = NextNullableDecimal();
            NextValue(); // Unit of altitude is fixed
            GeoidalSeparation = NextNullableDecimal();
            NextValue(); // Unit of geoidal separation is fixed
            DifferentialDataAge = NextNullableInt();
            DifferentialStationId = NextNullableInt();
        }
    }
}