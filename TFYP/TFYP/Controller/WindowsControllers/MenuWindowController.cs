using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View;
using TFYP.View.UIElements;

namespace TFYP.Controller.WindowsControllers
{
    internal class MenuWindowController : WindowController
    {
        public MenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
            : base(_inputHandler, _view, _uiTextures, _gameModel)
        {
            _view.changeToMenuWindow();
        }
    }
}
