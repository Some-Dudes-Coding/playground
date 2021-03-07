using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake {
    public class SnakeGame : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Vector2 _screenSize;
        private Vector2 _grid;
        private int _topOffset;

        private Snake _snake;
        private int _blockSize;

        private Texture2D _foodTexture;
        private Vector2 _foodPosition;

        private SpriteFont _font;
        private int _score;

        public SnakeGame() {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Snake.OnBitBody += Snake_GameOver;
        }

        ~SnakeGame() {
            Snake.OnBitBody -= Snake_GameOver;
        }

        protected override void Initialize() {
            _screenSize = new Vector2(768, 432);

            _graphics.PreferredBackBufferWidth = (int)_screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)_screenSize.Y;
            _graphics.ApplyChanges();

            _topOffset = 4;

            _blockSize = 20;
            _grid = new Vector2((int)(_screenSize.X / _blockSize), (int)(_screenSize.Y / _blockSize));

            _snake = new Snake(new Vector2((int)(_grid.X / 2) * _blockSize, (int)(_grid.Y / 2) * _blockSize));
            _snake.Initialize();

            _score = 0;

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _snake.LoadContent(Content);
            _foodTexture = Content.Load<Texture2D>("Food/Food");

            _font = Content.Load<SpriteFont>("Font/Manaspace Regular");

            SetRandomFoodPosition();
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _snake.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (_snake.IsPositionInBody(_foodPosition)) {
                _score++;

                _snake.AddTail();
                SetRandomFoodPosition();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_font, _score.ToString(), new Vector2(20, 20), Color.White);

            _spriteBatch.Draw(_foodTexture, _foodPosition, Color.White);
            _snake.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetRandomFoodPosition() {
            Random random = new Random();

            _foodPosition = new Vector2(random.Next(0, (int)_grid.X) * _blockSize, random.Next(_topOffset, (int)_grid.Y) * _blockSize);
    
            while (_snake.IsPositionInBody(_foodPosition))
                _foodPosition = new Vector2(random.Next(0, (int)_grid.X) * _blockSize, random.Next(_topOffset, (int)_grid.Y) * _blockSize);
        }

        private void Snake_GameOver() {
            System.Diagnostics.Debug.WriteLine("LOL"); // TODO: Handle game over logic and reset game
        }
    }
}
