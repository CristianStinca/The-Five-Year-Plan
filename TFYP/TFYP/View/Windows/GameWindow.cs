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
using Microsoft.Xna.Framework.Input;
using TFYP.Model;

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
        public Rectangle ScreenLimit;
        private List<Rectangle> _UIElementsContainers;
        private List<Button> _UIElements;

        List<IRenderable> mapRend = new List<IRenderable>();

        private bool is2D = false;

        #region UIButtons

        Button resZone;
        Button indZone;
        Button commZone;
        //Button delZone;
        Button buildRoad;
        Button delRoad;
        Button police;
        Button stadium;
        Button school;

        Button stopTime;
        Button timeX1;
        Button timeX2;
        Button timeX3;

        Button perspectiveButton;

        public delegate void UIButtonPressedHandler();

        public event UIButtonPressedHandler UIResidentialZoneButtonPressed;
        public event UIButtonPressedHandler UIIndustrialZoneButtonPressed;
        public event UIButtonPressedHandler UICommertialZoneButtonPressed;
        //public event UIButtonPressedHandler UIDeleteZoneButtonPressed;
        public event UIButtonPressedHandler UIBuildRoadButtonPressed;
        //public event UIButtonPressedHandler UIDeleteRoadButtonPressed;
        public event UIButtonPressedHandler UIDeleteButtonPressed;
        public event UIButtonPressedHandler UIPoliceButtonPressed;
        public event UIButtonPressedHandler UIStadiumButtonPressed;
        public event UIButtonPressedHandler UISchoolButtonPressed;

        public event UIButtonPressedHandler UIStopSpeedPressed;
        public event UIButtonPressedHandler UISpeedX1Pressed;
        public event UIButtonPressedHandler UISpeedX2Pressed;
        public event UIButtonPressedHandler UISpeedX3Pressed;

        public event UIButtonPressedHandler UIChangePerspectivePressed;

        #endregion

        public GameWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            mapIsInit = false;
            focusCoord = new Vector2(0, 0);
            mapRend = new();
            initPos = new Vector2(-TILE_W / 2, -TILE_H / 2);
            map = null;
            screenRect = new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
            _UIElementsContainers = new List<Rectangle>();
            _UIElements = new List<Button>();
            ScreenLimit = new Rectangle(0, 0,
                (int)Math.Round(((GameModel.MAP_W - 0.5f) * GameWindow.TILE_W * GameWindow.SCALE) - Globals.Graphics.PreferredBackBufferWidth),
                (int)Math.Round(((GameModel.MAP_H / 2) - 0.5f) * GameWindow.TILE_H * GameWindow.SCALE - Globals.Graphics.PreferredBackBufferHeight)
            );

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
                        new Vector2(
                            (initPos.X * SCALE) + focusCoord.X + deviation + _vo.Position.X + ScreenLimit.X + (j * TILE_W * SCALE),
                            (initPos.Y * SCALE) + focusCoord.Y + _vo.Position.Y + ScreenLimit.Y + (i * TILE_H * SCALE / 2) - ((_vo.Texture.Height - TILE_H) * (SCALE - 1))
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
            return _UIElementsContainers.Any(element => element.Contains(x, y));
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

            Sprite BackgroundButtonTab = new (Globals.Content.Load<Texture2D>("Buttons/ButtonsHolder"));
            _UIElementsContainers.Add(BackgroundButtonTab.CollisionRectangle);
            ElementsInWindow.Add(BackgroundButtonTab);

            ScreenLimit.X = BackgroundButtonTab.Texture.Width;
            ScreenLimit.Width += BackgroundButtonTab.Texture.Width;


            RenderableList buttonsBar = new (30, 20, 30);

            delRoad = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Demolish_Road_Button")));
            delRoad.ButtonPressed += (string name) => UIDeleteButtonPressed.Invoke();
            buttonsBar.AddElement(delRoad);

            RenderableList zonesList = new RenderableList(10, 20, 30);
            zonesList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Zones", new Vector2(20, 30), Color.Black));

            resZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Residential_Zone_Button")));
            resZone.ButtonPressed += (string name) => UIResidentialZoneButtonPressed.Invoke();
            zonesList.AddElement(resZone);
            indZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Industrial_Zone_Button")));
            indZone.ButtonPressed += (string name) => UIIndustrialZoneButtonPressed.Invoke();
            zonesList.AddElement(indZone);
            commZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Commertial_Zone_Button")));
            commZone.ButtonPressed += (string name) => UICommertialZoneButtonPressed.Invoke();
            zonesList.AddElement(commZone);
            //delZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Erase_Zone_Button")));
            //delZone.ButtonPressed += UIButtonPressed;
            //zonesList.AddElement(delZone);

            buttonsBar.AddElement(zonesList);

            RenderableList roadList = new RenderableList(10, 20, 30);
            roadList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Roads", new Vector2(20, 30), Color.Black));
            buildRoad = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Build_Road_Button")));
            buildRoad.ButtonPressed += (string name) => UIBuildRoadButtonPressed.Invoke();
            roadList.AddElement(buildRoad);

            buttonsBar.AddElement(roadList);

            RenderableList specialsList = new RenderableList(10, 20, 30);
            specialsList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Specials", new Vector2(20, 30), Color.Black));
            police = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Police_Button")));
            police.ButtonPressed += (string name) => UIPoliceButtonPressed.Invoke();
            specialsList.AddElement(police);
            stadium = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Stadium_Button")));
            stadium.ButtonPressed += (string name) => UIStadiumButtonPressed.Invoke();
            specialsList.AddElement(stadium);
            school = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/School_Button")));
            school.ButtonPressed += (string name) => UISchoolButtonPressed.Invoke();
            specialsList.AddElement(school);

            buttonsBar.AddElement(specialsList);

            ElementsInWindow.AddRange(buttonsBar.ToIRenderable());

            Texture2D buttonBackTexture = Globals.Content.Load<Texture2D>("Buttons/speed_buttons_container");
            Sprite buttonBack = new Sprite(buttonBackTexture, new Vector2(Globals.Graphics.PreferredBackBufferWidth - buttonBackTexture.Width - 20, 30));
            _UIElementsContainers.Add(buttonBack.CollisionRectangle);
            ElementsInWindow.Add(buttonBack);

            stopTime = AddButton (new Sprite(Globals.Content.Load<Texture2D>("Buttons/stop_button"), new Vector2(buttonBack.Position.X + 10, buttonBack.Position.Y + 7)));
            stopTime.ButtonPressed += (string name) => UIStopSpeedPressed.Invoke();
            ElementsInWindow.Add(stopTime.ToIRenderable());

            timeX1 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/speed_1"), new Vector2(stopTime.Position.X + stopTime.SourceRectangle.Width + 9, buttonBack.Position.Y + 7)));
            timeX1.ButtonPressed += (string name) => UISpeedX1Pressed.Invoke();
            ElementsInWindow.Add(timeX1.ToIRenderable());

            timeX2 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/speed_2"), new Vector2(timeX1.Position.X + timeX1.SourceRectangle.Width + 7, buttonBack.Position.Y + 7)));
            timeX2.ButtonPressed += (string name) => UISpeedX2Pressed.Invoke();
            ElementsInWindow.Add(timeX2.ToIRenderable());

            timeX3 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/speed_3"), new Vector2(timeX2.Position.X + timeX2.SourceRectangle.Width + 7, buttonBack.Position.Y + 7)));
            timeX3.ButtonPressed += (string name) => UISpeedX3Pressed.Invoke();
            ElementsInWindow.Add(timeX3.ToIRenderable());

            Texture2D perspectiveButtonTexture = Globals.Content.Load<Texture2D>("Buttons/Switch_Perspective_Button");
            perspectiveButton = AddButton(new Sprite(perspectiveButtonTexture, new Vector2(Globals.Graphics.PreferredBackBufferWidth - perspectiveButtonTexture.Width - 20, Globals.Graphics.PreferredBackBufferHeight - perspectiveButtonTexture.Height - 30)));
            _UIElementsContainers.Add(perspectiveButton.CollisionRectangle);
            perspectiveButton.ButtonPressed += (string name) => ChangePerspective();
            ElementsInWindow.Add(perspectiveButton.ToIRenderable());
        }

        private Button AddButton(Sprite sprite)
        {
            Button btn = new Button(sprite, _inputHandler);
            _UIElements.Add(btn);
            return btn;
        }

        public void ChangePerspective()
        {
            if (is2D)
            {
                Debug.WriteLine("Switched to 2.5D");
                // run all the map and switch the textures
            }
            else
            {
                Debug.WriteLine("Switched to 2D");
                // run all the map and switch the textures
            }

            is2D = !is2D;
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

            foreach (Button btn in _UIElements)
            {
                btn.Update();
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
