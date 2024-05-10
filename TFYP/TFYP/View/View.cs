using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Controller.WindowsControllers;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using TFYP.View.Windows;

namespace TFYP.View
{
    internal class View
    {
        public Window CurrentWindow { get; set; }

        protected IUIElements _UIElements;

        protected InputHandler _inputHandler;

        public View(IUIElements UIElements, InputHandler inputHandler) 
        {
            this._UIElements = UIElements;
            this._inputHandler = inputHandler;
        }

        /// <summary>
        /// Changes the current window to GameWindow.
        /// </summary>
        public void ChangeToGameWindow()
        {
            this.CurrentWindow = new GameWindow(_UIElements, _inputHandler);
        }

        /// <summary>
        /// Changes the current window to MenuWindow.
        /// </summary>
        public void ChangeToMenuWindow()
        {
            this.CurrentWindow = new MenuWindow(_UIElements, _inputHandler);
        }
        
        public void ChangeToSavesMenuWindow()
        {
            this.CurrentWindow = new SavesMenuWindow(_UIElements, _inputHandler);
        }
        
        public void ChangeToLoadsMenuWindow()
        {
            this.CurrentWindow = new LoadsMenuWindow(_UIElements, _inputHandler);
        }

        public void ChangeToSettingsWindow()
        {
            this.CurrentWindow = new SettingsWindow(_UIElements, _inputHandler);
        }

        public void Update()
        {
            this.CurrentWindow.Update();
        }

        public void Draw(IRenderer renderer)
        {
            this.CurrentWindow.Draw(renderer);
        }
    }
}
