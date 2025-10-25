using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Types
{
    public record Latitude(decimal Value) : DecimalBase(Value)
    {

        public override string ToString()
        {
            return $"{Math.Abs(Value):F6} {(Value >= 0m ? 'N' : 'S')}";
        }

        public static implicit operator decimal(Latitude latitude) => latitude.Value;
        public static implicit operator Latitude(decimal value) => new Latitude(value);
    }
}