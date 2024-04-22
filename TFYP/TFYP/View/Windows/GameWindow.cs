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
        private Rectangle gameScreenRect;
        public Rectangle ScreenLimit;
        private List<Rectangle> _UIElementsContainers;
        //private List<Button> _UpdateingUIElements;

        List<IRenderable> mapRend = new List<IRenderable>();
        List<IRenderable> screenWindow = new List<IRenderable>();
        List<IRenderable> menuWindow = new List<IRenderable>();

        bool is_menu_active = false;

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

        Button s_newgame;
        Button s_save;
        Button s_load;
        Button s_setting;
        Button s_exit;

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

        public event UIButtonPressedHandler UIMenuNewGameButtonPressed;
        public event UIButtonPressedHandler UIMenuSaveGameButtonPressed;
        public event UIButtonPressedHandler UIMenuLoadGameButtonPressed;
        public event UIButtonPressedHandler UIMenuOpenSettingsButtonPressed;
        public event UIButtonPressedHandler UIMenuExitButtonPressed;

        #endregion

        public GameWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            mapIsInit = false;
            focusCoord = new Vector2(0, 0);
            mapRend = new();
            initPos = new Vector2(-TILE_W / 2, -TILE_H / 2);
            map = null;
            screenRect = new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
            gameScreenRect = new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
            _UIElementsContainers = new List<Rectangle>();
            //_UpdateingUIElements = new List<Button>();
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
            if (!screenRect.Contains(x, y) || IsOnUIElement(x, y) || is_menu_active)
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

            gameScreenRect.X = BackgroundButtonTab.Texture.Width;
            gameScreenRect.Width -= BackgroundButtonTab.Texture.Width;

            RenderableList buttonsBar = new (30, 20, 30);

            delRoad = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Demolish_Road_Button")));
            delRoad.ButtonPressed += (string name) => NotifyEvent(UIDeleteButtonPressed);
            buttonsBar.AddElement(delRoad);

            RenderableList zonesList = new RenderableList(10, 20, 30);
            zonesList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Zones", new Vector2(20, 30), Color.Black));

            resZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Residential_Zone_Button")));
            resZone.ButtonPressed += (string name) => NotifyEvent(UIResidentialZoneButtonPressed);
            zonesList.AddElement(resZone);
            indZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Industrial_Zone_Button")));
            indZone.ButtonPressed += (string name) => NotifyEvent(UIIndustrialZoneButtonPressed);
            zonesList.AddElement(indZone);
            commZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Commertial_Zone_Button")));
            commZone.ButtonPressed += (string name) => NotifyEvent(UICommertialZoneButtonPressed);
            zonesList.AddElement(commZone);
            //delZone = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Erase_Zone_Button")));
            //delZone.ButtonPressed += UIButtonPressed;
            //zonesList.AddElement(delZone);

            buttonsBar.AddElement(zonesList);

            RenderableList roadList = new RenderableList(10, 20, 30);
            roadList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Roads", new Vector2(20, 30), Color.Black));
            buildRoad = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Build_Road_Button")));
            buildRoad.ButtonPressed += (string name) => NotifyEvent(UIBuildRoadButtonPressed);
            roadList.AddElement(buildRoad);

            buttonsBar.AddElement(roadList);

            RenderableList specialsList = new RenderableList(10, 20, 30);
            specialsList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Specials", new Vector2(20, 30), Color.Black));
            police = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Police_Button")));
            police.ButtonPressed += (string name) => NotifyEvent(UIPoliceButtonPressed);
            specialsList.AddElement(police);
            stadium = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Stadium_Button")));
            stadium.ButtonPressed += (string name) => NotifyEvent(UIStadiumButtonPressed);
            specialsList.AddElement(stadium);
            school = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/School_Button")));
            school.ButtonPressed += (string name) => NotifyEvent(UISchoolButtonPressed);
            specialsList.AddElement(school);

            buttonsBar.AddElement(specialsList);

            ElementsInWindow.AddRange(buttonsBar.ToIRenderable());

            Texture2D buttonBackTexture = Globals.Content.Load<Texture2D>("Buttons/speed_buttons_container");
            Sprite buttonBack = new Sprite(buttonBackTexture, new Vector2(Globals.Graphics.PreferredBackBufferWidth - buttonBackTexture.Width - 20, 30));
            _UIElementsContainers.Add(buttonBack.CollisionRectangle);
            ElementsInWindow.Add(buttonBack);

            stopTime = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/stop_button"), new Vector2(buttonBack.Position.X + 10, buttonBack.Position.Y + 7)));
            stopTime.ButtonPressed += (string name) => NotifyEvent(UIStopSpeedPressed);
            ElementsInWindow.Add(stopTime.ToIRenderable());

            timeX1 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/speed_1"), new Vector2(stopTime.Position.X + stopTime.SourceRectangle.Width + 9, buttonBack.Position.Y + 7)));
            timeX1.ButtonPressed += (string name) => NotifyEvent(UISpeedX1Pressed);
            ElementsInWindow.Add(timeX1.ToIRenderable());

            timeX2 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/speed_2"), new Vector2(timeX1.Position.X + timeX1.SourceRectangle.Width + 7, buttonBack.Position.Y + 7)));
            timeX2.ButtonPressed += (string name) => NotifyEvent(UISpeedX2Pressed);
            ElementsInWindow.Add(timeX2.ToIRenderable());

            timeX3 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/speed_3"), new Vector2(timeX2.Position.X + timeX2.SourceRectangle.Width + 7, buttonBack.Position.Y + 7)));
            timeX3.ButtonPressed += (string name) => NotifyEvent(UISpeedX3Pressed);
            ElementsInWindow.Add(timeX3.ToIRenderable());

            Texture2D perspectiveButtonTexture = Globals.Content.Load<Texture2D>("Buttons/Switch_Perspective_Button");
            perspectiveButton = AddButton(new Sprite(perspectiveButtonTexture, new Vector2(Globals.Graphics.PreferredBackBufferWidth - perspectiveButtonTexture.Width - 20, Globals.Graphics.PreferredBackBufferHeight - perspectiveButtonTexture.Height - 30)));
            _UIElementsContainers.Add(perspectiveButton.CollisionRectangle);
            perspectiveButton.ButtonPressed += (string name) => { if (!is_menu_active) ChangePerspective(); };
            ElementsInWindow.Add(perspectiveButton.ToIRenderable());
            
            RenderableContainer menuContainer = new(0, 0, ESize.AllScreen);
            Sprite menuBackground = new Sprite(Globals.Content.Load<Texture2D>("Menu/game_menu_back"));
            menuContainer.AddElement(EVPosition.Center, EHPosition.Center, menuBackground);

            RenderableList menuList = new(10, 0, 0);
            s_newgame = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_NewGame_Button")));
            menuList.AddElement(s_newgame);
            s_newgame.ButtonPressed += (string name) => NotifyMenuEvent(UIMenuNewGameButtonPressed);
            s_save = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_Save_Button")));
            menuList.AddElement(s_save);
            s_save.ButtonPressed += (string name) => NotifyMenuEvent(UIMenuSaveGameButtonPressed);
            s_load = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_Load_Button")));
            menuList.AddElement(s_load);
            s_load.ButtonPressed += (string name) => NotifyMenuEvent(UIMenuLoadGameButtonPressed);
            s_setting = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_Settings_Button")));
            menuList.AddElement(s_setting);
            s_setting.ButtonPressed += (string name) => NotifyMenuEvent(UIMenuOpenSettingsButtonPressed);
            s_exit = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_Exit_Button")));
            menuList.AddElement(s_exit);
            s_exit.ButtonPressed += (string name) => NotifyMenuEvent(UIMenuExitButtonPressed);

            menuContainer.AddElement(EVPosition.Center, EHPosition.Center, menuList);

            menuWindow.AddRange(menuContainer.ToIRenderable());
        }

        private void NotifyEvent(UIButtonPressedHandler eve)
        {
            if (!is_menu_active)
                eve.Invoke();
        }

        private void NotifyMenuEvent(UIButtonPressedHandler eve)
        {
            if (is_menu_active)
                eve.Invoke();
        }

        public void DrawMenu()
        {
            is_menu_active = true;
        }

        public void CleanMenu()
        {
            is_menu_active = false;
        }

        public enum EPrintInfo
        {
            Title,
            Normal,
            Sublist
        }

        public void PrintInfo(params Tuple<string, EPrintInfo>[] args)
        {
            RenderableContainer screenCont = new(gameScreenRect.X, gameScreenRect.Y, gameScreenRect.Size.ToVector2());
            screenCont.Margin = 20f;
            RenderableContainer cont = new(0, 0, ESize.FitContent, Color.LightGray);
            cont.Padding = 20f;
            RenderableList list = new(10, 0, 0);

            foreach (Tuple<string, EPrintInfo> item in args)
            {
                string text = item.Item1;
                EPrintInfo info = item.Item2;

                switch (info)
                {
                    case EPrintInfo.Title:
                        list.AddElement(new Text(Globals.Content.Load<SpriteFont>("UITitleText"), text, Vector2.Zero, Color.Black));
                        break;
                    case EPrintInfo.Normal:
                        list.AddElement(new Text(Globals.Content.Load<SpriteFont>("UISubtitleText"), text, Vector2.Zero, Color.Black));
                        break;
                    case EPrintInfo.Sublist:
                        list.AddElement(new Text(Globals.Content.Load<SpriteFont>("UINormalText"), text, Vector2.Zero, Color.Black));
                        break;
                }
            }

            cont.AddElement(EVPosition.Bottom, EHPosition.Left, list);
            screenCont.AddElement(EVPosition.Bottom, EHPosition.Left, cont);

            screenWindow.Clear();
            screenWindow.AddRange(screenCont.ToIRenderable());
        }

        public void DeleteInfo()
        {
            screenWindow.Clear();
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

            if (map != null)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        map[i, j].Update();
                    }
                }
            }
        }

        public override void Draw(IRenderer renderer)
        {
            if (mapIsInit)
            {
                renderer.DrawState(this.mapRend);
            }

            renderer.DrawState(screenWindow);

            if (is_menu_active)
                renderer.DrawState(menuWindow);

            base.Draw(renderer);
        }
    }
}
