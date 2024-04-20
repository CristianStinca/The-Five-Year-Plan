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
    internal class RenderableList : BaseRenderableObjectCollection
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

                //foreach (IGameObject element in _elements)
                //{
                //    element.Position = new Vector2(Position.X, Position.Y + _length);
                //    element.CollisionRectangle = new Rectangle(element.Position.ToPoint(), new Point(element.SourceRectangle.Width, element.SourceRectangle.Height));
                //    _length += element.SourceRectangle.Height + _space;
                //}
            }
        }
        public int Length { get { return _length; } }
        public int Space { get { return _space; } }

        public RenderableList(int space, Vector2 position) : base(position)
        {
            _space = space;
            _position = position;
            _hPosition = null;
        }

        public RenderableList(int space, int x, int y) : this(space, new Vector2(x, y)) { }
        public RenderableList(int space, int x, int y, EHPosition horizontal_position) : this(space, new Vector2(x, y))
        {
            _hPosition = horizontal_position;
        }

        public void AddElement(IGameObject renderable)
        {
            IGameObject element = renderable;

            int maxWidth = Math.Max(SourceRectangle.Width, element.SourceRectangle.Width);

            float x = Position.X;
            if (_hPosition != null)
            {
                switch ((EHPosition)_hPosition)
                {
                    case EHPosition.Right:
                        x += maxWidth - element.SourceRectangle.Width; break;
                    
                    case EHPosition.Center:
                        x += (maxWidth - element.SourceRectangle.Width) / 2.0f; break;
                }
            }

            element.Position = new Vector2(x, Position.Y + _length);
            element.CollisionRectangle = new Rectangle(element.Position.ToPoint(), new Point (element.SourceRectangle.Width, element.SourceRectangle.Height));
            _elements.Add(element);
            _length += element.SourceRectangle.Height;
            this.SourceRectangle = new Rectangle(0, 0, maxWidth, _length);
            this.CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(SourceRectangle.Width, SourceRectangle.Height));
            _length += _space;
        }

        //public void AddElementAt(int index, IRenderable renderable)
        //{
        //    List<IGameObject> rest = _elements.GetRange(index, _elements.Count - 1 - index);
        //    rest.Add(renderable);
        //    IGameObject lastElement = _elements.ElementAt(index - 1);
        //    _length = (int)Math.Round(lastElement.Position.Y) + lastElement.CollisionRectangle.Height;
        //    _elements.RemoveRange(index, _elements.Count - 1 - index);

        //    foreach (IGameObject element in rest)
        //    {
        //        AddElement(element);
        //    }
        //}
    }
}
