using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Types
{
    public record DecimalBase(decimal Value) : IEquatable<decimal>
    {
        public bool Equals(decimal other)
        {
            return Value == other;
        }
    }
}