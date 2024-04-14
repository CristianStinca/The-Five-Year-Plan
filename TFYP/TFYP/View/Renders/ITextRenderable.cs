using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.Renders
{
    internal interface ITextRenderable : IRenderable
    {
        SpriteFont Font { get; set; }
        string TextString { get; set; }
        Color Color { get; set; }
    }
}
