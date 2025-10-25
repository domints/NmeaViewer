using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Tree;

namespace NmeaViewer.Sentences
{
    [SentenceType("AAM")]
    public class AAM : NmeaSentence
    {
        public override string? SentenceId => "AAM";

        public bool ArrivalEntered { get; set; }
        public bool PerpendicularPassed { get; set; }

        [SkipValue]
        public decimal ArrivalCircleRadiusValue { get; set; }
        [SkipValue]
        public string RadiusUnit => "NM";

        public string ArrivalCircleRadius => $"{ArrivalCircleRadiusValue} {RadiusUnit}";

        public string? WaypointId { get; set; }

        public AAM(string sentence) : base(sentence)
        {
            var entered = NextBool();
            var passed = NextBool();
            if (!entered.HasValue || !passed.HasValue)
                return;

            ArrivalCircleRadiusValue = NextDecimal();
            NextValue(); // ignore unit because it's supposedly always nautical miles
            WaypointId = NextString();
        }

    }
}