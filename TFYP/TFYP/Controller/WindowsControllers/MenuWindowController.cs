using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View;
using TFYP.View.UIElements;
using TFYP.View.Windows;
using static TFYP.View.Windows.Window;

namespace TFYP.Controller.WindowsControllers
{
    internal class MenuWindowController : WindowController
    {


        MenuWindow _window;

        public MenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
            : base(_inputHandler, _view, _uiTextures)
        {
            _view.ChangeToMenuWindow();

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.MenuWindow).Name) == 0)
            {
                _window = (View.Windows.MenuWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("GameWindowController (set_map)");
            }

            _window.UIMenuNewGameButtonPressed += ToGameWindow;
            _window.UIMenuLoadGameButtonPressed += ToLoadsWindow;
            _window.UIMenuOpenSettingsButtonPressed += ToSettingsWindow;
        }
    }
}
