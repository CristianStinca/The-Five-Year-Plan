using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;

namespace TFYP.View.Windows
{
    internal abstract class Window
    {
        protected List<IRenderable> SpritesInWindow { get; set; } // list of all the sprites that need to be rendered on the scnene
        private IUIElements _UIElements;

        protected Window(IUIElements UIElements) 
        {
            this.SpritesInWindow = new();
            _UIElements = UIElements;
        }

        public virtual void Update()
        {
            // base for updating the ui while the window is active
        }

        public virtual void Draw(IRenderer renderer)
        {
            renderer.DrawState(this.SpritesInWindow);
        }
    }
}
