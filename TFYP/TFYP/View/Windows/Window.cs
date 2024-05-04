using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using TFYP.View.UIElements.ClickableElements;

namespace TFYP.View.Windows
{
    internal abstract class Window
    {
        public delegate void UIButtonPressedHandler();
        protected List<IRenderable> ElementsInWindow { get; set; } // list of all the sprites that need to be rendered on the scnene
        protected UIObjects _UIElements;
        protected InputHandler _inputHandler;
        protected List<Button> _UpdateingUIElements;

        protected Window(IUIElements UIElements, InputHandler inputHandler) 
        {
            this.ElementsInWindow = new();
            this._UpdateingUIElements = new();
            this._UIElements = (UIObjects)UIElements;
            this._inputHandler = inputHandler;
        }

        protected virtual Button AddButton(Sprite sprite)
        {
            Button btn = new Button(sprite, _inputHandler);
            _UpdateingUIElements.Add(btn);
            return btn;
        }

        public virtual void Update()
        {
            // base for updating the ui while the window is active
            foreach (Button btn in _UpdateingUIElements)
            {
                btn?.Update();
            }
        }

        public virtual void Draw(IRenderer renderer)
        {
            renderer.DrawState(this.ElementsInWindow);
            //renderer.DrawITextRenderable(this.TextInWindow);
        }
    }
}
