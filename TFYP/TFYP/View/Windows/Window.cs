using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;

namespace TFYP.View.Windows
{
    internal abstract class Window
    {
        protected List<IRenderable> SpritesInWindow { get; set; } // list of all the sprites that need to be rendered on the scnene

        protected Window() 
        {
            this.SpritesInWindow = new();
        }

        public virtual void Update()
        {
            // base for updating the ui while the window is active
        }

        public virtual void Draw(IRenderer renderer)
        {
            renderer.DrawState(this.SpritesInWindow);
        }

        // method to pass all the ViewObjects that need to be rendered on the scnene, from the Controller to the View
        public virtual void SendViewObject(ViewObject _vo)
        {
            Sprite sprite = new Sprite(
                Globals.Content.Load<Texture2D>(_vo.name),
                new Microsoft.Xna.Framework.Vector2(_vo.x, _vo.y),
                _vo.scale
            );

            this.SpritesInWindow.Add(sprite);
        }

        public virtual void SendViewObjects(IEnumerable<ViewObject> _voe)
        {
            foreach (ViewObject _vo in _voe)
            {
                Sprite sprite = new Sprite(
                    Globals.Content.Load<Texture2D>(_vo.name),
                    new Microsoft.Xna.Framework.Vector2(_vo.x, _vo.y),
                    _vo.scale
                );

                this.SpritesInWindow.Add(sprite);
            }
        }
    }
}
