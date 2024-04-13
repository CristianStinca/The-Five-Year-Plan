using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using TFYP.Utils;

namespace TFYP.View.Renders
{
    internal class MonoGameRenderer : IRenderer
    {
        //public void DrawIRenderable(List<ISprite> spritesToDraw)
        //{
        //    Globals.SpriteBatch.Begin(
        //        sortMode: SpriteSortMode.Immediate,
        //        samplerState: SamplerState.PointClamp,
        //        blendState: BlendState.AlphaBlend
        //    );

        //    foreach (ISprite sprite in spritesToDraw)
        //    {
        //        Globals.SpriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRectangle,
        //            sprite.Tint, 0.0f, Vector2.Zero, sprite.Scale, SpriteEffects.None, 0);
        //    }

        //    Globals.SpriteBatch.End();
        //}

        //public void DrawITextRenderable(List<ITextRenderable> textToDraw)
        //{
        //    Globals.SpriteBatch.Begin(
        //        sortMode: SpriteSortMode.Immediate,
        //        samplerState: SamplerState.PointClamp,
        //        blendState: BlendState.AlphaBlend
        //    );

        //    foreach (ITextRenderable text in textToDraw)
        //    {
        //        Globals.SpriteBatch.DrawString(text.Font, text.TextString, text.Position, text.Color);
        //    }

        //    Globals.SpriteBatch.End();
        //}

        /// <summary>
        /// Render to the screen the given IRenderables.
        /// </summary>
        /// <param name="elementsToDraw">The IRenderable elements to render.</param>
        public void DrawState(List<IRenderable> elementsToDraw)
        {
            Globals.SpriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                samplerState: SamplerState.PointClamp,
                blendState: BlendState.AlphaBlend
            );

            foreach (IRenderable element in elementsToDraw)
            {
                if (element is ISprite)
                {
                    ISprite sprite = (ISprite)element;
                    Globals.SpriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRectangle,
                        sprite.Tint, 0.0f, Vector2.Zero, sprite.Scale, SpriteEffects.None, 0);
                }

                if (element is ITextRenderable)
                {
                    ITextRenderable text = (ITextRenderable)element;
                    Globals.SpriteBatch.DrawString(text.Font, text.TextString, text.Position, text.Color);
                }
            }

            Globals.SpriteBatch.End();
        }
    }
}
