using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NmeaViewer;

namespace NmeaViewer.Sentences
{
    public class ValueSentence : NmeaSentence
    {
        public List<string> Values { get; protected set; }
        public ValueSentence(string sentence) : base(sentence)
        {
            SentenceId = sentence.Substring(2, 3);
            Values = new List<string>();
            ReadOnlySpan<char> value;
            do
            {
                value = NextValue();
                if (!value.IsEmpty)
                    Values.Add(new string(value));
            }
            while (!value.IsEmpty);
        }
    }
}