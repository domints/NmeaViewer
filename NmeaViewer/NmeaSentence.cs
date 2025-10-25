using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using NmeaViewer.Sentences;
using NmeaViewer.Tree;
using NmeaViewer.Types;

namespace NmeaViewer
{
    public class NmeaSentence : IBaseSentence
    {
        [SkipValue]
        public bool IsCorrect { get; protected set; }
        [SkipValue]
        public string? Sentence { get; protected set; }
        [SkipValue]
        public char StartChar { get; protected set; }
        [SkipValue]
        public string? TalkerId { get; protected set; }
        [SkipValue]
        public virtual string? SentenceId { get; protected set; }

        private int _lastCommaFound = 6;
        private int _checksumIndex;

        protected NmeaSentence()
        {

        }

        public NmeaSentence(string sentence)
        {
            Sentence = sentence;
            TalkerId = sentence.Substring(1, 2);
            _checksumIndex = sentence.IndexOf('*');
            if (_checksumIndex == -1)
            {
                IsCorrect = false;
                return;
            }

            IsCorrect = true;
        }

        protected ReadOnlySpan<char> NextValue()
        {
            if (Sentence is null)
                return ReadOnlySpan<char>.Empty;
            if (_lastCommaFound == -1)
                return ReadOnlySpan<char>.Empty;

            var newComma = Sentence.IndexOf(',', _lastCommaFound + 1, _checksumIndex - _lastCommaFound);
            var len = newComma == -1 ? _checksumIndex - _lastCommaFound - 1 : newComma - _lastCommaFound - 1;
            var lastComma = _lastCommaFound;
            _lastCommaFound = newComma;
            return Sentence.AsSpan(lastComma + 1, len);
        }

        protected bool? NextBool()
        {
            var val = NextValue();
            if (val.Length != 1)
                return null;
            switch (val[0])
            {
                case 'A':
                    return true;
                case 'V':
                    return false;
                default:
                    return null;
            }
        }

        protected decimal NextDecimal()
        {
            return decimal.Parse(NextValue(), CultureInfo.InvariantCulture);
        }

        protected decimal? NextNullableDecimal()
        {
            var val = NextValue();
            return val.Length > 0 ? decimal.Parse(val, CultureInfo.InvariantCulture) : null;
        }

        protected int NextInt()
        {
            return int.Parse(NextValue(), CultureInfo.InvariantCulture);
        }

        protected int? NextNullableInt()
        {
            var val = NextValue();
            return val.Length > 0 ? int.Parse(val, CultureInfo.InvariantCulture) : null;
        }

        protected string NextString()
        {
            return NextValue().ToString();
        }

        protected char NextChar()
        {
            return NextValue()[0];
        }

        protected Latitude? NextLatitude()
        {
            var coordVal = NextValue();
            if (coordVal.Length == 0)
            {
                NextValue(); // Ignore direction because coord was empty.
                return null;
            }
            var coordDegree = decimal.Parse(coordVal[0..2], CultureInfo.InvariantCulture);
            var coordMinute = decimal.Parse(coordVal[2..], CultureInfo.InvariantCulture);
            var coord = coordDegree + coordMinute / 60;
            var dir = NextValue();
            if (dir.Length != 1)
                return null;
            switch (dir[0])
            {
                case 'N':
                    return coord;
                case 'S':
                    return coord * -1;
                default:
                    return null;
            }
        }

        protected Longitude? NextLongitude()
        {
            var coordVal = NextValue();
            if (coordVal.Length == 0)
            {
                NextValue(); // Ignore direction because coord was empty.
                return null;
            }
            var coordDegree = decimal.Parse(coordVal[0..3], CultureInfo.InvariantCulture);
            var coordMinute = decimal.Parse(coordVal[3..], CultureInfo.InvariantCulture);
            var coord = coordDegree + coordMinute / 60;
            var dir = NextValue();
            if (dir.Length != 1)
                return null;
            switch (dir[0])
            {
                case 'E':
                    return coord;
                case 'W':
                    return coord * -1;
                default:
                    return null;
            }
        }
        
        protected TimeOnly NextTime()
        {
            var timeVal = NextValue();
            var hh = int.Parse(timeVal[0..2]);
            var mm = int.Parse(timeVal[2..4]);
            var ss = int.Parse(timeVal[4..6]);
            var ms = 0;
            var us = 0;
            if (timeVal.Length > 6)
            {
                if (timeVal.Length > 10)
                {
                    ms = int.Parse(timeVal[7..10]);
                    if (timeVal.Length > 13)
                    {
                        us = int.Parse(timeVal[10..13]);
                    }
                    else
                    {
                        var usSpan = timeVal[10..];
                        var mul = (int)Math.Pow(10, 3 - usSpan.Length);
                        us = int.Parse(usSpan) * mul;
                    }
                }
                else
                {
                    var msSpan = timeVal[7..];
                    var mul = (int)Math.Pow(10, 3 - msSpan.Length);
                    ms = int.Parse(msSpan) * mul;
                }
            }

            return new TimeOnly(hh, mm, ss, ms, us);
        }

        public static NmeaSentence FromString(string sentence)
        {
            var startCharOk = sentence.StartsWith('$') || sentence.StartsWith('!');
            if (!startCharOk)
                return new NmeaSentence { IsCorrect = false, StartChar = sentence[0] };
            if (!IsChecksumOk(sentence))
                return new NmeaSentence { IsCorrect = false };
            if (sentence[1] == 'P')
                    return new Proprietary(sentence);
            return SentenceLibrary.ParseSentence(sentence);
        }

        protected static bool IsChecksumOk(string sentence)
        {
            try
            {
                var checksum = 0;
                var stopCharIx = 0;
                for (int i = 1; i < sentence.Length; i++)
                {
                    if (sentence[i] == '*')
                    {
                        stopCharIx = i;
                        break;
                    }
                    if (sentence[i] > 0xEF)
                        return false;

                    checksum ^= sentence[i];
                }

                var sentChecksum = Convert.ToUInt16(sentence.Substring(stopCharIx + 1, 2), 16);
                if (sentChecksum == checksum)
                    return true;
            }
            catch { }

            return false;
        }
    }
}