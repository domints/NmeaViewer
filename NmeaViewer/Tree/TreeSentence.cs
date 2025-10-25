using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public class TreeSentence : BaseTreeSentence
    {
        

        public Dictionary<string, TreeSentenceValue> Values = new Dictionary<string, TreeSentenceValue>();

        public TreeSentence(string id) : base(id)
        {
        }

        public override IEnumerable<ITreeObject> GetChildren()
        {
            return Values.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
        }
    }
}