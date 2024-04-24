using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using TFYP.Utils;
using TFYP.View;
using TFYP.View.Renders;
using TFYP.View.Windows;
using TFYP.Controller.WindowsControllers;
using TFYP.Model;
using TFYP.View.UIElements;

namespace TFYP.Controller
{
    public class TFYP : Game
    {
        private View.View _view;
        private GameModel _gameModel;
        private MonoGameRenderer _renderer;
        private InputHandler _inputHandler;
        private Controller _controller;
        private IUIElements _uiTextures;

        public TFYP()
        {
            Globals.Graphics = new GraphicsDeviceManager(this);
            Globals.Content = this.Content;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //Globals.Graphics.IsFullScreen = true;

            Window.Title = "The Five Year Plan";

            Globals.Graphics.PreferredBackBufferWidth = 1280;
            Globals.Graphics.PreferredBackBufferHeight = 720;

            WindowController.ExitPressed += this.QuitGame;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // This is where you can query any required services and load any non - graphic related content.

            _renderer = new MonoGameRenderer();
            _inputHandler = new InputHandler();

            _uiTextures = new UIObjects();
            _view = new View.View(_uiTextures, _inputHandler);
            //_gameModel = GameModel.GetInstance();//initialized map with size here (Kristi)
            _controller = new Controller(_inputHandler, _view, _uiTextures);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            // The Update method is called multiple times per second, and it is used to update your game state
            // (checking for collisions, gathering input, playing audio, etc.).

            // Updates the logic of the controller (mainly key presses and instructions of what to draw)
            this._controller.Update(gameTime);

            // Updates the internal state of the view
            this._view.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Similar to the Update method, the Draw method is also called multiple times per second.
            // This, as the name suggests, is responsible for drawing content to the screen.

            // A call to the view as instructed by the MVC architecture
            this._view.Draw(_renderer);

            base.Draw(gameTime);
        }

        private void QuitGame()
        {
            this.Exit();
        }
    }
}
