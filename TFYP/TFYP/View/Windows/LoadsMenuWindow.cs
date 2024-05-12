using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using static TFYP.View.Windows.Window;
using TFYP.View.UIElements.ClickableElements;

namespace TFYP.View.Windows
{
    internal sealed class LoadsMenuWindow : Window
    {
        #region UIButtons

        Button slot1;
        Button slot2;
        Button slot3;
        Button back_btn;

        public event UIButtonPressedHandler UILoadMenuSlot1ButtonPressed;
        public event UIButtonPressedHandler UILoadMenuSlot2ButtonPressed;
        public event UIButtonPressedHandler UILoadMenuSlot3ButtonPressed;
        public event UIButtonPressedHandler UIBackButtonPressed;

        #endregion

        public LoadsMenuWindow(IUIElements UIElements, InputHandler inputHandler) : base(UIElements, inputHandler)
        {
            RenderableContainer mainContainer = new(0, 0, ESize.AllScreen, new Color(107, 3, 3));

            RenderableContainer container = new(0, 0, ESize.FitContent);

            RenderableVerticalList menuList = new(70, 0, 0, EHPosition.Center);
            //Text title = new Text(Globals.Content.Load<SpriteFont>("UIButtonsText"), "Choose save:", new Vector2(20, 30), Color.Black);
            Sprite title = new Sprite(Globals.Content.Load<Texture2D>("Menu/ChooseSave"));
            menuList.AddElement(title);

            RenderableVerticalList menuListButtons = new(20, 0, 0);

            slot1 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Slot1_Button")));
            slot1.ButtonPressed += (string name) => UILoadMenuSlot1ButtonPressed.Invoke();
            menuListButtons.AddElement(slot1);
            slot2 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Slot2_Button")));
            slot2.ButtonPressed += (string name) => UILoadMenuSlot2ButtonPressed.Invoke();
            menuListButtons.AddElement(slot2);
            slot3 = AddButton(new Sprite(Globals.Content.Load<Texture2D>("Menu/Slot3_Button")));
            slot3.ButtonPressed += (string name) => UILoadMenuSlot3ButtonPressed.Invoke();
            menuListButtons.AddElement(slot3);

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
