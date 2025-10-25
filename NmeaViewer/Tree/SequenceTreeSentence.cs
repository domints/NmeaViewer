using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public class SequenceTreeSentence : BaseTreeSentence
    {
        public SequenceTreeSentence(string id) : base(id)
        {
        }

        public Dictionary<int, TreeSentence> Sequence = new();

        public override IEnumerable<ITreeObject> GetChildren()
        {
            return Sequence.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
        }
    }
}