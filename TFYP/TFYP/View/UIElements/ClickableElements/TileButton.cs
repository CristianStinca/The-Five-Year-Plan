using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
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
        public delegate void TileButtonPressedHandler(int col, int row, int x, int y, string btn);
        public event TileButtonPressedHandler TileButtonPressed;
        public delegate void TileButtonHoverHandler(int col, int row, int x, int y);
        public event TileButtonHoverHandler TileButtonHover;

        private Vector2[] _vertecies;

        private int _col;
        private int _row;
        private int width;
        private int height;

        private bool activated = false;

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                _vertecies = new Vector2[]
                {
                    new ((width / 2) + Position.X, Position.Y),
                    new ((width) + Position.X, (height / 2) + Position.Y),
                    new ((width / 2) + Position.X, height + Position.Y),
                    new (Position.X, (height / 2) + Position.Y)
                };
            }
        }

        public TileButton(Sprite sprite, InputHandler inputHandler, int col, int row)
            : base(sprite, inputHandler)
        {
            width = Windows.GameWindow.TILE_W * Windows.GameWindow.SCALE;
            height = Windows.GameWindow.TILE_H * Windows.GameWindow.SCALE;
            this._col = col;
            this._row = row;

            _vertecies = new Vector2[]
            {
                new ((width / 2) + Position.X, Position.Y),
                new ((width) + Position.X, (height / 2) + Position.Y),
                new ((width / 2) + Position.X, height + Position.Y),
                new (Position.X, (height / 2) + Position.Y)
            };
        }

        public override bool IsMouseOverButton(MouseState mouse_state)
        {
            Vector2 q_point = mouse_state.Position.ToVector2();
            return IsPointOverButton(q_point);
        }

        /// <summary>
        /// Checks if a pair of coordinates are inside the Button margins.
        /// </summary>
        /// <param name="point">The pair of coordinates.</param>
        /// <returns>True if the point is inside the Button margins.</returns>
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

            if (IsMouseOverButton(mouse_state))
            {
                TileButtonHover.Invoke(this._col, this._row, mouse_state.Position.X, mouse_state.Position.Y);

                if (_inputHandler.LeftButton == Utils.KeyState.Clicked || _inputHandler.LeftButton == Utils.KeyState.Held)
                {
                    if (!activated)
                    {
                        activated = true;
                        TileButtonPressed.Invoke(this._col, this._row, mouse_state.Position.X, mouse_state.Position.Y, "L");
                    }
                }

                if (_inputHandler.RightButton == Utils.KeyState.Clicked)
                {
                    TileButtonPressed.Invoke(this._col, this._row, mouse_state.Position.X, mouse_state.Position.Y, "R");
                }
            }

            if (_inputHandler.LeftButton == Utils.KeyState.Released)
            {
                activated = false;
            }
        }
    }
}

