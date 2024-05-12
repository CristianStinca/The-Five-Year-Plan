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

        private GameWindow gameWindow { get; set; }
        private MenuWindow menuWindow { get; set; }
        private SavesMenuWindow savesMenuWindow { get; set; }
        private LoadsMenuWindow loadsMenuWindow { get; set; }
        private SettingsWindow settingsWindow { get; set; }

        public View(IUIElements UIElements, InputHandler inputHandler) 
        {
            this._UIElements = UIElements;
            this._inputHandler = inputHandler;
            
            gameWindow = new GameWindow(_UIElements, _inputHandler);
            menuWindow = new MenuWindow(_UIElements, _inputHandler);
            savesMenuWindow = new SavesMenuWindow(_UIElements, _inputHandler);
            loadsMenuWindow = new LoadsMenuWindow(_UIElements, _inputHandler);
            settingsWindow = new SettingsWindow(_UIElements, _inputHandler);
        }

        public void ChangeToGameWindow()
        {
            this.CurrentWindow = gameWindow;
        }

        public void ChangeToMenuWindow()
        {
            this.CurrentWindow = menuWindow;
        }
        
        public void ChangeToSavesMenuWindow()
        {
            this.CurrentWindow = savesMenuWindow;
        }
        
        public void ChangeToLoadsMenuWindow()
        {
            this.CurrentWindow = loadsMenuWindow;
        }

        public void ChangeToSettingsWindow()
        {
            this.CurrentWindow = settingsWindow;
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
