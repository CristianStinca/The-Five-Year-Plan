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
using TFYP.View.UIElements;
using TFYP.View.Windows;

namespace TFYP.Controller
{
    internal class TileButton : Button
    {
        public delegate void TileButtonPressedHandler();
        public event ButtonPressedHandler TileButtonPressed;

        private Vector2[] _vertecies;
        private Vector2 pos;

        public TileButton(Sprite sprite, Texture2D hoverImage, Texture2D normalImage, InputHandler inputHandler, Vector2 pos)
            : base(sprite, hoverImage, normalImage, inputHandler)
        {
            int width = View.Windows.GameWindow.TILE_W * View.Windows.GameWindow.SCALE;
            int height = View.Windows.GameWindow.TILE_H * View.Windows.GameWindow.SCALE;

            _vertecies = new Vector2[]
            {
                new (width / 2, 0),
                new (width, height / 2),
                new (width / 2, height),
                new (0, height / 2)
            };

            for (int i = 0; i < _vertecies.Length; i++)
            {
                Debug.WriteLine($"V.X: {_vertecies[i].X}, V.Y: {_vertecies[i].Y}");
            }
            Vector2 vec = new Vector2(width / 2, height / 2);
            Debug.WriteLine($"NV.X: {vec.X}, NV.Y: {vec.Y}");
            Debug.WriteLine(IsPointOverButton(vec));
        }

        public override bool IsMouseOverButton()
        {
            Vector2 q_point = Mouse.GetState().Position.ToVector2() - this.pos;
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

            if (IsMouseOverButton())
            {
                //Debug.WriteLine("Mouse_Over");
            }

            if (IsMouseOverButton() && _inputHandler.LeftButton == Utils.KeyState.Clicked)
            {
                TileButtonPressed.Invoke();
            }
        }
    }
}

