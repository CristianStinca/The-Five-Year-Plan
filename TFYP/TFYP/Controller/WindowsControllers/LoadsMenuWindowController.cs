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
    internal class LoadsMenuWindowController : WindowController
    {
        LoadsMenuWindow _window;
        public LoadsMenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
            : base(_inputHandler, _view, _uiTextures)
        {
            _view.ChangeToLoadsMenuWindow();

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.LoadsMenuWindow).Name) == 0)
            {
                _window = (View.Windows.LoadsMenuWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("LoadsWindowController (converting Window to LoadsWindow)");
            }

            _window.UIToMainMenuButtonPressed += ToMenuWindow;
            _window.UILoadMenuSlot1ButtonPressed += () => Debug.WriteLine("Load_Save_1");
            _window.UILoadMenuSlot2ButtonPressed += () => Debug.WriteLine("Load_Save_2");
            _window.UILoadMenuSlot3ButtonPressed += () => Debug.WriteLine("Load_Save_3");
        }
    }
}
