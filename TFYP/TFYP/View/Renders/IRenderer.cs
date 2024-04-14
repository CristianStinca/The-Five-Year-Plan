using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.Renders
{
    /// <summary>
    /// This interface is a pre-fab for renderers (for example it can ve used
    /// for some other framework to display, or even for the console)
    /// </summary>
    internal interface IRenderer
    {
        public void DrawState(List<IRenderable> elementsToDraw);
        //public void DrawIRenderable(List<ISprite> spritesToDraw);
        //public void DrawITextRenderable(List<ITextRenderable> textToDraw);
    }
}
