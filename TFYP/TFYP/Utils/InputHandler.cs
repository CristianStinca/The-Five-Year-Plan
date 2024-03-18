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

        public void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            CheckKey();
        }

        public void CheckKey()
        {
            for (int i = 0; i < CurrentKeyboardState.GetPressedKeys().Length; i++)
            {
                KeyToCheck = CurrentKeyboardState.GetPressedKeys()[i];
                // Proverqvame minaliq state za daden buton
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
    }
}
