using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public delegate void ChangeWindowHandler();
        public static event ChangeWindowHandler OnChangeToGameWindow;
        public static event ChangeWindowHandler OnChangeToMenuWindow;
        public static event ChangeWindowHandler OnChangeToSettingsWindow;
        public static event ChangeWindowHandler OnChangeToLoadsWindow;

        public delegate void ExitHandler();
        public static event ExitHandler ExitPressed;

        protected InputHandler _inputHandler;
        protected View.View _view;
        protected IUIElements _uiTextures;

        public WindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
        {
            this._inputHandler = _inputHandler;
            this._view = _view;
            this._uiTextures = _uiTextures;
        }

        public static void ToGameWindow()
        {
            OnChangeToGameWindow.Invoke();
        }
        
        public static void ToMenuWindow()
        {
            OnChangeToMenuWindow.Invoke();
        }

        public static void ToSettingsWindow()
        {
            OnChangeToSettingsWindow.Invoke();
        }

        public static void ToLoadsWindow()
        {
            OnChangeToLoadsWindow.Invoke();
        }

        public virtual void Update()
        {
            this._inputHandler.Update();
        }

        /// <summary>
        /// Raises the event ExitPressed.
        /// </summary>
        protected virtual void OnExitPressed()
        {
            ExitPressed?.Invoke();
        }
    }
}
