using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.Renders
{
    public interface IRenderable
    {
        Vector2 Position { get; set; }
        Texture2D Texture { get; set; }
        Rectangle SourceRectangle { get; set; }
        Color Tint { get; set; }
        float Scale { get; set; }
    }
}
