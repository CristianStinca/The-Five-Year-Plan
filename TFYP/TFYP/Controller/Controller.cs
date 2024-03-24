using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Controller.WindowsControllers;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View.UIElements;

namespace TFYP.Controller
{
    internal class Controller
    {
        public WindowController NextController { get; set; }

        public WindowController CurrentController { get; set; }

        public Controller(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
        {
            this.CurrentController = new GameWindowController(_inputHandler, _view, _uiTextures, _gameModel);
            this.NextController = this.CurrentController;
        }

        public void Update()
        {
            // reads the state of the model and instructs the view of what to do
            this.CurrentController.Update();

            // due to some events, the NextController migth be changed, therefore we will update the information
            // for the sake of moduleness, we will keep a controler for each view
            this.CurrentController = this.NextController;
        }
    }
}
