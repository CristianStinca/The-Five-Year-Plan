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
using TFYP.View.UIElements;

namespace TFYP.Controller
{
    internal class Button
    {
        public delegate void ButtonPressedHandler();
        public event ButtonPressedHandler ButtonPressed;

        public Sprite _sprite { get; set; }

        protected Texture2D _hoverImage { get; set; }

        protected Texture2D _normalImage { get; set; }

        protected InputHandler _inputHandler { get; }

        public Button(Sprite sprite, Texture2D hoverImage, Texture2D normalImage, InputHandler inputHandler)
        {
            _sprite = sprite;
            _hoverImage = hoverImage;
            _normalImage = normalImage;
            _inputHandler = inputHandler;
        }

        public virtual void Update()
        {
            MouseState mouse_state = Mouse.GetState();

            if (IsMouseOverButton())
            {
                Debug.WriteLine("Mouse_Over_BUTTON");
            }

            if (IsMouseOverButton() && _inputHandler.LeftButton == Utils.KeyState.Clicked)
            {
                ButtonPressed.Invoke();
            }
        }

        public void ChangeToHoverImage()
        {
            _sprite.Texture = _hoverImage;
        }

        public void ChangeToNormalImage()
        {
            _sprite.Texture = _normalImage;
        }

        public virtual bool IsMouseOverButton()
        {
            return _sprite.CollisionRectangle.Contains(Mouse.GetState().Position);
        }
    }
}
