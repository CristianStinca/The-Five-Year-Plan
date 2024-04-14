using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.View.Renders;

namespace TFYP.View.UIElements
{
    internal class Text : ITextRenderable
    {
        public SpriteFont Font { get; set; }
        public string TextString { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle CollisionRectangle { get; set; }

        public Text(SpriteFont font, string text, Vector2 position, Color color)
        {
            this.Font = font;
            this.TextString = text;
            this.Position = position;
            this.Color = color;
            this.SourceRectangle = new(Point.Zero, font.MeasureString(text).ToPoint());
            this.CollisionRectangle = new ((int)position.X, (int)position.Y, SourceRectangle.Width, SourceRectangle.Height);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
