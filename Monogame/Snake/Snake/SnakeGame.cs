using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake {
    public class SnakeGame : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Vector2 _screenSize;

        private Snake _snake;

        private Vector2 _foodPosition;
        private Texture2D _foodTexture;

        public SnakeGame() {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _screenSize = new Vector2(1024, 576);

            _graphics.PreferredBackBufferWidth = (int)_screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)_screenSize.Y;
            _graphics.ApplyChanges();

            _snake = new Snake(new Vector2(_screenSize.X / 2, _screenSize.Y / 2));
            _snake.Initialize();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _snake.LoadContent(Content);
            _foodTexture = Content.Load<Texture2D>("Food/Food");

            SetRandomFoodPosition();
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _snake.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (_snake.IsPositionInBody(_foodPosition))
                SetRandomFoodPosition();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_foodTexture, _foodPosition, Color.White);
            _snake.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetRandomFoodPosition() {
            Random random = new Random();

            _foodPosition = new Vector2(random.Next(0, (int)_screenSize.X), random.Next(0, (int)_screenSize.Y));
            while (_snake.IsPositionInBody(_foodPosition))
                _foodPosition = new Vector2(random.Next(0, (int)_screenSize.X), random.Next(0, (int)_screenSize.Y));
        }
    }
}
