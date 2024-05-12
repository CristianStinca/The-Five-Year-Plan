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
//using TYFP.Persistence;

namespace TFYP.Controller.WindowsControllers
{
    internal class LoadsMenuWindowController : WindowController
    {
        LoadsMenuWindow _window;
        public LoadsMenuWindowController(InputHandler _inputHandler, View.View _view, IUIElements _uiTextures, GameModel _gameModel)
            : base(_inputHandler, _view, _uiTextures)
        {
            SyncView(_view.ChangeToLoadsMenuWindow, out _window);

            _window.UIBackButtonPressed += ToLastOpenedWindow;
            _window.UILoadMenuSlot1ButtonPressed += () => { GameModel.Read(1); ToGameWindow(); };
            _window.UILoadMenuSlot2ButtonPressed += () => { GameModel.Read(2); ToGameWindow(); };
            _window.UILoadMenuSlot3ButtonPressed += () => { GameModel.Read(3); ToGameWindow(); };
        }

        override public void SetFocus()
        {
            base.SetFocus();
            SyncView(_view.ChangeToLoadsMenuWindow, out _window);
        }

        //private void LoadGame(int slot)
        //{
        //    var gameModel = Database.Read(slot);
        //    if (gameModel != null)
        //    {
        //        GameModel.GetInstance().ChangeGameModel(gameModel);  // Update the current game state with the loaded model
        //        Debug.WriteLine($"Game loaded successfully from slot {slot}.");
        //    }
        //    else
        //    {
        //        Debug.WriteLine($"Failed to load game from slot {slot}.");
        //    }
        //}
    }
}
