using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TFYP.View.Renders
{
    public class Sprite : IRenderable
    {
        public Sprite(Texture2D texture, Vector2 position, float scale)
        {
            this.Texture = texture;
            this.Position = position;
            this.Scale = scale;
            this.CollisionRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * this.Scale), (int)(texture.Height * this.Scale));
            this.SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            this.Tint = Color.White;
        }

        public Sprite(Texture2D texture) : this(texture, new Vector2(), 1f)
        {
        }

        public Sprite(Texture2D texture, Vector2 position) : this(texture, position, 1f)
        {
        }

        public Vector2 Position { get; set; }

        public virtual Texture2D Texture { get; set; }

        public Color Tint { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public Rectangle CollisionRectangle { get; set; }

        public float Scale { get; set; }
    }
}