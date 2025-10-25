using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Types
{
    public record Longitude(decimal Value) : DecimalBase(Value)
    {
        public override string ToString()
        {
            return $"{Math.Abs(Value):F6} {(Value >= 0m ? 'E' : 'W')}";
        }

        public static implicit operator decimal(Longitude latitude) => latitude.Value;
        public static implicit operator Longitude(decimal value) => new Longitude(value);
    }
}