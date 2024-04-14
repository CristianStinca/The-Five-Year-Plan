﻿using Microsoft.Xna.Framework;
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

namespace TFYP.Controller.WindowsControllers
{
    internal class GameWindowController : WindowController
    {
        private Vector2 _focusCoord; 
        private Rectangle _screenLimits;

        View.Windows.GameWindow _gw_view;

        private EBuildable? _activeZone;

        public GameWindowController(InputHandler inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
            : base(inputHandler, _view, _uiTextures, _gameModel)
        {
            _view.changeToGameWindow();

            _focusCoord = new();
            _activeZone = null;
            InitiateConverionDict();

            _screenLimits = new Rectangle(0, 0,
                            (int)Math.Round(((GameModel.MAP_W - 0.5f) * View.Windows.GameWindow.TILE_W * View.Windows.GameWindow.SCALE) - Globals.Graphics.PreferredBackBufferWidth),
                            (((GameModel.MAP_H - 1) / 2) * View.Windows.GameWindow.TILE_H * View.Windows.GameWindow.SCALE) - Globals.Graphics.PreferredBackBufferHeight
            );

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
        }

        /// <summary>
        /// Method to manage the event of a button click.
        /// </summary>
        /// <param name="x">The x coordinate of the tile.</param>
        /// <param name="y">The y coordinate of the tile.</param>
        /// <param name="btn">The name of the mouse button (L/R)</param>
        public void ClickInButton(int x, int y, string btn)
        {

            //Zone zone1 = new Zone(EBuildable.Stadium);
            //Zone zone2 = new Zone(EBuildable.None);

            //Debug.WriteLine($"Clicked on X: {x}, Y: {y}");
            switch (btn)
            {
                case "L":
                    //Debug.WriteLine($"X: {Mouse.GetState().X}, Y: {Mouse.GetState().Y}.");
                    if (_activeZone != null)
                    {
                        _gameModel.AddZone(y, x, (EBuildable)_activeZone);
                    }
                    break;

                case "R":
                    _activeZone = null;
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            var map = _gameModel.map;
            ISprite[,] out_map = new ISprite[map.GetLength(0), map.GetLength(1)];

            // TODO: add an event/obs collection to model to avoid re-drawing of the matrix

            // trnsform the array of Buildable from the model to array of ViewObject for View
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    out_map[i, j] = this.CreateUIElement(map[i, j].type);
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
                    OnExitPressed();
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

                _gw_view.SetFocusCoord(-_focusCoord);
            }
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

        #region MODEL_TO_VIEW_TYPE_CONVERSIONS

        private Dictionary<EBuildable, ISprite> conversionDict;

        /// <summary>
        /// Initiates the dictionary for conversions between EBuildable and UITexture
        /// </summary>
        private void InitiateConverionDict()
        {
            conversionDict = new()
            {
                { EBuildable.None, _uiTextures.EmptyTile },
                { EBuildable.Stadium, _uiTextures.StadiumTile }
            };
        }

        /// <summary>
        /// Just a converter.
        /// </summary>
        /// <param name="from">An EBuildable object.</param>
        /// <returns>A IRenderable object.</returns>
        /// <exception cref="ArgumentException">Raises exception if the EBuildable is not in the dictionary.</exception>
        private ISprite CreateUIElement(EBuildable from)
        {
            if (!conversionDict.ContainsKey(from))
            {
                throw new ArgumentException(from.ToString());
            }

            return conversionDict[from];
        }

        private void LinkViewEvents()
        {
            _gw_view.UIResidentialZoneButtonPressed += () => _activeZone = EBuildable.Residential;
            _gw_view.UIIndustrialZoneButtonPressed += () => _activeZone = EBuildable.Industrial;
            _gw_view.UICommertialZoneButtonPressed += () => _activeZone = EBuildable.Service;
            //_gw_view.UIDeleteZoneButtonPressed += () => _activeZone = EBuildable.None;
            _gw_view.UIBuildRoadButtonPressed += () => _activeZone = EBuildable.Road;
            //_gw_view.UIDeleteRoadButtonPressed += () => _activeZone = EBuildable.None;
            _gw_view.UIDeleteButtonPressed += () => _activeZone = EBuildable.None;
            _gw_view.UIPoliceButtonPressed += () => _activeZone = EBuildable.PoliceStation;
            _gw_view.UIStadiumButtonPressed += () => _activeZone = EBuildable.Stadium;
            _gw_view.UISchoolButtonPressed += () => _activeZone = EBuildable.School;
        }

        #endregion
    }
}
