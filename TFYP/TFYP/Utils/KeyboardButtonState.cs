namespace TFYP.Utils
{
    using Microsoft.Xna.Framework.Input;

    public enum KeyState
    {
        Held,
        Clicked,
        Released,
        None
    }

    public class KeyboardButtonState
    {
        public Keys Button { get; set; }
        public KeyState ButtonState { get; set; }

        public KeyboardButtonState(Keys button)
        {
            Button = button;
            ButtonState = KeyState.Clicked;
        }
    }
}
