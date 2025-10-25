using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public class TreeSentenceValue : ITreeObject
    {
        public string Name { get; private set; }
        public string? Value { get; set; }

        public TreeSentenceValue(string name)
        {
            Name = name;
        }

        public override string ToString() => $"{Name}: {Value}";

        public IEnumerable<ITreeObject> GetChildren()
        {
            return Enumerable.Empty<ITreeObject>();
        }
    }
}