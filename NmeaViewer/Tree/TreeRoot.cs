using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public class TreeRoot : ITreeObject
    {
        public Dictionary<string, TreeTalker> Talkers = new Dictionary<string, TreeTalker>();

        public IEnumerable<ITreeObject> GetChildren()
        {
            return Talkers.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
        }

        public override string ToString() => "Messages";
    }
}