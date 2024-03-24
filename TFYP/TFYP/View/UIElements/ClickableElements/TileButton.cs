using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.Windows;

namespace TFYP.View.UIElements.ClickableElements
{
    internal class TileButton : Button
    {
        public delegate void TileButtonPressedHandler(int x, int y, string btn);
        public event TileButtonPressedHandler TileButtonPressed;

        private Vector2[] _vertecies;

        private int _x;
        private int _y;

        public TileButton(Sprite sprite, InputHandler inputHandler, int x, int y)
            : base(sprite, inputHandler)
        {
            int width = Windows.GameWindow.TILE_W * Windows.GameWindow.SCALE;
            int height = Windows.GameWindow.TILE_H * Windows.GameWindow.SCALE;
            this._x = x;
            this._y = y;

            _vertecies = new Vector2[]
            {
                new ((width / 2) + sprite.Position.X, sprite.Position.Y),
                new ((width) + sprite.Position.X, (height / 2) + sprite.Position.Y),
                new ((width / 2) + sprite.Position.X, height + sprite.Position.Y),
                new (sprite.Position.X, (height / 2) + sprite.Position.Y)
            };
        }

        public override bool IsMouseOverButton(MouseState mouse_state)
        {
            Vector2 q_point = mouse_state.Position.ToVector2();
            return IsPointOverButton(q_point);
        }

        public bool IsPointOverButton(Vector2 point)
        {
            bool result = false;
            int j = _vertecies.Length - 1;
            for (int i = 0; i < _vertecies.Length; i++)
            {
                if (_vertecies[i].Y < point.Y && _vertecies[j].Y >= point.Y ||
                    _vertecies[j].Y < point.Y && _vertecies[i].Y >= point.Y)
                {
                    if (_vertecies[i].X + (point.Y - _vertecies[i].Y) /
                       (_vertecies[j].Y - _vertecies[i].Y) *
                       (_vertecies[j].X - _vertecies[i].X) < point.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public override void Update()
        {
            MouseState mouse_state = Mouse.GetState();

            if (IsMouseOverButton(mouse_state) && _inputHandler.LeftButton == Utils.KeyState.Clicked)
            {
                TileButtonPressed.Invoke(this._x, this._y, "L");
            }

            if (IsMouseOverButton(mouse_state) && _inputHandler.RightButton == Utils.KeyState.Clicked)
            {
                TileButtonPressed.Invoke(this._x, this._y, "R");
            }
        }
    }
}

