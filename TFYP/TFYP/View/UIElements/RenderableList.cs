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
    internal class RenderableList : IRenderableObjectCollection
    {
        List<IGameObject> _elements;
        protected int _space = 0;
        protected int _length = 0;
        protected Vector2 _position = Vector2.Zero;

        public Vector2 Position { 
            get
            {
                return _position;
            } 
            set
            {
                _position = value;
                _length = 0;
                this.CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(SourceRectangle.Width, SourceRectangle.Height));
                foreach (IGameObject element in _elements)
                {
                    element.Position = new Vector2(Position.X, Position.Y + _length);
                    element.CollisionRectangle = new Rectangle(element.Position.ToPoint(), new Point(element.SourceRectangle.Width, element.SourceRectangle.Height));
                    _length += element.SourceRectangle.Height + _space;
                }
            }
        }
        public int Length { get { return _length; } }
        public int Space { get { return _space; } }

        public Rectangle SourceRectangle { get; set; }
        public Rectangle CollisionRectangle { get; set; }

        public RenderableList(int space, Vector2 position)
        {
            _elements = new ();
            _space = space;
            _position = position;
        }

        public RenderableList(int space, int x, int y) : this(space, new Vector2(x, y)) { }

        public void AddElement(IGameObject renderable)
        {
            IGameObject element = renderable;

            element.Position = new Vector2(Position.X, Position.Y + _length);
            element.CollisionRectangle = new Rectangle(element.Position.ToPoint(), new Point (element.SourceRectangle.Width, element.SourceRectangle.Height));
            _elements.Add(element);
            _length += element.SourceRectangle.Height;
            this.SourceRectangle = new Rectangle(0, 0, Math.Max(SourceRectangle.Width, element.SourceRectangle.Width), _length);
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

        public virtual List<IRenderable> ToIRenderable()
        {
            List<IRenderable> list = new List<IRenderable>();
            foreach (IGameObject element in _elements)
            {
                if (element is IRenderable)
                {
                    list.Add((IRenderable)element);
                }
                else if (element is IRenderableObjectCollection)
                {
                    list.AddRange(((IRenderableObjectCollection)element).ToIRenderable());
                }
                else if (element is IGameObject)
                {
                    list.Add(((IRenderableObject)element).ToIRenderable());
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            
            return list;
        }
    }
}
