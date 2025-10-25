using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Sentences
{
    [SentenceType("ALM")]
    public class ALM : NmeaSentence, ISequenceSentence
    {
        public override string? SentenceId => "ALM";

        public int TotalMessages { get ; set; }
        public int MessageNumber { get; set; }
        public int PRNNumber { get; set; }
        public int GPSWeekNumber { get; set; }
        public string? SVHealth { get; set; }
        public string? Eccentricity { get; set; }
        public string? AlmanacRefTime { get; set; }
        public string? InclinationAngle { get; set; }
        public string? RateOfRightAscension { get; set; }
        public string? RootOfSemiMajorAxis { get; set; }
        public string? ArgumentOfPerigee { get; set; }
        public string? LongitudeOfAscensionMode { get; set; }
        public string? MeanAnomaly { get; set; }
        public string? F0ClockParam { get; set; }
        public string? F1ClockParam { get; set; }

        public ALM(string sentence) : base(sentence)
        {
            TotalMessages = NextInt();
            MessageNumber = NextInt();
            PRNNumber = NextInt();
            GPSWeekNumber = NextInt();
            SVHealth = NextString();
            Eccentricity = NextString();
            AlmanacRefTime = NextString();
            InclinationAngle = NextString();
            RateOfRightAscension = NextString();
            RootOfSemiMajorAxis = NextString();
            ArgumentOfPerigee = NextString();
            LongitudeOfAscensionMode = NextString();
            MeanAnomaly = NextString();
            F0ClockParam = NextString();
            F1ClockParam = NextString();
        }
    }
}