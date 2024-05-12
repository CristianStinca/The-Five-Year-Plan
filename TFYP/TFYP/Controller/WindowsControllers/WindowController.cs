using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View.UIElements;
using TFYP.View.Windows;

namespace TFYP.Controller.WindowsControllers
{
    internal abstract class WindowController
    {
        public delegate void ChangeWindowHandler();
        public static event ChangeWindowHandler OnChangeToGameWindow;
        public static event ChangeWindowHandler OnChangeToMenuWindow;
        public static event ChangeWindowHandler OnChangeToSettingsWindow;
        public static event ChangeWindowHandler OnChangeToLoadsWindow;
        public static event ChangeWindowHandler OnChangeToSavesWindow;
        public static event ChangeWindowHandler OnBackWindow;

        public delegate void ExitHandler();
        public static event ExitHandler ExitPressed;

        protected InputHandler _inputHandler;
        protected View.View _view;
        protected UIObjects _uiTextures;

        public WindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
        {
            this._inputHandler = _inputHandler;
            this._view = _view;
            this._uiTextures = _uiTextures as UIObjects;

            this.LoseFocus();
        }

        public virtual void SetFocus() { }

        public virtual void LoseFocus() { }

        protected void SyncView<T>(Action ChangeToViewWindowFunc, out T window) where T : Window
        {
            ChangeToViewWindowFunc.Invoke();

            if (_view.CurrentWindow.GetType().Name.CompareTo(typeof(T).Name) == 0)
            {
                window = (T)_view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException($"Conversion error ({_view.CurrentWindow.GetType().Name} => {typeof(T).Name})");
            }
        }

        public static void ToGameWindow()
        {
            OnChangeToGameWindow.Invoke();
        }
        
        public static void ToMenuWindow()
        {
            OnChangeToMenuWindow.Invoke();
        }

        public static void ToLastOpenedWindow()
        {
            OnBackWindow.Invoke();
        }

        public static void ToSettingsWindow()
        {
            OnChangeToSettingsWindow.Invoke();
        }

        public static void ToLoadsWindow()
        {
            OnChangeToLoadsWindow.Invoke();
        }

        public static void ToSavesWindow()
        {
            OnChangeToSavesWindow.Invoke();
        }

        public virtual void Update(GameTime gameTime)
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
