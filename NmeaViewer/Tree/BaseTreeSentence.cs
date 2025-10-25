using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public abstract class BaseTreeSentence : ITreeObject
    {
        public BaseTreeSentence(string id)
        {
            SentenceId = id;
        }

        public string SentenceId { get; }

        public override string ToString() => SentenceId;

        public abstract IEnumerable<ITreeObject> GetChildren();
    }
}