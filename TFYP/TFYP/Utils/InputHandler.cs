namespace TFYP.Utils
{
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    public class InputHandler
    {
        public InputHandler()
        {
            ActiveKeys = new List<KeyboardButtonState>();
        }

        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

        public List<KeyboardButtonState> ActiveKeys { get; set; }

        public Keys KeyToCheck { get; set; }
        
        public MouseState CurrentMouseState { get; set; }
        public MouseState PreviousMouseState { get; set; }

        public KeyState LeftButton { get; set; }
        public KeyState RightButton { get; set; }
        public KeyState MiddleButton { get; set; }

        public void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            CheckKey();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
            CheckMouseButton();
        }

        public void CheckKey()
        {
            for (int i = 0; i < CurrentKeyboardState.GetPressedKeys().Length; i++)
            {
                KeyToCheck = CurrentKeyboardState.GetPressedKeys()[i];

                if (PreviousKeyboardState.IsKeyUp(KeyToCheck) &&
                    CurrentKeyboardState.IsKeyDown(KeyToCheck))
                {
                    ActiveKeys.Add(new KeyboardButtonState(KeyToCheck));
                }
                else if (PreviousKeyboardState.IsKeyDown(KeyToCheck) &&
                    CurrentKeyboardState.IsKeyDown(KeyToCheck))
                {
                    foreach (KeyboardButtonState key in ActiveKeys)
                    {
                        if (key.Button == KeyToCheck)
                        {
                            key.ButtonState = KeyState.Held;
                        }
                    }
                }
                else if (PreviousKeyboardState.IsKeyDown(KeyToCheck) &&
                    CurrentKeyboardState.IsKeyUp(KeyToCheck))
                {
                    foreach (KeyboardButtonState key in ActiveKeys)
                    {
                        if (key.Button == KeyToCheck)
                        {
                            key.ButtonState = KeyState.Released;
                        }
                    }
                }
            }

            for (int i = 0; i < ActiveKeys.Count; i++)
            {
                if (PreviousKeyboardState.IsKeyUp(ActiveKeys[i].Button) &&
                    CurrentKeyboardState.IsKeyUp(ActiveKeys[i].Button))
                {
                    ActiveKeys[i].Button = Keys.None;
                    ActiveKeys[i].ButtonState = KeyState.None;
                }
            }

            while (ActiveKeys.Contains(new KeyboardButtonState(Keys.None)))
            {
                ActiveKeys.Remove(new KeyboardButtonState(Keys.None));
            }
        }

        public void CheckMouseButton()
        {
            LeftButton = SetMouseButtonState(PreviousMouseState.LeftButton, CurrentMouseState.LeftButton);
            RightButton = SetMouseButtonState(PreviousMouseState.RightButton, CurrentMouseState.RightButton);
            MiddleButton = SetMouseButtonState(PreviousMouseState.MiddleButton, CurrentMouseState.MiddleButton);
        }

        private KeyState SetMouseButtonState(ButtonState prev_state, ButtonState curr_state)
        {
            if (prev_state == ButtonState.Released && curr_state == ButtonState.Pressed)
            {
                return KeyState.Clicked;
            }
            else if (prev_state == ButtonState.Pressed && curr_state == ButtonState.Pressed)
            {
                return KeyState.Held;
            }
            else if (prev_state == ButtonState.Pressed && curr_state == ButtonState.Released)
            {
                return KeyState.Released;
            }

            return KeyState.None ;
        }
    }
}
