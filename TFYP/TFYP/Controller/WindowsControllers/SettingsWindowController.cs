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
    internal class SettingsWindowController : WindowController
    {
        SettingsWindow _window;
        public SettingsWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
            : base(_inputHandler, _view, _uiTextures)
        {
            _view.ChangeToSettingsWindow();

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.SettingsWindow).Name) == 0)
            {
                _window = (View.Windows.SettingsWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("SettingsWindowController (converting Window to SettingsWindow)");
            }

            _window.UIToMainMenuButtonPressed += ToMenuWindow;
        }
    }
}
