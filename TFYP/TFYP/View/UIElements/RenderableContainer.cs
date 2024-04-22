using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.View.Renders;
using TFYP.Utils;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace TFYP.View.UIElements
{
    internal class RenderableContainer : BaseRenderableObjectCollection
    {
        private Color? _backgroundColor;
        private List<Tuple<EVPosition, EHPosition, IGameObject>> _elementsDetails = null;
        private ESize _size;

        private Rectangle _sourceRectangle;
        private Rectangle _collisionRectangle;

        private float padding = 0; 
        public float Padding
        {
            get => padding;
            set
            {
                padding = value;

                if (_size == ESize.AllScreen || _size == ESize.Custom)
                    _sourceRectangle = new Rectangle(0, 0, _sourceRectangle.Width - ((int)padding * 2), _sourceRectangle.Height - ((int)padding * 2));
                
                _collisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _sourceRectangle.Width + (int)(margin + padding) * 2, _sourceRectangle.Height + (int)(margin + padding) * 2);
                
                UpdateList();
            }
        }

        private float margin = 0;
        public float Margin
        {
            get => margin;
            set
            {
                margin = value;

                if (_size == ESize.AllScreen || _size == ESize.Custom)
                    _sourceRectangle = new Rectangle(0, 0, _sourceRectangle.Width - ((int)margin * 2), _sourceRectangle.Height - ((int)margin * 2));

                _collisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _sourceRectangle.Width + (int)(margin + padding) * 2, _sourceRectangle.Height + (int)(margin + padding) * 2);

                UpdateList();
            }
        }

        public override Rectangle CollisionRectangle
        {
            get => _collisionRectangle;
        }

        public override Rectangle SourceRectangle
        {
            get => _sourceRectangle;
            set
            {
                _sourceRectangle = value;
                _collisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _sourceRectangle.Width + (int)(margin + padding) * 2, _sourceRectangle.Height + (int)(margin + padding) * 2);
            }
        }

        public override Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _collisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _sourceRectangle.Width + (int)(margin + padding) * 2, _sourceRectangle.Height + (int)(margin + padding) * 2);
                UpdateList();
            }
        }

        private void UpdateList()
        {
            if (_elementsDetails == null)
                return;

            List<Tuple<EVPosition, EHPosition, IGameObject>> temp_list = new List<Tuple<EVPosition, EHPosition, IGameObject>>(_elementsDetails);

            this._elements.Clear();
            this._elementsDetails.Clear();

            foreach (Tuple<EVPosition, EHPosition, IGameObject> obj in temp_list)
            {
                this.AddElement(obj.Item1, obj.Item2, obj.Item3);
            }
        }

        public RenderableContainer(Vector2 position, Vector2 size) : base(position)
        {
            this._sourceRectangle = new Rectangle(Point.Zero, size.ToPoint());
            this._collisionRectangle = new Rectangle(_position.ToPoint(), size.ToPoint());
            _elementsDetails = new();
            _size = ESize.Custom;

            _backgroundColor = null;
        }
        public RenderableContainer(int x, int y, Vector2 size) : this(new Vector2(x, y), size) { }
        public RenderableContainer(int x, int y, Vector2 size, Color backgroundColor) : this(new Vector2(x, y), size)
        {
            _backgroundColor = backgroundColor;
        }

        public RenderableContainer(Vector2 position, ESize size) : base(position)
        {
            switch (size)
            {
                case ESize.AllScreen:
                    this.SourceRectangle = new Rectangle(Point.Zero, new Point(Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight)); break;

                case ESize.FullHorizontaly:
                    this.SourceRectangle = new Rectangle(Point.Zero, new Point(Globals.Graphics.PreferredBackBufferWidth, 0)); break;

                case ESize.FullVerticaly:
                    this.SourceRectangle = new Rectangle(Point.Zero, new Point(0, Globals.Graphics.PreferredBackBufferHeight)); break;

                case ESize.FitContent:
                    this.SourceRectangle = new Rectangle(Point.Zero, Point.Zero); break;
            }

            _elementsDetails = new();
            _backgroundColor = null;
        }
        public RenderableContainer(int x, int y, ESize size) : this(new Vector2(x, y), size) { }
        public RenderableContainer(int x, int y, ESize size, Color backgroundColor) : this(new Vector2(x, y), size)
        {
            _backgroundColor = backgroundColor;
            _size = size;
        }

        public void AddElement(EVPosition vertical_allignment, EHPosition horizontal_allignment, IGameObject element)
        {
            if (_size != ESize.AllScreen || _size != ESize.Custom)
            {
                int width = Math.Max(element.CollisionRectangle.Width, this.SourceRectangle.Width);
                int height = Math.Max(element.CollisionRectangle.Height, this.SourceRectangle.Height);

                switch (_size)
                {
                    case ESize.FitContent:
                        this.SourceRectangle = new Rectangle(Point.Zero, new Point((int)width, (int)height));
                        break;

                    case ESize.FullVerticaly:
                        this.SourceRectangle = new Rectangle(Point.Zero, new Point((int)width, SourceRectangle.Height));
                        break;

                    case ESize.FullHorizontaly:
                        this.SourceRectangle = new Rectangle(Point.Zero, new Point(SourceRectangle.Width, (int)height));
                        break;
                }
            }

            float x = 0f, y = 0f;

            switch (vertical_allignment)
            {
                case EVPosition.Center:
                    y = ((this.CollisionRectangle.Height) - element.CollisionRectangle.Height) / 2.0f; break;

                case EVPosition.Top:
                    y = padding + margin; break;

                case EVPosition.Bottom:
                    y = (this.CollisionRectangle.Height - padding - margin) - element.CollisionRectangle.Height; break;
            }

            y += this.Position.Y;

            switch (horizontal_allignment)
            {
                case EHPosition.Center:
                    x = ((this.CollisionRectangle.Width) - element.CollisionRectangle.Width) / 2.0f; break;

                case EHPosition.Left:
                    x = padding + margin; break;

                case EHPosition.Right:
                    x = (this.CollisionRectangle.Width - padding - margin) - element.CollisionRectangle.Width; break;
            }

            x += this.Position.X;

            element.Position = new Vector2(x, y);

            _elements.Add(element);
            _elementsDetails.Add(new Tuple<EVPosition, EHPosition, IGameObject>(vertical_allignment, horizontal_allignment, element));
        }

        private Sprite CreateBackground()
        {
            Color color = (Color)_backgroundColor;

            int width = SourceRectangle.Width + (int)(padding * 2);
            int height = SourceRectangle.Height + (int)(padding * 2);

            if (width <= 0 || height <= 0) { return null; }

            //initialize a texture
            Texture2D texture = new Texture2D(Globals.Graphics.GraphicsDevice, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = color;
            }

            //set the color
            texture.SetData(data);

            return new Sprite(texture, new Vector2(Position.X + margin, Position.Y + margin));
        }

        public override List<IRenderable> ToIRenderable()
        {
            List<IRenderable> list = base.ToIRenderable();
            if (_backgroundColor != null)
            {
                list.Insert(0, CreateBackground());
            }
            
            return list;
        }
    }
}
