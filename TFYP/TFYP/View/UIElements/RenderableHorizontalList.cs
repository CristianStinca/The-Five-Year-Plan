using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.View.Renders;

namespace TFYP.View.UIElements
{
    internal class RenderableHorizontalList : BaseRenderableObjectCollection
    {
        protected int _space = 0;
        protected int _width = 0;

        private EVPosition? _vPosition;

        public override Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _width = 0;
                this.CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(SourceRectangle.Width, SourceRectangle.Height));

                List<IGameObject> temp_list = new List<IGameObject>(_elements);
                this._elements.Clear();

                foreach (IGameObject obj in temp_list)
                {
                    this.AddElement(obj);
                }
            }
        }
        public int Width { get { return _width; } }
        public int Space { get { return _space; } }

        public RenderableHorizontalList(int space, Vector2 position) : base(position)
        {
            _space = space;
            _position = position;
            _vPosition = null;
        }

        public RenderableHorizontalList(int space, int x, int y) : this(space, new Vector2(x, y)) { }
        public RenderableHorizontalList(int space, int x, int y, EVPosition vertical_position) : this(space, new Vector2(x, y))
        {
            _vPosition = vertical_position;
        }

        public void AddElement(IGameObject renderable)
        {
            IGameObject element = renderable;

            int maxHeight = Math.Max(SourceRectangle.Height, element.CollisionRectangle.Height);

            float y = Position.Y;
            if (_vPosition != null)
            {
                switch ((EHPosition)_vPosition)
                {
                    case EHPosition.Right:
                        y += maxHeight - element.CollisionRectangle.Height; break;

                    case EHPosition.Center:
                        y += (maxHeight - element.CollisionRectangle.Height) / 2.0f; break;
                }
            }

            element.Position = new Vector2(Position.X + _width, y);

            _elements.Add(element);
            _width += element.CollisionRectangle.Width;
            this.SourceRectangle = new Rectangle(0, 0, _width, maxHeight);
            this.CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(SourceRectangle.Width, SourceRectangle.Height));
            _width += _space;
        }
    }
}
