using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;

namespace TFYP.View.Windows
{
    internal sealed class GameWindow : Window
    {
        public static readonly int TILE_W = 30;

        public static readonly int TILE_H = 20;

        public static readonly int SCALE = 5;

        //private Sprite[,] map;
        private bool mapIsInit;

        private Vector2 focusCoord;
        private Vector2 initPos;

        List<IRenderable> mapRend = new List<IRenderable>();

        public GameWindow(IUIElements UIElements) : base(UIElements)
        {
            mapIsInit = false;
            focusCoord = new Vector2(0, 0);
            mapRend = new();
            initPos = new Vector2(-TILE_W / 2, -TILE_H / 2);

            //Sprite sprite_back = new Sprite(Globals.Content.Load<Texture2D>("back2"));
            //SpritesInWindow.Add(sprite_back);
        }

        public void SendGameMap(IRenderable[,] _map)
        {
            //map = new Sprite[_map.GetLength(0), _map.GetLength(1)];
            this.mapRend.Clear();

            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    IRenderable _vo = _map[i, j];
                    float deviation = (i % 2 == 1) ? (TILE_W * SCALE / 2f) : 0f;

                    Sprite sprite = new Sprite(
                        _vo.Texture,
                        new Microsoft.Xna.Framework.Vector2(
                            initPos.X * SCALE + focusCoord.X + deviation + _vo.Position.X + (j * TILE_W * SCALE),
                            initPos.Y * SCALE + focusCoord.Y + _vo.Position.Y + (i * TILE_H * SCALE / 2f)
                        ),
                        SCALE
                    );

                    //map[i, j] = sprite;

                    mapRend.Add(sprite);
                }
            }

            this.mapIsInit = true;
        }

        public void SetFocusCoord(Vector2 vec)
        {
            focusCoord = vec;
        }

        public override void Draw(IRenderer renderer)
        {
            if (mapIsInit)
            {
                renderer.DrawState(this.mapRend);
            }

            base.Draw(renderer);
        }
    }
}
