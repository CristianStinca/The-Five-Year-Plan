using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.Renders
{
    internal interface IGameObject
    {
        Vector2 Position { get; set; }
        Rectangle SourceRectangle { get; set; }
        Rectangle CollisionRectangle { get; set; }
    }
}
