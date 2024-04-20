using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.UIElements;
using TFYP.View.Windows;

namespace TFYP.Controller.WindowsControllers
{
    internal class SavesMenuWindowController : WindowController
    {
        SavesMenuWindow _window;
        public SavesMenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
            : base(_inputHandler, _view, _uiTextures)
        {
            _view.ChangeToSavesMenuWindow();

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.SavesMenuWindow).Name) == 0)
            {
                _window = (View.Windows.SavesMenuWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("SavesMenuWindowController (converting Window to SavesMenuWindow)");
            }

            _window.UIToMainMenuButtonPressed += ToMenuWindow;
            _window.UISavesMenuSlot1ButtonPressed += () => Debug.WriteLine("Load_Save_1");
            _window.UISavesMenuSlot2ButtonPressed += () => Debug.WriteLine("Load_Save_2");
            _window.UISavesMenuSlot3ButtonPressed += () => Debug.WriteLine("Load_Save_3");
        }
    }
}
