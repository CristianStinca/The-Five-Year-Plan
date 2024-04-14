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
    internal class ButtonHoverable : Button
    {
        public delegate void ButtonHoverablePressedHandler();
        public event ButtonHoverablePressedHandler ButtonHoverablePressed;

        protected Texture2D _hoverImage { get; set; }

        protected Texture2D _normalImage { get; set; }

        public ButtonHoverable(Sprite sprite, Texture2D hoverImage, InputHandler inputHandler) : base(sprite, inputHandler)
        {
            _hoverImage = hoverImage;
            _normalImage = sprite.Texture;
        }

        /// <summary>
        /// Chanfes the Button texture to the one designated for hover.
        /// </summary>
        public void ChangeToHoverImage()
        {
            Sprite.Texture = _hoverImage;
        }

        /// <summary>
        /// Changes the Button texture to the one designated for normal view.
        /// </summary>
        public void ChangeToNormalImage()
        {
            Sprite.Texture = _normalImage;
        }
    }
}
