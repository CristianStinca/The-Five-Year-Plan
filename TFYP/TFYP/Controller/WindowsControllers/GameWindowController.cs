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
        public GameWindowController(InputHandler inputHandler, View.View _view, GameModel _gameModel) : base(inputHandler, _view, _gameModel)
        {
            focusCoord = new();

            if (_view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) != 0)
            {
                _view.CurrentWindow = GameWindow.Instance;
            }
            //_view.CurrentWindow.SendViewObject(new ViewObject("back2"));
            //_view.CurrentWindow.SendViewObject(new ViewObject(TypesConverison.GetVal(EBuildable.Stadium.ToString()), 0, 0, 10));\\

            var map = gameModel.map;
            ViewObject[,] out_map = new ViewObject[map.GetLength(0), map.GetLength(1)];

            // TODO: add an event/obs collection to model to avoid re-drawing of the matrix

            // trnsform the array of Buildable from the model to array of ViewObject for View
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    out_map[i, j] = new ViewObject(TypesConverison.GetVal(map[i, j].type.ToString()), 0, 0, 10);
                }
            }

            // sending the new array to the View
            if (view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) == 0)
            {
                Debug.WriteLine("game map sent!");
                ((GameWindow)view.CurrentWindow).SendGameMap(out_map);
            }
            else
            {
                throw new TypeLoadException("GameWindowController (set_map)");
            }
        }

        public override void Update()
        {
            base.Update();

            

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
                //else if (key.Button == Keys.Up && key.ButtonState == Utils.KeyState.Held)
                //{
                //    Vector2 up = new(1, 0);
                //    focusCoord += up;

                //    if (view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) == 0)
                //    {
                //        ((GameWindow)view.CurrentWindow).SetFocusCoord(focusCoord);
                //    }
                //    else
                //    {
                //        throw new TypeLoadException("GameWindowController (constructor)");
                //    }
                //}
                //else if (key.Button == Keys.Down && key.ButtonState == Utils.KeyState.Held)
                //{
                //    Vector2 down = new(-1, 0);
                //    focusCoord += down;

                //    if (view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) == 0)
                //    {
                //        ((GameWindow)view.CurrentWindow).SetFocusCoord(focusCoord);
                //    }
                //    else
                //    {
                //        throw new TypeLoadException("GameWindowController (constructor)");
                //    }
                //}
                //else if (key.Button == Keys.Left && key.ButtonState == Utils.KeyState.Held)
                //{
                //    Vector2 left = new(0, -1);
                //    focusCoord += left;

                //    if (view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) == 0)
                //    {
                //        ((GameWindow)view.CurrentWindow).SetFocusCoord(focusCoord);
                //    }
                //    else
                //    {
                //        throw new TypeLoadException("GameWindowController (constructor)");
                //    }
                //}
                //else if (key.Button == Keys.Right && key.ButtonState == Utils.KeyState.Held)
                //{
                //    Vector2 right = new(0, 1);
                //    focusCoord += right;

                //    if (view.CurrentWindow.GetType().Name.CompareTo(typeof(GameWindow).Name) == 0)
                //    {
                //        ((GameWindow)view.CurrentWindow).SetFocusCoord(focusCoord);
                //    }
                //    else
                //    {
                //        throw new TypeLoadException("GameWindowController (constructor)");
                //    }
                //}
            }
        }
    }
}
