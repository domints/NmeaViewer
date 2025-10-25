using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui.Views;

namespace NmeaViewer.Tree
{
    public class MessageTreeBuilder : ITreeBuilder<ITreeObject>
    {
        public bool SupportsCanExpand => true;

        public bool CanExpand(ITreeObject toExpand)
        {
            return toExpand is not TreeSentenceValue;
        }

        public IEnumerable<ITreeObject> GetChildren(ITreeObject forObject)
        {
            return forObject.GetChildren();
        }
    }
}