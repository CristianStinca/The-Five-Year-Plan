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
using MonoGame.Extended.BitmapFonts;
using TFYP.Model.Disasters;

namespace TFYP.View.Windows
{
    internal sealed class GameWindow : Window
    {
        public delegate void TilePressedInWindowHandler(int i, int j, string btn);
        public event TilePressedInWindowHandler TileButtonPressedInWindow;
        public delegate void TileHoverInWindowHandler(int i, int j);
        public event TileHoverInWindowHandler TileButtonHoverInWindow;


        public static readonly int TILE_W = 30;

        public static readonly int TILE_H = 20;

        public static readonly int SCALE = 5;

        private TileButton[,] map;
        private bool mapIsInit;

        private List<IRenderable> disasters = new();

        private Vector2 focusCoord;
        private Vector2 initPos;

        private Rectangle screenRect;
        private Rectangle gameScreenRect;
        public Rectangle ScreenLimit;
        private List<Rectangle> _UIElementsContainers;
        //private List<Button> _UpdateingUIElements;
        private Rectangle statsRect = new();
        private Rectangle tileInfoRect = new();

        List<IRenderable> mapRend = new List<IRenderable>();
        List<IRenderable> screenWindow = new List<IRenderable>();
        List<IRenderable> statsWindow = new List<IRenderable>();
        List<IRenderable> menuWindow = new List<IRenderable>();
        List<IRenderable> gameOverWindow = new List<IRenderable>();

        public bool is_menu_active = false;
        public bool is_game_over = false;
        public bool is_tile_info_active = false;
        public bool is_tile_info_bttn_active = false;
        public bool is_budget_active = false;

        public bool is2D = false;

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
        Button university;

        Button budget;
        Button disaster;

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

        Button ge_newgame;
        Button ge_load;

        Button screenWindowBttn;
        Button statsWindowBttn;
        Button upgradeBttn;

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
        public event UIButtonPressedHandler UIUniverisyButtonPressed;

        public event UIButtonPressedHandler UIBudgetButtonPressed;
        public event UIButtonPressedHandler UIDisasterButtonPressed;

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

        public event UIButtonPressedHandler UIUpgradeTileButtonPressed;

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

            InitialiseMap();
            InitialiseUi();
        }

        private void InitialiseMap()
        {
            if (map == null)
            {
                map = new TileButton[GameModel.MAP_H, GameModel.MAP_W];
            }

            for (int i = 0; i < GameModel.MAP_H; i++)
            {
                for (int j = 0; j < GameModel.MAP_W; j++)
                {
                    float deviation = (i % 2 == 1) ? (TILE_W * SCALE / 2f) : 0f;
                    Sprite sprite = new Sprite(
                        Globals.Content.Load<Texture2D>("Tiles/innac"),
                        new Vector2(
                            deviation + (j * TILE_W * SCALE),
                            (i * TILE_H * SCALE / 2)
                        ),
                        SCALE
                    );

                    TileButton tile = new TileButton(sprite, _inputHandler, j, i);

                    tile.TileButtonPressed += OnTilePressed;
                    tile.TileButtonHover += OnTileHover;
                    map[i, j] = tile;
                }
            }

        }

