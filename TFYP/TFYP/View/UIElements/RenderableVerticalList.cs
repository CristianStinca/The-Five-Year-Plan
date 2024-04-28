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
    internal class RenderableVerticalList : BaseRenderableObjectCollection
    {
        protected int _space = 0;
        protected int _length = 0;

        private EHPosition? _hPosition;

        public override Vector2 Position { 
            get
            {
                return _position;
            } 
            set
            {
                _position = value;
                _length = 0;
                this.CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(SourceRectangle.Width, SourceRectangle.Height));
                
                List<IGameObject> temp_list = new List<IGameObject>(_elements);
                this._elements.Clear();

                foreach (IGameObject obj in temp_list)
                {
                    this.AddElement(obj);
                }
            }
        }
        public int Length { get { return _length; } }
        public int Space { get { return _space; } }

        public RenderableVerticalList(int space, Vector2 position) : base(position)
        {
            _space = space;
            _position = position;
            _hPosition = null;
        }

        public RenderableVerticalList(int space, int x, int y) : this(space, new Vector2(x, y)) { }
        public RenderableVerticalList(int space, int x, int y, EHPosition horizontal_position) : this(space, new Vector2(x, y))
        {
            _hPosition = horizontal_position;
        }

        public void AddElement(IGameObject renderable)
        {
            IGameObject element = renderable;

            int maxWidth = Math.Max(SourceRectangle.Width, element.CollisionRectangle.Width);

            float x = Position.X;
            if (_hPosition != null)
            {
                switch ((EHPosition)_hPosition)
                {
                    case EHPosition.Right:
                        x += maxWidth - element.CollisionRectangle.Width; break;
                    
                    case EHPosition.Center:
                        x += (maxWidth - element.CollisionRectangle.Width) / 2.0f; break;
                }
            }

            element.Position = new Vector2(x, Position.Y + _length);

            _elements.Add(element);
            _length += element.CollisionRectangle.Height;
            this.SourceRectangle = new Rectangle(0, 0, maxWidth, _length);
            this.CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(SourceRectangle.Width, SourceRectangle.Height));
            _length += _space;
        }
    }
}
