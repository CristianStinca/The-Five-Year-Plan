using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;
using TFYP.Utils;
using TFYP.View;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using TFYP.View.Windows;
using static TFYP.View.Windows.GameWindow;
using static TFYP.View.Windows.Window;
using MonoGame.Extended.Timers;

namespace TFYP.Controller.WindowsControllers
{
    internal class GameWindowController : WindowController
    {
        private GameModel _gameModel;

        private Vector2 _focusCoord; 
        private Vector2 _initCoord; 
        private Rectangle _screenLimits;

        View.Windows.GameWindow _gw_view;

        private EBuildable? _activeZone;
        private Point? _selectedZone;
        private bool _win_is_open = false;

        private bool _menu_is_active = false;

        private int speed = 1;
        private bool stop = false;

        private double elapsedTime = 0;

        public GameWindowController(InputHandler inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
            : base(inputHandler, _view, _uiTextures)
        {
            _view.ChangeToGameWindow();
            this._gameModel = _gameModel;

            _activeZone = null;

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.GameWindow).Name) == 0)
            {
                _gw_view = (View.Windows.GameWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("GameWindowController (set_map)");
            }

            LinkViewEvents();
            _gw_view.TileButtonPressedInWindow += ClickInButton;
            //_gw_view.UIMenuNewGameButtonPressed += ToGameWindow;
            _screenLimits = _gw_view.ScreenLimit;
            _focusCoord = new Vector2(_screenLimits.X + _gw_view.ScreenLimit.Width / 2, _screenLimits.Y + _gw_view.ScreenLimit.Height / 2);
            _initCoord = new Vector2(_screenLimits.X, _screenLimits.Y);
            _gw_view.SetFocusCoord(_initCoord - _focusCoord);
        }

        /// <summary>
        /// Method to manage the event of a button click.
        /// </summary>
        /// <param name="x">The x coordinate of the tile.</param>
        /// <param name="y">The y coordinate of the tile.</param>
        /// <param name="btn">The name of the mouse button (L/R)</param>
        public void ClickInButton(int x, int y, string btn)
        {
            switch (btn)
            {
                case "L":
                    //_gw_view.DeleteInfo();

                    if (_activeZone != null)
                    {
                        //_win_is_open = false;
                        _gameModel.AddZone(x, y, (EBuildable)_activeZone);
                    }
                    else if (!_win_is_open)
                    {
                        //_win_is_open = true;
                        _selectedZone = new Point(x, y);
                        _gw_view.is_tile_info_active = true;
                    }
                    break;

                case "R":
                    Debug.WriteLine($"X: {x}, Y: {y}");

                    _activeZone = null;
                    _gw_view.DeleteInfo();
                    _gw_view.is_tile_info_active = false;
                    //_win_is_open = false;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!stop)
            {
                elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * this.speed;

                if (elapsedTime > 5000)
                {
                    elapsedTime = 0;

                    //update the model every 5 seconds on normal speed
                    _gameModel.UpdateCityState();
                }
            }

            if (_gw_view.is_tile_info_active && _selectedZone != null)
            {
                SendTileInfo();
            }

            if (_gw_view.is_budget_active)
            {
                SendBudgetInfo();
            }

            var map = _gameModel.map;
            ISprite[,] out_map = new ISprite[map.GetLength(0), map.GetLength(1)];

            // TODO: add an event/obs collection to model to avoid re-drawing of the matrix

            // trnsform the array of Buildable from the model to array of ViewObject for View
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    ISprite sprite = _uiTextures.EmptyTile;

                    switch (map[i, j].Type)
                    {
                        case EBuildable.Stadium:
                            sprite = _uiTextures.StadiumTile;
                            break;

                        case EBuildable.School:
                            sprite = _uiTextures.SchoolTile;
                            break;

                        case EBuildable.PoliceStation:
                            sprite = _uiTextures.PoliceTile;
                            break;

                        case EBuildable.Residential:
                            sprite = _uiTextures.ResidentialTile;
                            break;

                        case EBuildable.Industrial:
                            sprite = _uiTextures.IndustrialTile;
                            break;

                        case EBuildable.Service:
                            sprite = _uiTextures.ServiceTile;
                            break;

                        case EBuildable.Road:
                            sprite = DecideRoad(j, i);
                            break;

                        case EBuildable.DoneResidential:
                            sprite = _uiTextures.DoneResidentialTile;
                            break;

                        case EBuildable.Inaccessible:
                            sprite = _uiTextures.Inaccessible;
                            break;
                    }

                    out_map[i, j] = sprite;
                }
            }

            // sending the new array to the View
            _gw_view.SendGameMap(out_map);

            int speed = 20;

