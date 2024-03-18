using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TFYP.Utils;

namespace TFYP.View.Renders
{
    public class MonoGameRenderer : IRenderer
    {
        public void DrawState(List<IRenderable> spritesToDraw)
        {
            Globals.SpriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                samplerState: SamplerState.PointClamp,
                blendState: BlendState.AlphaBlend
            );

            foreach (IRenderable sprite in spritesToDraw)
            {
                Globals.SpriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRectangle,
                    sprite.Tint, 0.0f, Vector2.Zero, sprite.Scale, SpriteEffects.None, 0);
            }

            Globals.SpriteBatch.End();
        }
    }
}
