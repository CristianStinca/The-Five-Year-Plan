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

namespace TFYP.View.Windows
{
    internal sealed class SettingsWindow : Window
    {
        #region UIButtons

        Button back_btn;

        public event UIButtonPressedHandler UIToMainMenuButtonPressed;

        #endregion

        public SettingsWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            RenderableContainer mainContainer = new(0, 0, ESize.AllScreen, new Color(107, 3, 3));

            RenderableContainer container = new(0, 0, ESize.FitContent);

            RenderableList menuList = new(70, 0, 0, EHPosition.Center);
            //Text title = new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Choose save:", new Vector2(20, 30), Color.Black);
            Sprite title = new Sprite(Globals.Content.Load<Texture2D>("Menu/Settings"));
            menuList.AddElement(title);

            RenderableList menuListButtons = new(20, 0, 0);

            //slot1 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Slot1_Button")));
            //slot1.ButtonPressed += (string name) => UISavesMenuSlot1ButtonPressed.Invoke();
            //menuListButtons.AddElement(slot1);
            //slot2 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Slot2_Button")));
            //slot2.ButtonPressed += (string name) => UISavesMenuSlot2ButtonPressed.Invoke();
            //menuListButtons.AddElement(slot2);
            //slot3 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Slot3_Button")));
            //slot3.ButtonPressed += (string name) => UISavesMenuSlot3ButtonPressed.Invoke();
            //menuListButtons.AddElement(slot3);

            menuList.AddElement(menuListButtons);

            container.AddElement(EVPosition.Center, EHPosition.Center, menuList);
            mainContainer.AddElement(EVPosition.Center, EHPosition.Center, container);

            ElementsInWindow.AddRange(mainContainer.ToIRenderable());

            back_btn = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/back_Button"), new Vector2(25, 25)));
            back_btn.ButtonPressed += (string name) => UIToMainMenuButtonPressed.Invoke();

            ElementsInWindow.Add(back_btn.ToIRenderable());
        }
    }
}
