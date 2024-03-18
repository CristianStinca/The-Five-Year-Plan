using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.Renders
{
    internal interface IRenderer
    {
        /*
            This interface is a pre-fab for potential future renderers (for example we could use
            some other framework to display, or even do it in the console)
        */

        public void DrawState(List<IRenderable> spritesToDraw);
    }
}
