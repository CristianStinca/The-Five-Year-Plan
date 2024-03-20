using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View.UIElements;

namespace TFYP.Controller.WindowsControllers
{
    internal abstract class WindowController
    {
        public delegate void ExitHandler();
        public static event ExitHandler ExitPressed;

        protected InputHandler inputHandler;
        protected View.View view;
        protected GameModel gameModel;
        protected IUIElements uiTextures;

        public WindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
        {
            this.inputHandler = _inputHandler;
            this.view = _view;
            this.gameModel = _gameModel;
            this.uiTextures = _uiTextures;
        }

        public virtual void Update()
        {
            this.inputHandler.Update();
        }

        protected virtual void OnExitPressed()
        {
            ExitPressed?.Invoke();
        }
    }
}
