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

namespace TFYP.View.UIElements
{
    internal class RenderableContainer : BaseRenderableObjectCollection
    {
        private Sprite background;
        private Color _backgroundColor;
        private List<Tuple<EVPosition, EHPosition, IGameObject>> _elementsDetails = null;
        private ESize _size;
        private Vector2 _vectorSize;

        private Rectangle _sourceRectangle;

        public override Rectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            set
            {
                if (_elementsDetails == null)
                {
                    _sourceRectangle = value;
                    return;
                }

                List<Tuple<EVPosition, EHPosition, IGameObject>> temp_list = new List<Tuple<EVPosition, EHPosition, IGameObject>>(_elementsDetails);

                this._elements.Clear();
                background = new Sprite(CreateTexture(value, _backgroundColor), this.Position);
                _elements.Add(background);

                this._elementsDetails.Clear();

                _sourceRectangle = value;
                foreach (Tuple<EVPosition, EHPosition, IGameObject> obj in temp_list)
                {
                    this.AddElement(obj.Item1, obj.Item2, obj.Item3);
                }
            }
        }

        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                List<Tuple<EVPosition, EHPosition, IGameObject>> temp_list = new List<Tuple<EVPosition, EHPosition, IGameObject>>(_elementsDetails);

                this._elements.Clear();

                if (background != null)
                {
                    background.Position = value;
                    _elements.Add(background);
                }

                this._elementsDetails.Clear();

                _position = value;
                foreach (Tuple<EVPosition, EHPosition, IGameObject> obj in temp_list)
                {
                    this.AddElement(obj.Item1, obj.Item2, obj.Item3);
                }
            }
        }

        //public enum EVPosition
        //{
        //    Center,
        //    Top, 
        //    Bottom
        //}

        //public enum EHPosition
        //{
        //    Center,
        //    Right,
        //    Left
        //}

        //public enum ESize
        //{
        //    AllScreen,
        //    FullVerticaly,
        //    FullHorizontaly,
        //    FitContent,
        //    Custom
        //}

        public RenderableContainer(Vector2 position, Vector2 size) : base(position)
        {
            this.SourceRectangle = new Rectangle(Point.Zero, size.ToPoint());
            this.CollisionRectangle = new Rectangle(Position.ToPoint(), SourceRectangle.Size);
            _elementsDetails = new();
            _size = ESize.Custom;
        }

        public RenderableContainer(int x, int y, Vector2 size) : this(new Vector2(x, y), size) { }
        public RenderableContainer(int x, int y, Vector2 size, Color backgroundColor) : this(new Vector2(x, y), size)
        {
            background = new Sprite(CreateTexture(SourceRectangle, backgroundColor), this.Position);
            _elements.Add(background);
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

            this.CollisionRectangle = new Rectangle(Position.ToPoint(), SourceRectangle.Size);
            _elementsDetails = new();
        }

        public RenderableContainer(int x, int y, ESize size) : this(new Vector2(x, y), size) { }

        public RenderableContainer(int x, int y, ESize size, Color backgroundColor) : this(new Vector2(x, y), size)
        {
            _backgroundColor = backgroundColor;
            _size = size;

            if (_size != ESize.FitContent)
            {
                background = new Sprite(CreateTexture(SourceRectangle, backgroundColor), this.Position);
                _elements.Add(background);
            }
        }

        public void AddElement(EVPosition vertical_allignment, EHPosition horizontal_allignment, IGameObject element)
        {
            float x = 0f, y = 0f;

            switch (vertical_allignment)
            {
                case EVPosition.Center:
                    y = (this.SourceRectangle.Height - element.SourceRectangle.Height) / 2.0f; break;

                case EVPosition.Top:
                    y = 0; break;

                case EVPosition.Bottom:
                    y = this.SourceRectangle.Height - element.SourceRectangle.Height; break;
            }

            y += this.Position.Y;

            switch (horizontal_allignment)
            {
                case EHPosition.Center:
                    x = (this.SourceRectangle.Width - element.SourceRectangle.Width) / 2.0f; break;

                case EHPosition.Left:
                    x = 0; break;

                case EHPosition.Right:
                    x = this.SourceRectangle.Width - element.SourceRectangle.Width; break;
            }

            x += this.Position.X;

            element.Position = new Vector2(x, y);
            element.CollisionRectangle = new Rectangle(element.Position.ToPoint(), SourceRectangle.Size);

            _elements.Add(element);
            _elementsDetails.Add(new Tuple<EVPosition, EHPosition, IGameObject>(vertical_allignment, horizontal_allignment, element));

            if (_size != ESize.AllScreen || _size != ESize.Custom)
            {
                int? width = null, height = null;
                if (element.SourceRectangle.Width > this.SourceRectangle.Width)
                {
                    width = element.SourceRectangle.Width;
                }

                if (element.SourceRectangle.Height > this.SourceRectangle.Height)
                {
                    height = element.SourceRectangle.Height;
                }

                switch (_size)
                {
                    case ESize.FitContent:
                        {
                            if (width != null || height != null)
                            {
                                this.SourceRectangle = new Rectangle(0, 0, (int)width, (int)height);
                            }
                            break;
                        }

                    case ESize.FullVerticaly:
                        {
                            if (width != null)
                            {
                                this.SourceRectangle = new Rectangle(0, 0, (int)width, SourceRectangle.Height);
                            }
                            break;
                        }

                    case ESize.FullHorizontaly:
                        {
                            if (height != null)
                            {
                                this.SourceRectangle = new Rectangle(0, 0, SourceRectangle.Width, (int)height);
                            }
                            break;
                        }
                }
            }
        }

        private Texture2D CreateTexture(Rectangle rectangle, Color color)
        {
            int width = rectangle.Width;
            int height = rectangle.Height;

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

            return texture;
        }
    }
}
