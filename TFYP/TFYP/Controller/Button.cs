using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.View.Renders;
using TFYP.View.UIElements;

namespace TFYP.Controller
{
    internal class Button
    {
        public Sprite _sprite { get; set; }

        private Texture2D _hoverImage { get; set; }

        private Texture2D _normalImage { get; set; }

        public Button(Sprite sprite, Texture2D hoverImage, Texture2D normalImage)
        {
            _sprite = sprite;
            _hoverImage = hoverImage;
            _normalImage = normalImage;
        }

        public void ChangeToHoverImage()
        {
            _sprite.Texture = _hoverImage;
        }

        public void ChangeToNormalImage()
        {
            _sprite.Texture = _normalImage;
        }
    }
}
