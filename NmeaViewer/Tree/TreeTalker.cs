using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public class TreeTalker : ITreeObject
    {
        public TreeTalker(string id)
        {
            TalkerId = id;
            
        }

        public Dictionary<string, BaseTreeSentence> Sentences = new Dictionary<string, BaseTreeSentence>();

        public string TalkerId { get; }

        public override string ToString() => TalkerId != "P" ? TalkerId : "Proprietary";

        public IEnumerable<ITreeObject> GetChildren()
        {
            return Sentences.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
        }
    }
}