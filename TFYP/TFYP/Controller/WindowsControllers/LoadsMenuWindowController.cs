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
using TYFP.Persistence;

namespace TFYP.Controller.WindowsControllers
{
    internal class LoadsMenuWindowController : WindowController
    {
        LoadsMenuWindow _window;
        public LoadsMenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
            : base(_inputHandler, _view, _uiTextures)
        {
            _view.ChangeToLoadsMenuWindow();

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.LoadsMenuWindow).Name) == 0)
            {
                _window = (View.Windows.LoadsMenuWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("LoadsWindowController (converting Window to LoadsWindow)");
            }

            _window.UIToMainMenuButtonPressed += ToMenuWindow;
            _window.UILoadMenuSlot1ButtonPressed += () => GameModel.Read(1);
            _window.UILoadMenuSlot2ButtonPressed += () => GameModel.Read(2);
            _window.UILoadMenuSlot3ButtonPressed += () => GameModel.Read(3);
        }
        private void LoadGame(int slot)
        {
            var gameModel = Database.Read(slot);
            if (gameModel != null)
            {
                GameModel.GetInstance().ChangeGameModel(gameModel);  // Update the current game state with the loaded model
                Debug.WriteLine($"Game loaded successfully from slot {slot}.");
            }
            else
            {
                Debug.WriteLine($"Failed to load game from slot {slot}.");
            }
        }
    }
}
