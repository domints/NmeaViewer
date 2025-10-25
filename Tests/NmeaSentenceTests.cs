using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer;
using NmeaViewer.Sentences;
using Xunit;

namespace Tests
{
    public class NmeaSentenceTests
    {
        [Theory]
        [InlineData("$GPGLL,5057.970,N,00146.110,E,142451,A*27")]
        [InlineData("!GPVTG,089.0,T,,,15.2,N,,*7F")]
        public void RecognizesCorrectSentence(string sentence)
        {
            var result = NmeaSentence.FromString(sentence);
            Assert.True(result.IsCorrect);
        }

        [Fact]
        public void Fail_WhenWrongStartingChar()
        {
            var sentence = "%GPGLL,5057.970,N,00146.110,E,142451,A*27";

            var result = NmeaSentence.FromString(sentence);

            Assert.False(result.IsCorrect);
        }

        [Fact]
        public void Fail_WhenWrongAddressLength()
        {
            var sentence = "$GPGLLX,5057.970,N,00146.110,E,142451,A*27";

            var result = NmeaSentence.FromString(sentence);

            Assert.False(result.IsCorrect);
        }

        [Fact]
        public void Fail_WhenWrongChecksum()
        {
            var sentence = "$GPGLL,5057.970,N,00146.110,E,142451,A*28";

            var result = NmeaSentence.FromString(sentence);

            Assert.False(result.IsCorrect);
        }

        [Fact]
        public void Fail_WhenHighChar()
        {
            var sentence = "$GPGLL,5057.970,N,00146.110º,E,142451,A*27";

            var result = NmeaSentence.FromString(sentence);

            Assert.False(result.IsCorrect);
        }

        [Fact]
        public void Success_ParsesSingleValue()
        {
            var sentence = "$GP420,2137*0A";

            var result = new ValueSentence(sentence);

            Assert.NotNull(result.Values);
            Assert.Single(result.Values);
            Assert.Equal("2137", result.Values[0]);
        }

        [Fact]
        public void Success_ParsesMultipleValues()
        {
            var sentence = "$GP420,2137,69*29";

            var result = new ValueSentence(sentence);

            Assert.NotNull(result.Values);
            Assert.Equal(2, result.Values.Count);
            Assert.Equal("2137", result.Values[0]);
            Assert.Equal("69", result.Values[1]);
        }

        [Fact]
        public void Success_RecognizesProprietarySentence()
        {
            var sentence = "$PXDD2137,420*15";

            var result = NmeaSentence.FromString(sentence);

            Assert.True(result.IsCorrect);
            Assert.IsType<Proprietary>(result);
        }

        [Fact]
        public void Success_Proprietary_CorrectSentenceId()
        {
            var sentence = "$PXDD2137,420*15";

            var result = NmeaSentence.FromString(sentence);

            Assert.Equal("XDD", result.SentenceId);
        }

        [Fact]
        public void Success_Proprietary_CorrectData()
        {
            var sentence = "$PXDD2137,420*15";

            var result = (Proprietary)NmeaSentence.FromString(sentence);

            Assert.Equal("2137,420", result.Data);
        }

        [Fact]
        public void TextSearchTest()
        {
            var text = "ABCDEF\r\nGHIJK";

            var newline = text.IndexOf("\r\n");
            var result = text.Substring(0, newline);
            var rest = text.Substring(newline + 2);

            Assert.Equal("ABCDEF", result);
            Assert.Equal("GHIJK", rest);
        }
    }
}