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
using TFYP.Model.GameObjects;
using TFYP.Utils;
using TFYP.View;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using TFYP.View.Windows;

namespace TFYP.Controller.WindowsControllers
{
    internal class GameWindowController : WindowController
    {
        private Vector2 focusCoord; 
        private Vector4 screenLimits;
        private TileButton t_button;
        private Button button;
        public GameWindowController(InputHandler inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
            : base(inputHandler, _view, _uiTextures, _gameModel)
        {
            _view.changeToGameWindow();

            focusCoord = new();
            InitiateConverionDict();

            screenLimits = new Vector4(
                0, 0,
                -(((GameModel.MAP_H / 2) - 1) * View.Windows.GameWindow.TILE_H * View.Windows.GameWindow.SCALE) + Globals.Graphics.PreferredBackBufferHeight,
                - (((GameModel.MAP_W / 2) - 1) * View.Windows.GameWindow.TILE_W * View.Windows.GameWindow.SCALE) + Globals.Graphics.PreferredBackBufferWidth
            );

            _gameModel.Build(3, 4, EBuildable.Stadium);
            _gameModel.Build(2, 2, EBuildable.Stadium);

            Texture2D texture = uiTextures.StadiumTile.Texture;

            t_button = new TileButton(new Sprite(texture, (float)View.Windows.GameWindow.SCALE), texture, texture, inputHandler, focusCoord);
            //button = new Button(new Sprite(texture, (float)View.Windows.GameWindow.SCALE), texture, texture, inputHandler);
            t_button.TileButtonPressed += ClickInButton;
        }

        public void ClickInButton()
        {
            Debug.WriteLine("Clicked!!!");
        }

        public override void Update()
        {
            base.Update();

            var map = gameModel.map;
            IRenderable[,] out_map = new IRenderable[map.GetLength(0), map.GetLength(1)];

            // TODO: add an event/obs collection to model to avoid re-drawing of the matrix

            // trnsform the array of Buildable from the model to array of ViewObject for View
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    out_map[i, j] = this.CreateUIElement(map[i, j].type);
                }
            }

            View.Windows.GameWindow gw_view = null;
            if (view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.GameWindow).Name) == 0)
            {
                gw_view = (View.Windows.GameWindow)view.CurrentWindow;
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

            if (inputHandler.LeftButton == Utils.KeyState.Clicked)
            {
                //MouseState ms = Mouse.GetState();
                //Debug.WriteLine($"X: {ms.X}, Y: {ms.Y}");
            }
            else if (inputHandler.RightButton == Utils.KeyState.Clicked)
            {
                Vector2 ms = Mouse.GetState().Position.ToVector2() - this.focusCoord;
                Debug.WriteLine($"X: {ms.X}, Y: {ms.Y}");
            }

            t_button.Update();
        }

        private bool ExecuteFocusMove(Vector2 direction)
        {
            Vector2 result = focusCoord + direction;

            //if (result.X <= screenLimits.X && result.Y <= screenLimits.Y && result.Y >= screenLimits.Z && result.X >= screenLimits.W)
            {
                focusCoord = result;
                return true;
            }

            return false;
        }

        private Dictionary<EBuildable, IRenderable> conversionDict;
        private void InitiateConverionDict()
        {
            conversionDict = new()
            {
                { EBuildable.None, uiTextures.EmptyTile },
                { EBuildable.Stadium, uiTextures.StadiumTile }
            };
        }

        private IRenderable CreateUIElement(EBuildable from)
        {
            if (!conversionDict.ContainsKey(from))
            {
                throw new ArgumentException(from.ToString());
            }

            return conversionDict[from];
        }
    }
}