            // reading the keys
            foreach (KeyboardButtonState key in _inputHandler.ActiveKeys) 
            {
                if (key.Button == Keys.Escape && key.ButtonState == Utils.KeyState.Clicked)
                {
                    //OnExitPressed();
                    if (_menu_is_active)
                        _gw_view.CleanMenu();
                    else
                        _gw_view.DrawMenu();

                    _menu_is_active = !_menu_is_active;
                }
                else if (key.Button == Keys.Up && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(0, -speed));
                }
                else if (key.Button == Keys.Down && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(0, speed));
                }
                else if (key.Button == Keys.Left && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(-speed, 0));
                }
                else if (key.Button == Keys.Right && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(speed, 0));
                }

                _gw_view.SetFocusCoord(_initCoord - _focusCoord);
            }
        }

        private Sprite DecideRoad(int x, int y)
        {
            byte dir = 0b_0000;
            Buildable[] arr = GetAdj(x, y);
            if (arr[0]?.Type == EBuildable.Road)
                dir |= 0b_1000;
            if (arr[1]?.Type == EBuildable.Road)
                dir |= 0b_0100;
            if (arr[2]?.Type == EBuildable.Road)
                dir |= 0b_0010;
            if (arr[3]?.Type == EBuildable.Road)
                dir |= 0b_0001;

            //Debug.WriteLine($"X: {x}, Y: {y}");
            return _uiTextures.RoadTiles[dir];
        }

        private Buildable[] GetAdj(int x, int y)
        {
            Buildable[] arr = new Buildable[4];

            if (y % 2 == 1)
            {
                arr[0] = _gameModel.GetMapElementAt(x, y - 1);
                arr[1] = _gameModel.GetMapElementAt(x + 1, y - 1);
                arr[2] = _gameModel.GetMapElementAt(x + 1, y + 1);
                arr[3] = _gameModel.GetMapElementAt(x, y + 1);
            }
            else
            {
                arr[0] = _gameModel.GetMapElementAt(x - 1, y - 1);
                arr[1] = _gameModel.GetMapElementAt(x, y - 1);
                arr[2] = _gameModel.GetMapElementAt(x, y + 1);
                arr[3] = _gameModel.GetMapElementAt(x - 1, y + 1);
            }

            return arr;
        }

        private void SendTileInfo()
        {
            Point zn_point = (Point)_selectedZone;
            Buildable z = (Buildable)_gameModel.GetMapElementAt(zn_point.X, zn_point.Y);
            if (z is Zone)
            {
                Zone zn = (Zone)z;

                _gw_view.PrintInfo(Tuple.Create(z.Type.ToString(), EPrintInfo.Title),
                                   Tuple.Create("Level: " + zn.Level.ToString(), EPrintInfo.Normal),
                                   Tuple.Create("Number of citizens: " + zn.NCitizensInZone.ToString() + " / " + zn.Capacity.ToString(), EPrintInfo.Normal),
                                   Tuple.Create("Satisfaction: " + zn.GetZoneSatisfaction(_gameModel), EPrintInfo.Normal)
                );
            }
            else
            {
                if (z.Type == EBuildable.Inaccessible || z.Type == EBuildable.Road)
                {
                    _gw_view.DeleteInfo();
                    return;
                }

                _gw_view.PrintInfo(Tuple.Create(z.Type.ToString(), EPrintInfo.Title),
                                   Tuple.Create("Cap", EPrintInfo.Normal),
                                   Tuple.Create("Residents", EPrintInfo.Normal)
                );
            }
        }

        private void SendBudgetInfo()
        {
            _gw_view.PrintStats(Tuple.Create("City budget: " + _gameModel.Statistics.Budget.Balance, EPrintInfo.Title));
        }

        /// <summary>
        /// Method that tries to move the map.
        /// </summary>
        /// <param name="direction">The direction of the move.</param>
        /// <returns>True if a move will be executed.</returns>
        private bool ExecuteFocusMove(Vector2 direction)
        {
            Vector2 result = _focusCoord + direction;

            if (_screenLimits.Contains(result))
            {
                _focusCoord = result;
                return true;
            }

            return false;
        }

        #region LINKING_VIEW_EVENTS

        private void LinkViewEvents()
        {
            _gw_view.UIResidentialZoneButtonPressed += () => { _activeZone = EBuildable.Residential; Debug.WriteLine("Selected Residential."); };
            _gw_view.UIIndustrialZoneButtonPressed += () => { _activeZone = EBuildable.Industrial; Debug.WriteLine("Selected Industrial."); };
            _gw_view.UICommertialZoneButtonPressed += () => { _activeZone = EBuildable.Service; Debug.WriteLine("Selected Service."); };
            //_gw_view.UIDeleteZoneButtonPressed += () => { _activeZone = EBuildable.None; Debug.WriteLine("Selected None."); };
            _gw_view.UIBuildRoadButtonPressed += () => { _activeZone = EBuildable.Road; Debug.WriteLine("Selected Road."); };
            //_gw_view.UIDeleteRoadButtonPressed += () => { _activeZone = EBuildable.None; Debug.WriteLine("Selected None."); };
            _gw_view.UIDeleteButtonPressed += () => { _activeZone = EBuildable.None; Debug.WriteLine("Selected Delete."); };
            _gw_view.UIPoliceButtonPressed += () => { _activeZone = EBuildable.PoliceStation; Debug.WriteLine("Selected PoliceStation."); };
            _gw_view.UIStadiumButtonPressed += () => { _activeZone = EBuildable.Stadium; Debug.WriteLine("Selected Stadium."); };
            _gw_view.UISchoolButtonPressed += () => { _activeZone = EBuildable.School; Debug.WriteLine("Selected School."); };
            _gw_view.UIBudgetButtonPressed += () => { Debug.WriteLine(_gameModel.Statistics.Budget.Balance); _gw_view.is_budget_active = true; };

            _gw_view.UIStopSpeedPressed += () => { stop = true; Debug.WriteLine("Speed stop."); };
            _gw_view.UISpeedX1Pressed += () => { speed = 1; stop = false; Debug.WriteLine("Speed X1."); };
            _gw_view.UISpeedX2Pressed += () => { speed = 2; stop = false; Debug.WriteLine("Speed X2."); };
            _gw_view.UISpeedX3Pressed += () => { speed = 3; stop = false; Debug.WriteLine("Speed X3."); };

            // TODO: from menu go back to game
            _gw_view.UIMenuNewGameButtonPressed += ToGameWindow;
            _gw_view.UIMenuSaveGameButtonPressed += ToLoadsWindow;
            _gw_view.UIMenuLoadGameButtonPressed += ToLoadsWindow;
            _gw_view.UIMenuOpenSettingsButtonPressed += ToSettingsWindow;
            _gw_view.UIMenuExitButtonPressed += () => base.OnExitPressed();
    }

        #endregion
    }
}
