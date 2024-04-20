using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace TFYP.View.Renders
{
    internal class BaseRenderableObjectCollection : IRenderableObjectCollection
    {
        protected List<IGameObject> _elements;

        protected Vector2 _position;
        public virtual Vector2 Position { get => _position; set => _position = value; }
        public virtual Rectangle SourceRectangle { get; set; }
        public virtual Rectangle CollisionRectangle { get; set; }

        public BaseRenderableObjectCollection(Vector2 position)
        {
            this._elements = new();
            this._position = position;
            this.CollisionRectangle = new Rectangle(Position.ToPoint(), Point.Zero);
            this.SourceRectangle = new Rectangle(0, 0, 0, 0);
        }

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
