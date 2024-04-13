using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.View.Renders;

namespace TFYP.View.UIElements
{
    internal class RenderableList : ICloneable
    {
        List<IRenderable> _elements;
        int _space = 0;
        int _length = 0;

        public Vector2 Position { get; set; }
        public int Length { get { return _length; } }
        public int Space { get { return _space; } }

        public RenderableList(int space, Vector2 position)
        {
            _elements = new ();
            _space = space;
            Position = position;
        }

        public RenderableList(int space, int x, int y) : this(space, new Vector2(x, y)) { }

        public void AddElement(IRenderable renderable)
        {
            IRenderable element = (IRenderable)renderable.Clone();

            element.Position = new Vector2(Position.X, Position.Y + _length);
            element.CollisionRectangle = new Rectangle(element.Position.ToPoint(), new Point (element.SourceRectangle.Width, element.SourceRectangle.Height));
            _elements.Add(element);
            _length += element.SourceRectangle.Height + _space;
        }

        public void AddElementAt(int index, IRenderable renderable)
        {
            List<IRenderable> rest = _elements.GetRange(index, _elements.Count - 1 - index);
            rest.Add(renderable);
            IRenderable lastElement = _elements.ElementAt(index - 1);
            _length = (int)Math.Round(lastElement.Position.Y) + lastElement.CollisionRectangle.Height;
            _elements.RemoveRange(index, _elements.Count - 1 - index);

            foreach (IRenderable element in rest)
            {
                AddElement(element);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<IRenderable> GetToDraw()
        {
            return _elements;
        }
    }
}
