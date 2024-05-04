using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.View.Renders;

namespace TFYP.View.UIElements
{
    internal class BitmapText : ITextRenderable
    {
        public BitmapFont BTFont { get; set; }
        public string TextString { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle CollisionRectangle { get; set; }
        public SpriteFont Font { get; set; }

        public BitmapText(BitmapFont font, string text, Vector2 position, Color color)
        {
            this.Font = null;
            this.BTFont = font;
            this.TextString = text;
            this.Position = position;
            this.Color = color;
            this.SourceRectangle = new(Point.Zero, ((Point)font.MeasureString(text)));
            this.CollisionRectangle = new((int)position.X, (int)position.Y, SourceRectangle.Width, SourceRectangle.Height);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
