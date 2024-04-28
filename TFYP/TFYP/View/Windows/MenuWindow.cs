using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using TFYP.View.UIElements.ClickableElements;

namespace TFYP.View.Windows
{
    internal sealed class MenuWindow : Window
    {
        #region UIButtons

        Button new_btn;
        Button load_btn;
        Button settings_btn;

        public event UIButtonPressedHandler UIMenuNewGameButtonPressed;
        public event UIButtonPressedHandler UIMenuLoadGameButtonPressed;
        public event UIButtonPressedHandler UIMenuOpenSettingsButtonPressed;

        #endregion

        public MenuWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            RenderableContainer mainContainer = new(0, 0, ESize.AllScreen, new Color(107, 3, 3));

            RenderableContainer container = new(0, 0, ESize.FitContent);

            RenderableVerticalList menuList = new(70, 0, 0, EHPosition.Center);
            Sprite title_btn = new Sprite(Globals.Content.Load<Texture2D>("Menu/Title"));
            menuList.AddElement(title_btn);

            RenderableVerticalList menuListButtons = new(20, 0, 0);

            new_btn = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/NewGame_Button")));
            new_btn.ButtonPressed += (string name) => UIMenuNewGameButtonPressed.Invoke();
            menuListButtons.AddElement(new_btn);
            load_btn = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/LoadGame_Button")));
            load_btn.ButtonPressed += (string name) => UIMenuLoadGameButtonPressed.Invoke();
            menuListButtons.AddElement(load_btn);
            settings_btn = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Settings_Button")));
            settings_btn.ButtonPressed += (string name) => UIMenuOpenSettingsButtonPressed.Invoke();
            menuListButtons.AddElement(settings_btn);

            menuList.AddElement(menuListButtons);

            container.AddElement(EVPosition.Center, EHPosition.Center, menuList);
            mainContainer.AddElement(EVPosition.Center, EHPosition.Center, container);

            ElementsInWindow.AddRange(mainContainer.ToIRenderable());
        }
    }
}
