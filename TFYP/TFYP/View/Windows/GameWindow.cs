using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;

namespace TFYP.View.Windows
{
    internal sealed class GameWindow : Window
    {
        public static readonly int TILE_W = 30;

        public static readonly int TILE_H = 20;

        public static readonly int SCALE = 10;

        private Sprite[,] map;
        private bool mapIsInit;

        private Vector2 focusCoord;
        private Vector2 initPos;


        List<IRenderable> mapRend = new List<IRenderable>();

        public GameWindow() : base()
        {
            mapIsInit = false;
            focusCoord = new Vector2(0, 0);
            mapRend = new();
            initPos = new Vector2(-TILE_W / 2, -TILE_H / 2);

            //Sprite sprite_back = new Sprite(Globals.Content.Load<Texture2D>("back2"));
            //SpritesInWindow.Add(sprite_back);
        }

        public void SendGameMap(ViewObject[,] _map)
        {
            map = new Sprite[_map.GetLength(0), _map.GetLength(1)];
            this.mapRend.Clear();

            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    var _vo = _map[i, j];
                    float deviation = (i % 2 == 1) ? (TILE_W * SCALE / 2f) : 0f;


                    Sprite sprite = new Sprite(
                        Globals.Content.Load<Texture2D>(_vo.name),
                        new Microsoft.Xna.Framework.Vector2(
                            initPos.X * SCALE + focusCoord.X + deviation + _vo.x + (j * TILE_W * SCALE),
                            initPos.Y * SCALE + focusCoord.Y + _vo.y + (i * TILE_H * SCALE / 2f)
                        ),
                        SCALE
                    );

                    map[i, j] = sprite;

                    mapRend.Add(sprite);
                }
            }

            this.mapIsInit = true;
        }

        public void SetFocusCoord(Vector2 vec)
        {
            focusCoord = vec;
        }

        public override void Update()
        {
            base.Update();

            //if (!mapIsInit) return;

            //for (int i = 0; i < map.GetLength(0); i++)
            //    for (int j = 0; j < map.GetLength(1); j++) ;
                    //this.SpritesInWindow.Add(map[i, j]);
        }

        public override void Draw(IRenderer renderer)
        {
            if (mapIsInit)
            {
                renderer.DrawState(this.mapRend);
            }

            base.Draw(renderer);
        }

        private static readonly Lazy<GameWindow> lazy = new Lazy<GameWindow>(() => new GameWindow());
        public static GameWindow Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }
}
