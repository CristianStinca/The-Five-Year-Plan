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
    internal class Button : IRenderableObject
    {
        public delegate void ButtonPressedHandler(string name);
        public event ButtonPressedHandler ButtonPressed;

        public Sprite Sprite { get; set; }

        protected InputHandler _inputHandler { get; }
        public virtual Vector2 Position { get => Sprite.Position; set => Sprite.Position = value; }
        public virtual Rectangle SourceRectangle { get => Sprite.SourceRectangle; set => Sprite.SourceRectangle = value; }
        public virtual Rectangle CollisionRectangle { get => Sprite.CollisionRectangle; set => Sprite.CollisionRectangle = value; }

        public Button(Sprite sprite, InputHandler inputHandler)
        {
            Sprite = sprite;
            _inputHandler = inputHandler;
        }

        public virtual void Update()
        {
            MouseState mouse_state = Mouse.GetState();

            if (IsMouseOverButton(mouse_state) && _inputHandler.LeftButton == Utils.KeyState.Clicked)
            {
                ButtonPressed.Invoke(Sprite.Texture.Name);
            }
        }

        /// <summary>
        /// Checks if the Mouse coursor is over the Button.
        /// </summary>
        /// <param name="mouse_state">The current mouse state</param>
        /// <returns>True if the coursor is over the Button.</returns>
        public virtual bool IsMouseOverButton(MouseState mouse_state)
        {
            return Sprite.CollisionRectangle.Contains(mouse_state.Position);
        }

        public IRenderable ToIRenderable()
        {
            return this.Sprite;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