        /// <summary>
        /// Method to update the matrix of IRenderable
        /// </summary>
        /// <param name="_map">The IRenderable matrix.</param>
        public void SendGameMap(ISprite[,] _map)
        {
            this.mapRend.Clear();
            

            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    Sprite _vo = _map[i, j] as Sprite;

                    float deviation = (i % 2 == 1) ? (TILE_W * SCALE / 2f) : 0f;
                    map[i, j].Position = new Vector2(
                        (initPos.X * SCALE) + focusCoord.X + deviation + ScreenLimit.X + (j * TILE_W * SCALE),
                        (initPos.Y * SCALE) + focusCoord.Y + ScreenLimit.Y + (i * TILE_H * SCALE / 2)
                    );

                    if (_vo == null)
                        continue;

                    if (_vo.Texture.Name == _UIElements.EmptyTile.Texture.Name)
                    {
                        Sprite new_sprite = _UIElements.getGrass(i, j);
                        _vo.Texture = new_sprite.Texture;
                    }

                    _vo.Position += new Vector2(
                        (initPos.X * SCALE) + focusCoord.X + ScreenLimit.X,
                        (initPos.Y * SCALE) + focusCoord.Y + ScreenLimit.Y
                    );

                    mapRend.Add(_vo);
                    // TODO: change the TileButton to accept no sprite and generate it only once
                }
            }

            this.mapIsInit = true;
        }

        public void SendDisasters(params Sprite[] disasters)
        {
            this.disasters.Clear();
            foreach (Sprite spr in disasters)
            {
                spr.Position += new Vector2(
                    (initPos.X * SCALE) + focusCoord.X + ScreenLimit.X,
                    (initPos.Y * SCALE) + focusCoord.Y + ScreenLimit.Y
                );

                this.disasters.Add(spr);
            }
        }

        public void OnTilePressed(int col, int row, int x, int y, string btn)
        {
            if (!screenRect.Contains(x, y) || IsOnUIElement(x, y) || is_menu_active || is_game_over || (tileInfoRect.Contains(x, y) && is_tile_info_active) || (statsRect.Contains(x, y) && is_budget_active))
            {
                return;
            }

            TileButtonPressedInWindow.Invoke(col, row, btn); // TODO: Refactor the events so that the controller gets it more directly? (now we create an event for every tile and that's sad :( )
        }

        public void OnTileHover(int col, int row, int x, int y)
        {
            if (!screenRect.Contains(x, y) || IsOnUIElement(x, y) || is_menu_active || is_game_over || (tileInfoRect.Contains(x, y) && is_tile_info_active) || (statsRect.Contains(x, y) && is_budget_active))
            {
                return;
            }

            TileButtonHoverInWindow.Invoke(col, row);
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

            RenderableVerticalList buttonsBar = new (15, 20, 30);

            delRoad = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Demolish_Road_Button")));
            delRoad.ButtonPressed += (string name) => NotifyEvent(UIDeleteButtonPressed);
            buttonsBar.AddElement(delRoad);

            RenderableVerticalList zonesList = new RenderableVerticalList(5, 20, 30);
            //zonesList.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/propaganda_22"), "Zones", new Vector2(20, 30), Color.Black));
            zonesList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Menu/zones")));

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

            RenderableVerticalList roadList = new RenderableVerticalList(5, 20, 30);
            //roadList.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/propaganda_22"), "Roads", new Vector2(20, 30), Color.Black));
            roadList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Menu/roads")));

            buildRoad = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Build_Road_Button")));
            buildRoad.ButtonPressed += (string name) => NotifyEvent(UIBuildRoadButtonPressed);
            roadList.AddElement(buildRoad);

            buttonsBar.AddElement(roadList);

            RenderableVerticalList specialsList = new RenderableVerticalList(5, 20, 30);
            //specialsList.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/propaganda_22"), "Specials", new Vector2(20, 30), Color.Black));
            specialsList.AddElement(new Sprite(Globals.Content.Load<Texture2D>("Menu/specials")));

            police = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Police_Button")));
            police.ButtonPressed += (string name) => NotifyEvent(UIPoliceButtonPressed);
            specialsList.AddElement(police);
            stadium = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Stadium_Button")));
            stadium.ButtonPressed += (string name) => NotifyEvent(UIStadiumButtonPressed);
            specialsList.AddElement(stadium);
            school = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/School_Button")));
            school.ButtonPressed += (string name) => NotifyEvent(UISchoolButtonPressed);
            specialsList.AddElement(school);
            university = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/Uni_Button")));
            university.ButtonPressed += (string name) => NotifyEvent(UIUniverisyButtonPressed);
            specialsList.AddElement(university);

            buttonsBar.AddElement(specialsList);

            RenderableVerticalList buttonsList = new RenderableVerticalList(5, 20, 30);

            budget = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/budget_button")));
            budget.ButtonPressed += (string name) => NotifyEvent(UIBudgetButtonPressed);
            buttonsList.AddElement(budget);

            disaster = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Buttons/disaster_Button")));
            disaster.ButtonPressed += (string name) => NotifyEvent(UIDisasterButtonPressed);
            buttonsList.AddElement(disaster);

            buttonsBar.AddElement(buttonsList);

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
            perspectiveButton.ButtonPressed += (string name) => { if (!is_menu_active || !is_game_over) ChangePerspective(); };
            ElementsInWindow.Add(perspectiveButton.ToIRenderable());
            
            RenderableContainer menuContainer = new(0, 0, ESize.AllScreen, Color.Black * 0.3f);
            Sprite menuBackground = new Sprite(Globals.Content.Load<Texture2D>("Menu/game_menu_back"));
            menuContainer.AddElement(EVPosition.Center, EHPosition.Center, menuBackground);

            RenderableVerticalList menuList = new(5, 0, 0);
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

            RenderableContainer gameEndContainer = new(0, 0, ESize.AllScreen, Color.Black * 0.3f);
            Sprite gameEndBackground = new Sprite(Globals.Content.Load<Texture2D>("Menu/game_end_back"));
            gameEndContainer.AddElement(EVPosition.Center, EHPosition.Center, gameEndBackground);

            RenderableVerticalList gameEndList1 = new(5, 0, 0);
            RenderableVerticalList gameEndList2 = new(20, 0, 0);
            Sprite gameEndSprite = new Sprite(Globals.Content.Load<Texture2D>("Menu/game_end"));
            gameEndList2.AddElement(gameEndSprite);
            ge_newgame = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_NewGame_Button")));
            gameEndList1.AddElement(ge_newgame);
            ge_newgame.ButtonPressed += (string name) => NotifyEndGameEvent(UIMenuNewGameButtonPressed);
            ge_load = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Small_Load_Button")));
            gameEndList1.AddElement(ge_load);
            ge_load.ButtonPressed += (string name) => NotifyEndGameEvent(UIMenuLoadGameButtonPressed);

            gameEndList2.AddElement(gameEndList1);

            gameEndContainer.AddElement(EVPosition.Center, EHPosition.Center, gameEndList2);

            gameOverWindow.AddRange(gameEndContainer.ToIRenderable());
        }

        private void NotifyEvent(UIButtonPressedHandler eve)
        {
            if (!is_menu_active && !is_game_over)
                eve.Invoke();
        }

        private void NotifyMenuEvent(UIButtonPressedHandler eve)
        {
            if (is_menu_active)
                eve.Invoke();
        }

        private void NotifyEndGameEvent(UIButtonPressedHandler eve)
        {
            if (is_game_over)
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

        public void PrintInfo(bool is_upgradeable, params Tuple<string, EPrintInfo>[] args)
        {
            screenWindow.Clear();

            RenderableContainer screenCont = new(gameScreenRect.X, gameScreenRect.Y, gameScreenRect.Size.ToVector2());
            screenCont.Margin = 20f;
            RenderableContainer cont = new(0, 0, ESize.FitContent, Color.LightGray);
            cont.Padding = 20f;
            RenderableHorizontalList mainList = new(10, 0, 0);

            RenderableVerticalList list = CreateList(args);

            if (is_upgradeable)
            {
                is_tile_info_bttn_active = true;
                upgradeBttn = new Button(new Sprite(Globals.Content.Load<Texture2D>("Buttons/upgrade_Button")), _inputHandler);
                upgradeBttn.ButtonPressed += (string name) => { if ((!is_menu_active || !is_game_over) && is_tile_info_bttn_active) UIUpgradeTileButtonPressed.Invoke(); };
                list.AddElement(upgradeBttn);
            }

            mainList.AddElement(list);

            screenWindowBttn = new Button(new Sprite(Globals.Content.Load<Texture2D>("Buttons/close_Button")), _inputHandler);
            screenWindowBttn.ButtonPressed += (string name) => { if ((!is_menu_active || !is_game_over) && is_tile_info_active) DeleteInfo(); };
            mainList.AddElement(screenWindowBttn);

            cont.AddElement(EVPosition.Top, EHPosition.Left, mainList);
            screenCont.AddElement(EVPosition.Bottom, EHPosition.Left, cont);
            tileInfoRect = new Rectangle(cont.CollisionRectangle.Location, cont.CollisionRectangle.Size);

            screenWindow.AddRange(screenCont.ToIRenderable());
        }

        public void DeleteInfo()
        {
            screenWindow.Clear();
            statsRect = new Rectangle();
            is_tile_info_active = false;
            is_tile_info_bttn_active = false;
        }

        public void PrintStats(params Tuple<string, EPrintInfo>[] args)
        {
            statsWindow.Clear();

            RenderableContainer screenCont = new(gameScreenRect.X, gameScreenRect.Y, gameScreenRect.Size.ToVector2());
            screenCont.Margin = 20f;
            RenderableContainer cont = new(0, 0, ESize.FitContent, Color.LightGray);
            cont.Padding = 20f;
            RenderableHorizontalList mainList = new(10, 0, 0);

            RenderableVerticalList list = CreateList(args);

            statsWindowBttn = new Button(new Sprite(Globals.Content.Load<Texture2D>("Buttons/close_Button")), _inputHandler);
            statsWindowBttn.ButtonPressed += (string name) => { if (!is_menu_active || !is_game_over) DeleteStats(); };

            mainList.AddElement(list);
            mainList.AddElement(statsWindowBttn);
            cont.AddElement(EVPosition.Top, EHPosition.Left, mainList);
            screenCont.AddElement(EVPosition.Top, EHPosition.Left, cont);
            statsRect = new Rectangle(cont.CollisionRectangle.Location, cont.CollisionRectangle.Size);

            statsWindow.AddRange(screenCont.ToIRenderable());
        }

        public void DeleteStats()
        {
            statsWindow.Clear();
            tileInfoRect = new Rectangle();
            is_budget_active = false;
        }

        private RenderableVerticalList CreateList(params Tuple<string, EPrintInfo>[] args)
        {
            RenderableVerticalList list = new(10, 0, 0);

            foreach (Tuple<string, EPrintInfo> item in args)
            {
                string text = item.Item1;
                EPrintInfo info = item.Item2;

                switch (info)
                {
                    case EPrintInfo.Title:
                        list.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/m6x11_26"), text, Vector2.Zero, Color.Black));
                        break;
                    case EPrintInfo.Normal:
                        list.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/m6x11_18"), text, Vector2.Zero, Color.Black));
                        break;
                    case EPrintInfo.Sublist:
                        list.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/m6x11_22"), text, Vector2.Zero, Color.Black));
                        break;
                }
            }

            return list;
        }

        public void ChangePerspective()
        {
            if (is2D)
            {
                Debug.WriteLine($"Switched to 2.5D");
                // run all the map and switch the textures
            }
            else
            {
                Debug.WriteLine("Switched to 2D");
                // run all the map and switch the textures
            }

            is2D = !is2D;
        }

        public void GameOver()
        {
            is_game_over = true;
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

            screenWindowBttn?.Update();
            statsWindowBttn?.Update();
            upgradeBttn?.Update();
        }

        public override void Draw(IRenderer renderer)
        {
            if (mapIsInit)
            {
                renderer.DrawState(this.mapRend);
                renderer.DrawState(this.disasters);
            }

            renderer.DrawState(screenWindow);
            renderer.DrawState(statsWindow);

            base.Draw(renderer);

            if (is_menu_active)
                renderer.DrawState(menuWindow);

            if (is_game_over)
                renderer.DrawState(gameOverWindow);
        }
    }
}
