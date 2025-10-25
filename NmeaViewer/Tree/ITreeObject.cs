using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NmeaViewer.Tree
{
    public interface ITreeObject
    {
        public IEnumerable<ITreeObject> GetChildren();
    }
}