using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer.Sentences;

namespace Tests
{
    public class GllTests
    {
        [Theory]
        [InlineData("$GPGLL,5000.00,N,02020.20,E,000000.00,A*hh", 50)]
        [InlineData("$GPGLL,5030.00,N,02020.20,E,000000.00,A*hh", 50.5)]
        [InlineData("$GPGLL,5015.00,N,02020.20,E,000000.00,A*hh", 50.25)]
        [InlineData("$GPGLL,5045.24,N,02020.20,E,000000.00,A*hh", 50.754)]
        [InlineData("$GPGLL,5030.00,S,02020.20,E,000000.00,A*hh", -50.5)]
        public void ParsesLatitude(string sentence, decimal latitude)
        {
            var result = new GLL(sentence);

            Assert.NotNull(result.Latitude);
            Assert.Equal(latitude, (decimal)result.Latitude);
        }

        [Theory]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000.00,A*hh", 50)]
        [InlineData("$GPGLL,0000.00,N,05030.00,E,000000.00,A*hh", 50.5)]
        [InlineData("$GPGLL,0000.00,N,05015.00,E,000000.00,A*hh", 50.25)]
        [InlineData("$GPGLL,0000.24,N,15045.24,E,000000.00,A*hh", 150.754)]
        [InlineData("$GPGLL,0000.00,S,05030.00,W,000000.00,A*hh", -50.5)]
        public void ParsesLongitude(string sentence, decimal longitude)
        {
            var result = new GLL(sentence);

            Assert.NotNull(result.Longitude);
            Assert.Equal(longitude, (decimal)result.Longitude);
        }

        [Theory]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000.00,A*hh", 0, 0, 0, 0, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,120000.00,A*hh", 12, 0, 0, 0, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,001500.00,A*hh", 0, 15, 0, 0, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000020.00,A*hh", 0, 0, 20, 0, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000.90,A*hh", 0, 0, 0, 900, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000.900,A*hh", 0, 0, 0, 900, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000,A*hh", 0, 0, 0, 0, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000.9,A*hh", 0, 0, 0, 900, 0)]
        [InlineData("$GPGLL,0000.00,N,05000.00,E,000000.9001,A*hh", 0, 0, 0, 900, 100)]
        public void ParsesTime(string sentence, int hr, int min, int sec, int ms, int us)
        {
            var result = new GLL(sentence);

            Assert.Equal(new TimeOnly(hr, min, sec, ms, us), result.Time);
        }
    }
}