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
            SyncView(_view.ChangeToSettingsWindow, out _window);

            _window.UIBackButtonPressed += ToLastOpenedWindow;
        }

        override public void SetFocus()
        {
            base.SetFocus();
            SyncView(_view.ChangeToSettingsWindow, out _window);
        }
    }
}
