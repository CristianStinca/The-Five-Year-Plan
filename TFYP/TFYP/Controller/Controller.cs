using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public WindowController LastController { get; set; }

        GameWindowController gameWindowController { get; set; }
        MenuWindowController menuWindowController { get; set; }
        SettingsWindowController settingsWindowController { get; set; }
        SavesMenuWindowController savesMenuWindowController { get; set; }
        LoadsMenuWindowController loadsMenuWindowController { get; set; }

        public Controller(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
        {
            gameWindowController = new GameWindowController(_inputHandler, _view, _uiTextures, GameModel.GetInstance());
            menuWindowController = new MenuWindowController(_inputHandler, _view, _uiTextures);
            settingsWindowController = new SettingsWindowController(_inputHandler, _view, _uiTextures);
            savesMenuWindowController = new SavesMenuWindowController(_inputHandler, _view, _uiTextures, GameModel.GetInstance());
            loadsMenuWindowController = new LoadsMenuWindowController(_inputHandler, _view, _uiTextures, GameModel.GetInstance());

            this.CurrentController = menuWindowController;
            CurrentController.SetFocus();
            this.NextController = this.CurrentController;

            WindowController.OnChangeToGameWindow += () => SwitchWindowControllers(gameWindowController);
            WindowController.OnChangeToMenuWindow += () => SwitchWindowControllers(menuWindowController);
            WindowController.OnChangeToSettingsWindow += () => SwitchWindowControllers(settingsWindowController);
            WindowController.OnChangeToSavesWindow += () => SwitchWindowControllers(savesMenuWindowController);
            WindowController.OnChangeToLoadsWindow += () => SwitchWindowControllers(loadsMenuWindowController);
            WindowController.OnBackWindow += () => SwitchWindowControllers(this.LastController);
        }

        private void SwitchWindowControllers<T>(T windowController) where T : WindowController
        {
            this.NextController = windowController;
            this.CurrentController.LoseFocus();
            this.NextController.SetFocus();
        }

        public void Update(GameTime gameTime)
        {
            // reads the state of the model and instructs the view of what to do
            this.CurrentController.Update(gameTime);

            // due to some events, the NextController migth be changed, therefore we will update the information
            // for the sake of moduleness, we will keep a controler for each view
            if (this.CurrentController.GetType() != this.NextController.GetType())
                this.LastController = this.CurrentController;

            this.CurrentController = this.NextController;
        }
    }
}
