using System;
using System.Diagnostics;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View.UIElements;
using TFYP.View.Windows;
using TYFP.Persistence;

namespace TFYP.Controller.WindowsControllers
{
    internal class SavesMenuWindowController : WindowController
    {
        SavesMenuWindow _window;

        public SavesMenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures)
            : base(_inputHandler, _view, _uiTextures)
        {
            _view.ChangeToSavesMenuWindow();

            if (base._view.CurrentWindow.GetType().Name.CompareTo(typeof(View.Windows.SavesMenuWindow).Name) == 0)
            {
                _window = (View.Windows.SavesMenuWindow)base._view.CurrentWindow;
            }
            else
            {
                throw new TypeLoadException("SavesMenuWindowController (converting Window to SavesMenuWindow)");
            }

            _window.UIToMainMenuButtonPressed += ToMenuWindow;
            _window.UISavesMenuSlot1ButtonPressed += () => GameModel.Save(1);
            _window.UISavesMenuSlot2ButtonPressed += () => GameModel.Save(2);
            _window.UISavesMenuSlot3ButtonPressed += () => GameModel.Save(3);
            //_window.UISavesMenuSlot1ButtonPressed += () => Debug.WriteLine("Save_Save_1");
        }

        private void SaveGame(int slot)
        {
            // Assumes you have a method to get the current game model state
            GameModel currentGameModel = GameModel.GetInstance(); // Fetch current game model state
            Database.Save(currentGameModel, slot);
            Debug.WriteLine($"Game saved in slot {slot}.");
        }
    }
}
