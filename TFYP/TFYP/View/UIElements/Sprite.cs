﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TFYP.View.Renders;
using TFYP.View.Windows;

namespace TFYP.View.UIElements
{
    internal class Sprite : ISprite
    {
        protected Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                CollisionRectangle = new Rectangle(_position.ToPoint(), CollisionRectangle.Size);
            }
        }

        public virtual Texture2D Texture { get; set; }
        public virtual Texture2D AltTexture { get; set; }

        public Color Tint { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public Rectangle CollisionRectangle { get; set; }

        public float Scale { get; set; }

        public Sprite(Texture2D texture, Vector2 position, float scale)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            CollisionRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Tint = Color.White;
        }

        public Sprite(Texture2D texture) : this(texture, new Vector2(0, 0), 1f) { }
        public Sprite(Texture2D texture, Vector2 position) : this(texture, position, 1f) { }
        public Sprite(Texture2D texture, float scale) : this(texture, Vector2.Zero, scale) { }

        public Sprite(Texture2D[] texture, Vector2 position, float scale) : this(texture[0], position, scale)
        {
            AltTexture = texture[1];
        }

        public Sprite(Texture2D[] texture) : this(texture, new Vector2(0, 0), 1f) { }
        public Sprite(Texture2D[] texture, Vector2 position) : this(texture, position, 1f) { }
        public Sprite(Texture2D[] texture, float scale) : this(texture, Vector2.Zero, scale) { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SwitchTextures()
        {
            Texture2D temp = this.Texture;
            this.Texture = this.AltTexture;
            this.AltTexture = temp;
        }
    }
}