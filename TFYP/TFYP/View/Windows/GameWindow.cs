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
using TFYP.Model.Facilities;
using Microsoft.Xna.Framework.Input;

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
        private List<Rectangle> _UIElementsContainers;
        private List<Button> _UIElements;

        List<IRenderable> mapRend = new List<IRenderable>();

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

        public delegate void UIResidentialZoneButtonPressedHandler();
        public event UIResidentialZoneButtonPressedHandler UIResidentialZoneButtonPressed;

        public delegate void UIIndustrialZoneButtonPressedHandler();
        public event UIIndustrialZoneButtonPressedHandler UIIndustrialZoneButtonPressed;

        public delegate void UICommertialZoneButtonPressedHandler();
        public event UICommertialZoneButtonPressedHandler UICommertialZoneButtonPressed;

        //public delegate void UIDeleteZoneButtonPressedHandler();
        //public event UIDeleteZoneButtonPressedHandler UIDeleteZoneButtonPressed;

        public delegate void UIBuildRoadButtonPressedHandler();
        public event UIBuildRoadButtonPressedHandler UIBuildRoadButtonPressed;

        //public delegate void UIDeleteRoadButtonPressedHandler();
        //public event UIDeleteRoadButtonPressedHandler UIDeleteRoadButtonPressed;
        public delegate void UIDeleteButtonPressedHandler();
        public event UIDeleteButtonPressedHandler UIDeleteButtonPressed;

        public delegate void UIPoliceButtonPressedHandler();
        public event UIPoliceButtonPressedHandler UIPoliceButtonPressed;

        public delegate void UIStadiumButtonPressedHandler();
        public event UIStadiumButtonPressedHandler UIStadiumButtonPressed;

        public delegate void UISchoolButtonPressedHandler();
        public event UISchoolButtonPressedHandler UISchoolButtonPressed;

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

            Sprite BackgroundButtonTab = new (Globals.Content.Load<Texture2D>("ButtonsHolder"));
            _UIElementsContainers.Add(new Rectangle(0, 0, BackgroundButtonTab.Texture.Width, BackgroundButtonTab.Texture.Height));
            ElementsInWindow.Add(BackgroundButtonTab);

            RenderableList buttonsBar = new (30, 20, 30);

            delRoad = new Button(new Sprite(Globals.Content.Load<Texture2D>("Demolish_Road_Button")), _inputHandler);
            delRoad.ButtonPressed += UIButtonPressed;
            _UIElements.Add(delRoad);
            buttonsBar.AddElement(delRoad);

            RenderableList zonesList = new RenderableList(10, 20, 30);
            zonesList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Zones", new Vector2(20, 30), Color.Black));

            resZone = new Button(new Sprite(Globals.Content.Load<Texture2D>("Residential_Zone_Button")), _inputHandler);
            resZone.ButtonPressed += UIButtonPressed;
            _UIElements.Add(resZone);
            zonesList.AddElement(resZone);
            indZone = new Button(new Sprite(Globals.Content.Load<Texture2D>("Industrial_Zone_Button")), _inputHandler);
            indZone.ButtonPressed += UIButtonPressed;
            _UIElements.Add(indZone);
            zonesList.AddElement(indZone);
            commZone = new Button(new Sprite(Globals.Content.Load<Texture2D>("Commertial_Zone_Button")), _inputHandler);
            commZone.ButtonPressed += UIButtonPressed;
            _UIElements.Add(commZone);
            zonesList.AddElement(commZone);
            //delZone = new Button(new Sprite(Globals.Content.Load<Texture2D>("Erase_Zone_Button")), _inputHandler);
            //delZone.ButtonPressed += UIButtonPressed;
            //_UIElements.Add(delZone);
            //zonesList.AddElement(delZone);

            buttonsBar.AddElement(zonesList);

            RenderableList roadList = new RenderableList(10, 20, 30);
            roadList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Roads", new Vector2(20, 30), Color.Black));
            buildRoad = new Button(new Sprite(Globals.Content.Load<Texture2D>("Build_Road_Button")), _inputHandler);
            buildRoad.ButtonPressed += UIButtonPressed;
            _UIElements.Add(buildRoad);
            roadList.AddElement(buildRoad);

            buttonsBar.AddElement(roadList);

            RenderableList specialsList = new RenderableList(10, 20, 30);
            specialsList.AddElement(new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Specials", new Vector2(20, 30), Color.Black));
            police = new Button(new Sprite(Globals.Content.Load<Texture2D>("Police_Button")), _inputHandler);
            police.ButtonPressed += UIButtonPressed;
            _UIElements.Add(police);
            specialsList.AddElement(police);
            stadium = new Button(new Sprite(Globals.Content.Load<Texture2D>("Stadium_Button")), _inputHandler);
            stadium.ButtonPressed += UIButtonPressed;
            _UIElements.Add(stadium);
            specialsList.AddElement(stadium);
            school = new Button(new Sprite(Globals.Content.Load<Texture2D>("School_Button")), _inputHandler);
            school.ButtonPressed += UIButtonPressed;
            _UIElements.Add(school);
            specialsList.AddElement(school);

            buttonsBar.AddElement(specialsList);

            ElementsInWindow.AddRange(buttonsBar.ToIRenderable());
        }

        private void UIButtonPressed(string name)
        {
            switch (name)
            {
                case var value when value == resZone.Sprite.Texture.Name:
                    UIResidentialZoneButtonPressed.Invoke();
                    break;
                case var value when value ==  indZone.Sprite.Texture.Name:
                    UIIndustrialZoneButtonPressed.Invoke();
                    break;
                case var value when value ==  commZone.Sprite.Texture.Name:
                    UICommertialZoneButtonPressed.Invoke();
                    break;
                //case var value when value ==  delZone.Sprite.Texture.Name:
                //    UIDeleteZoneButtonPressed.Invoke();
                //    break;
                case var value when value ==  buildRoad.Sprite.Texture.Name:
                    UIBuildRoadButtonPressed.Invoke();
                    break;
                //case var value when value ==  delRoad.Sprite.Texture.Name:
                //    UIDeleteRoadButtonPressed.Invoke();
                //    break;
                case var value when value ==  delRoad.Sprite.Texture.Name:
                    UIDeleteButtonPressed.Invoke();
                    break;
                case var value when value ==  police.Sprite.Texture.Name:
                    UIPoliceButtonPressed.Invoke();
                    break;
                case var value when value ==  stadium.Sprite.Texture.Name:
                    UIStadiumButtonPressed.Invoke();
                    break;
                case var value when value ==  school.Sprite.Texture.Name:
                    UISchoolButtonPressed.Invoke();
                    break;
            }
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
