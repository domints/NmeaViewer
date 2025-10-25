using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Sentences
{
    public class Proprietary : NmeaSentence
    {
        public string? Data { get; protected set; }
        public Proprietary(string sentence)
        {
            TalkerId = "P";
            SentenceId = sentence.Substring(2, 3);
            var checksumIndex = sentence.IndexOf('*');
            if (checksumIndex == -1)
            {
                IsCorrect = false;
                return;
            }

            if (!IsChecksumOk(sentence))
            {
                IsCorrect = false;
                return;
            }

            Data = sentence.Substring(5, checksumIndex - 5);

            IsCorrect = true;
        }
    }
}