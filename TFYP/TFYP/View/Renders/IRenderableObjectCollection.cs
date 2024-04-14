using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.Renders
{
    internal interface IRenderableObjectCollection : IGameObject
    {
        List<IRenderable> ToIRenderable();
    }
}
