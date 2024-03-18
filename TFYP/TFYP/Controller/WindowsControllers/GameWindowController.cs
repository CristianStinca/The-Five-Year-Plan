using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Model.GameObjects;
using TFYP.Utils;
using TFYP.View;
using TFYP.View.Windows;

namespace TFYP.Controller.WindowsControllers
{
    internal class GameWindowController : WindowController
    {
        private Vector2 focusCoord;
        private Vector4 screenLimits;
        public GameWindowController(InputHandler inputHandler, View.View _view, GameModel _gameModel) : base(inputHandler, _view, _gameModel)
        {
            focusCoord = new();

            if (_view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) != 0)
            {
                _view.CurrentWindow = GameWindow.Instance;
            }

            //_view.CurrentWindow.SendViewObject(new ViewObject("back2"));
            //_view.CurrentWindow.SendViewObject(new ViewObject(TypesConverison.GetVal(EBuildable.Stadium.ToString()), 0, 0, 10));

            screenLimits = new Vector4(
                0, 0,
                -(((GameModel.MAP_H / 2) - 1) * GameWindow.TILE_H * GameWindow.SCALE) + Globals.Graphics.PreferredBackBufferHeight,
                - (((GameModel.MAP_W / 2) - 1) * GameWindow.TILE_W * GameWindow.SCALE) + Globals.Graphics.PreferredBackBufferWidth
            );

            _gameModel.Build(3, 4, EBuildable.Stadium);
        }

        public override void Update()
        {
            base.Update();

            var map = gameModel.map;
            ViewObject[,] out_map = new ViewObject[map.GetLength(0), map.GetLength(1)];

            // TODO: add an event/obs collection to model to avoid re-drawing of the matrix

            // trnsform the array of Buildable from the model to array of ViewObject for View
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    out_map[i, j] = new ViewObject(TypesConverison.GetVal(map[i, j].type.ToString()));
                }
            }

            GameWindow gw_view = null;
            if (view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) == 0)
            {
                gw_view = (GameWindow)view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("GameWindowController (set_map)");
            }

            // sending the new array to the View
            gw_view.SendGameMap(out_map);

            int speed = 20;

            // reading the keys
            foreach (KeyboardButtonState key in inputHandler.ActiveKeys) 
            {
                if (key.Button == Keys.Escape && key.ButtonState == Utils.KeyState.Clicked)
                {
                    OnExitPressed();
                }
                else if (key.Button == Keys.S && key.ButtonState == Utils.KeyState.Clicked)
                {
                    Debug.WriteLine("Stadium Invoked (Click)");
                }
                else if (key.Button == Keys.Up && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(0, speed));
                }
                else if (key.Button == Keys.Down && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(0, -speed));
                }
                else if (key.Button == Keys.Left && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(speed, 0));
                }
                else if (key.Button == Keys.Right && key.ButtonState == Utils.KeyState.Held)
                {
                    ExecuteFocusMove(new Vector2(-speed, 0));
                }

                gw_view.SetFocusCoord(focusCoord);
            }
        }

        private bool ExecuteFocusMove(Vector2 direction)
        {
            Vector2 result = focusCoord + direction;

            if (result.X <= screenLimits.X && result.Y <= screenLimits.Y && result.Y >= screenLimits.Z && result.X >= screenLimits.W)
            {
                focusCoord = result;
                return true;
            }

            return false;
        }
    }
}
