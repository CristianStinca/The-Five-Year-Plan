using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TFYP.View.Renders;

namespace TFYP.View.UIElements
{
    internal class RenderableListCollection
    {
        List<RenderableList> _elements;
        int _space = 0;
        Vector2 _position;
        private int _length = 0;

        public RenderableListCollection(int space, Vector2 position)
        {
            _elements = new();
            _space = space;
            _position = position;
        }

        public RenderableListCollection(int space, int x, int y) : this(space, new Vector2(x, y)) { }

        public void AddElement(RenderableList renderable)
        {
            //RenderableList element = (RenderableList)renderable.Clone();
            RenderableList element = new RenderableList(renderable.Space, new Vector2(_position.X, _position.Y + _length));

            foreach (IRenderable rend in renderable.GetToDraw())
            {
                element.AddElement(rend);
            }

            _elements.Add(element);
            _length += element.Length + _space;
            Debug.WriteLine($"Length after: {_length}");
        }

        public void AddElementAt(int index, RenderableList renderable)
        {
            List<RenderableList> rest = _elements.GetRange(index, _elements.Count - 1 - index);
            rest.Add(renderable);
            RenderableList lastElement = _elements.ElementAt(index - 1);
            _length = (int)Math.Round(lastElement.Position.Y) + lastElement.Length;
            _elements.RemoveRange(index, _elements.Count - 1 - index);

            foreach (RenderableList element in rest)
            {
                AddElement(element);
            }
        }

        public List<IRenderable> GetToDraw()
        {
            List<IRenderable> list = new ();

            foreach (RenderableList element in _elements)
            {
                foreach (IRenderable renderable in element.GetToDraw())
                {
                    list.Add(renderable);
                }
            }

            return list;
        }
    }
}
