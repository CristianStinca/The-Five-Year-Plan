using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;

namespace TFYP.View.UIElements.ClickableElements
{
    internal class Button
    {
        public delegate void ButtonPressedHandler();
        public event ButtonPressedHandler ButtonPressed;

        public Sprite _sprite { get; set; }

        protected InputHandler _inputHandler { get; }

        public Button(Sprite sprite, InputHandler inputHandler)
        {
            _sprite = sprite;
            _inputHandler = inputHandler;
        }

        public virtual void Update()
        {
            MouseState mouse_state = Mouse.GetState();

            if (IsMouseOverButton(mouse_state))
            {
                Debug.WriteLine("Mouse_Over_BUTTON");
            }

            if (IsMouseOverButton(mouse_state) && _inputHandler.LeftButton == Utils.KeyState.Clicked)
            {
                ButtonPressed.Invoke();
            }
        }

        /// <summary>
        /// Checks if the Mouse coursor is over the Button.
        /// </summary>
        /// <param name="mouse_state">The current mouse state</param>
        /// <returns>True if the coursor is over the Button.</returns>
        public virtual bool IsMouseOverButton(MouseState mouse_state)
        {
            return _sprite.CollisionRectangle.Contains(mouse_state.Position);
        }
    }
}
