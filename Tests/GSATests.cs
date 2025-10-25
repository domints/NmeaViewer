using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer;
using NmeaViewer.Sentences;

namespace Tests
{
    public class GSATests
    {
        [Fact]
        public void TestParsing()
        {
            var sentence = "$GNGSA,A,3,,,,,,,,,,,6.2,1.9,5.9,4*34";
            
            var result = new GSA(sentence);

            Assert.True(result.IsCorrect);
        }
    }
}