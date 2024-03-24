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

        protected InputHandler _inputHandler;
        protected View.View _view;
        protected GameModel _gameModel;
        protected IUIElements _uiTextures;

        public WindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
        {
            this._inputHandler = _inputHandler;
            this._view = _view;
            this._gameModel = _gameModel;
            this._uiTextures = _uiTextures;
        }

        public virtual void Update()
        {
            this._inputHandler.Update();
        }

        protected virtual void OnExitPressed()
        {
            ExitPressed?.Invoke();
        }
    }
}
