using System;
using System.Diagnostics;
using TFYP.Model;
using TFYP.Utils;
using TFYP.View.UIElements;
using TFYP.View.Windows;
//using TYFP.Persistence;

namespace TFYP.Controller.WindowsControllers
{
    internal class SavesMenuWindowController : WindowController
    {
        SavesMenuWindow _window;

        public SavesMenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
            : base(_inputHandler, _view, _uiTextures)
        {
            SyncView(_view.ChangeToSavesMenuWindow, out _window);

            _window.UIBackButtonPressed += ToLastOpenedWindow;
            _window.UISavesMenuSlot2ButtonPressed += () => { GameModel.Save(2); ToGameWindow(); };
            _window.UISavesMenuSlot1ButtonPressed += () => { GameModel.Save(1); ToGameWindow(); };
            _window.UISavesMenuSlot3ButtonPressed += () => { GameModel.Save(3); ToGameWindow(); };
            //_window.UISavesMenuSlot1ButtonPressed += () => Debug.WriteLine("Save_Save_1");
        }

        override public void SetFocus()
        {
            base.SetFocus();
            SyncView(_view.ChangeToSavesMenuWindow, out _window);
        }

        //private void SaveGame(int slot)
        //{
        //    // Assumes you have a method to get the current game model state
        //    GameModel currentGameModel = GameModel.GetInstance(); // Fetch current game model state
        //    Database.Save(currentGameModel, slot);
        //    Debug.WriteLine($"Game saved in slot {slot}.");
        //}
    }
}
