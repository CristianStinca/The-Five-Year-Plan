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
using TFYP.View.UIElements.ClickableElements;
using static TFYP.View.UIElements.ClickableElements.Button;

namespace TFYP.View.Windows
{
    internal sealed class GameWindow : Window
    {
        public delegate void TilePressedInWindowHandler(int i, int j, string btn);
        public event TilePressedInWindowHandler TileButtonPressedInWindow;

        public static readonly int TILE_W = 30;

        public static readonly int TILE_H = 20;

        public static readonly int SCALE = 5;

        private TileButton[,] map;
        private bool mapIsInit;

        private Vector2 focusCoord;
        private Vector2 initPos;

        private Rectangle screenRect;
        private List<Rectangle> _UIElements;

        List<IRenderable> mapRend = new List<IRenderable>();

        public GameWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            mapIsInit = false;
            focusCoord = new Vector2(0, 0);
            mapRend = new();
            initPos = new Vector2(-TILE_W / 2, -TILE_H / 2);
            map = null;
            screenRect = new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
            _UIElements = new List<Rectangle>();

            InitialiseUi();
        }

        /// <summary>
        /// Method to update the matrix of IRenderable
        /// </summary>
        /// <param name="_map">The IRenderable matrix.</param>
        public void SendGameMap(ISprite[,] _map)
        {
            if (map == null)
            {
                map = new TileButton[_map.GetLength(0), _map.GetLength(1)];
            }

            this.mapRend.Clear();

            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    ISprite _vo = _map[i, j];
                    float deviation = (i % 2 == 1) ? (TILE_W * SCALE / 2f) : 0f;

                    Sprite sprite = new Sprite(
                        _vo.Texture,
                        new Microsoft.Xna.Framework.Vector2(
                            (initPos.X * SCALE) + focusCoord.X + deviation + _vo.Position.X + (j * TILE_W * SCALE),
                            (initPos.Y * SCALE) + focusCoord.Y + _vo.Position.Y + (i * TILE_H * SCALE / 2) - ((_vo.Texture.Height - TILE_H) * (SCALE - 1))
                        ),
                        SCALE
                    );

                    TileButton tile = new TileButton(sprite, _inputHandler, j, i);
                    tile.TileButtonPressed += OnTilePressed;

                    map[i, j] = tile;

                    mapRend.Add(sprite);
                }
            }

            this.mapIsInit = true;
        }

        public void OnTilePressed(int col, int row, int x, int y, string btn)
        {
            if (!screenRect.Contains(x, y) || IsOnUIElement(x, y))
            {
                return;
            }

            TileButtonPressedInWindow.Invoke(col, row, btn); // TODO: Refactor the events so that the controller gets it more directly? (now we create an event for every tile and that's sad :( )
        }

        private bool IsOnUIElement(int x, int y)
        {
            return _UIElements.Any(element => element.Contains(x, y));
        }

        public void SetFocusCoord(Vector2 vec)
        {
            focusCoord = vec;
        }

        private void InitialiseUi()
        {
            //Sprite ResidentialZone = new Sprite(Globals.Content.Load<Texture2D>("Residential_Zone_Button"), new Vector2(20, 30));
            //ResidentialZone.SourceRectangle = new Rectangle(0, 0, ResidentialZone.SourceRectangle.Width, (int)Math.Round(ZonesText.Font.MeasureString(ZonesText.TextString).Y));
            //ElementsInWindow.Add(ResidentialZone);

            Sprite BackgroundButtonTab = new (Globals.Content.Load<Texture2D>("ButtonsHolder"));
            _UIElements.Add(new Rectangle(0, 0, BackgroundButtonTab.Texture.Width, BackgroundButtonTab.Texture.Height));
            ElementsInWindow.Add(BackgroundButtonTab);

            RenderableListCollection buttonsBar = new (10, 20, 30);

            RenderableList zonesList = new RenderableList(10, 20, 30);
            zonesList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Zones", new Vector2(20, 30), Color.Black));
            zonesList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Residential_Zone_Button")));
            zonesList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Industrial_Zone_Button")));
            zonesList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Commertial_Zone_Button")));
            zonesList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Erase_Zone_Button")));

            buttonsBar.AddElement(zonesList);

            RenderableList roadList = new RenderableList(10, 20, 30);
            roadList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Roads", new Vector2(20, 30), Color.Black));
            roadList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Build_Road_Button")));
            roadList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Demolish_Road_Button")));

            buttonsBar.AddElement(roadList);

            RenderableList specialsList = new RenderableList(10, 20, 30);
            specialsList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Specials", new Vector2(20, 30), Color.Black));
            specialsList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Police_Button")));
            specialsList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Stadium_Button")));
            specialsList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("School_Button")));

            buttonsBar.AddElement(specialsList);

            ElementsInWindow.AddRange(buttonsBar.GetToDraw());
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].Update();
                }
            }
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
