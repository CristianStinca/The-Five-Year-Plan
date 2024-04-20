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

    public enum EVPosition
    {
        Center,
        Top,
        Bottom
    }

    public enum EHPosition
    {
        Center,
        Right,
        Left
    }

    public enum ESize
    {
        AllScreen,
        FullVerticaly,
        FullHorizontaly,
        FitContent,
        Custom
    }
}
