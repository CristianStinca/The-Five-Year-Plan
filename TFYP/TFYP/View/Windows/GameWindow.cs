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
        private Sprite[,] map;
        private bool mapIsInit;
        private Vector2 focusCoord;
        private Vector2 tileSize;
        private bool isTileSizeInit;
        List<IRenderable> mapRend = new List<IRenderable>();

        public GameWindow() : base()
        {
            mapIsInit = false;
            focusCoord = new Vector2(0, 0);
            tileSize = new Vector2(0, 0);
            isTileSizeInit = false;
            mapRend = new();

            //Sprite sprite_back = new Sprite(Globals.Content.Load<Texture2D>("back2"));
            //SpritesInWindow.Add(sprite_back);
        }

        public void SendGameMap(ViewObject[,] _map)
        {
            map = new Sprite[_map.GetLength(0), _map.GetLength(1)];
            this.mapRend.Clear();

            //if (!isTileSizeInit)
            //{
            //    Texture2D texture = Globals.Content.Load<Texture2D>(_map[0,0].name);
            //    tileSize.X = texture.Width;
            //    tileSize.Y = texture.Height;
            //    texture.Dispose();
            //    isTileSizeInit = true;
            //}

            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    var _vo = _map[i, j];
                    Texture2D texture = Globals.Content.Load<Texture2D>(_vo.name);
                    float deviation = (i % 2 == 1) ? (texture.Width * _vo.scale / 2f) : 0f;
                    Sprite sprite = new Sprite(
                        Globals.Content.Load<Texture2D>(_vo.name),
                        new Microsoft.Xna.Framework.Vector2(
                            deviation + _vo.x + (j * texture.Width * _vo.scale),
                            _vo.y + (i * texture.Height * _vo.scale / 2f)
                        ),
                        //new Microsoft.Xna.Framework.Vector2(_vo.x, _vo.y),
                        _vo.scale
                    );

                    map[i, j] = sprite;
                    //renderables.Add(sprite);
                    mapRend.Add(sprite);
                }
            }

            this.mapIsInit = true;

            //this.SpritesInWindow.Add(map[0, 0]);
        }

        public void SetFocusCoord(Vector2 vec)
        {
            //focusCoord = vec;
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
