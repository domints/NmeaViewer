using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Sentences;

namespace Tests
{
    public class GGATests
    {
        [Theory]
        [InlineData("$GNGGA,210704.000,,,,,0,00,25.5,,,,,,*7A")]
        public void ParsesTime(string sentence)
        {
            var result = new GGA(sentence);

            Assert.Equal(new TimeOnly(21, 07, 04, 00), result.Time);
        }
    }
}