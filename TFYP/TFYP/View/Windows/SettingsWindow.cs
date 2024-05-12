using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.UIElements.ClickableElements;
using TFYP.View.UIElements;
using Microsoft.Xna.Framework;
using TFYP.View.Renders;
using MonoGame.Extended.BitmapFonts;

namespace TFYP.View.Windows
{
    internal sealed class SettingsWindow : Window
    {
        #region UIButtons

        Button back_btn;

        public event UIButtonPressedHandler UIBackButtonPressed;

        #endregion

        public SettingsWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            RenderableContainer mainContainer = new(0, 0, ESize.AllScreen, new Color(107, 3, 3));

            RenderableContainer container = new(0, 0, ESize.FitContent);

            RenderableVerticalList menuList = new(70, 0, 0, EHPosition.Center);
            //Text title = new BitmapText(Globals.Content.Load<BitmapFont>("UIButtonsText"), "Choose save:", new Vector2(20, 30), Color.Black);
            Sprite title = new Sprite(Globals.Content.Load<Texture2D>("Menu/Settings"));
            menuList.AddElement(title);

            RenderableVerticalList menuListButtons = new(20, 0, 0);

            menuListButtons.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/propaganda_60"), "[AWSD] - Movement", Vector2.Zero, new Color(222, 219, 0)));
            menuListButtons.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/propaganda_60"), "[R] - Rotate", Vector2.Zero, new Color(222, 219, 0)));
            menuListButtons.AddElement(new BitmapText(Globals.Content.Load<BitmapFont>("Fonts/propaganda_60"), "[Esc] - Menu", Vector2.Zero, new Color(222, 219, 0)));

            menuList.AddElement(menuListButtons);

            container.AddElement(EVPosition.Center, EHPosition.Center, menuList);
            mainContainer.AddElement(EVPosition.Center, EHPosition.Center, container);

            ElementsInWindow.AddRange(mainContainer.ToIRenderable());

            back_btn = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/back_Button"), new Vector2(25, 25)));
            back_btn.ButtonPressed += (string name) => UIBackButtonPressed.Invoke();

            ElementsInWindow.Add(back_btn.ToIRenderable());
        }
    }
}
