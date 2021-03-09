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

        private Texture2D _borderBlock;

        private Texture2D _foodTexture;
        private Vector2 _foodPosition;

        private SpriteFont _font;
        private int _score;

        private bool _gameOver;
        private Texture2D _gameOverScreen;

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
            _screenSize = new Vector2(780, 440);

            _graphics.PreferredBackBufferWidth = (int)_screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)_screenSize.Y;
            _graphics.ApplyChanges();

            _topOffset = 4;

            _blockSize = 20;
            _grid = new Vector2((int)(_screenSize.X / _blockSize), (int)(_screenSize.Y / _blockSize));

            _snake = new Snake(new Vector2((int)(_grid.X / 2) * _blockSize, (int)(_grid.Y / 2) * _blockSize));
            _snake.Initialize();

            _score = 0;

            _gameOver = false;
            _gameOverScreen = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _gameOverScreen.SetData(new[] { new Color(0, 0, 0, 200) });

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _snake.LoadContent(Content);

            _borderBlock = Content.Load<Texture2D>("Border Block/Border Block");

            _foodTexture = Content.Load<Texture2D>("Food/Food");

            _font = Content.Load<SpriteFont>("Font/Manaspace Regular");

            SetRandomFoodPosition();
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();
            
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (_gameOver) {
                if (state.IsKeyDown(Keys.Space))
                    ResetGame();
                
                return;
            }

            _snake.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (IsPositionOnBorder(_snake.HeadPosition))
                _gameOver = true;

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

            for (int i = 0; i < _grid.X; i++) {
                _spriteBatch.Draw(_borderBlock, new Vector2(i * _blockSize, _topOffset * _blockSize), Color.White);
                _spriteBatch.Draw(_borderBlock, new Vector2(i * _blockSize, (_grid.Y - 1) * _blockSize), Color.White);
            }

            for (int i = _topOffset; i < _grid.Y - 1; i++) {
                _spriteBatch.Draw(_borderBlock, new Vector2(0, i * _blockSize), Color.White);
                _spriteBatch.Draw(_borderBlock, new Vector2((_grid.X - 1) * _blockSize, i * _blockSize), Color.White);
            }

            _spriteBatch.DrawString(_font, "Score: " + _score.ToString(), new Vector2(20, 30), Color.White);

            _spriteBatch.Draw(_foodTexture, _foodPosition, Color.White);
            _snake.Draw(_spriteBatch);

            if (_gameOver) {
                _spriteBatch.Draw(_gameOverScreen, new Rectangle(0, 0, (int)_screenSize.X, (int)_screenSize.Y), Color.White);
                _spriteBatch.DrawString(_font, "        Game Over!\nPress Space to play again.", new Vector2((_screenSize.X / 2) - 250, _screenSize.Y / 2), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetRandomFoodPosition() {
            Random random = new Random();

            _foodPosition = new Vector2(random.Next(0, (int)_grid.X) * _blockSize, random.Next(_topOffset, (int)_grid.Y) * _blockSize);
    
            while (_snake.IsPositionInBody(_foodPosition) || IsPositionOnBorder(_foodPosition))
                _foodPosition = new Vector2(random.Next(0, (int)_grid.X) * _blockSize, random.Next(_topOffset, (int)_grid.Y) * _blockSize);
        }

        private bool IsPositionOnBorder(Vector2 position) {
            return position.X < _blockSize || 
                   position.X >= ((_grid.X - 1) * _blockSize) || 
                   position.Y <= _topOffset * _blockSize || 
                   position.Y >= ((_grid.Y - 1) * _blockSize);
        }

        private void Snake_GameOver() {
            _gameOver = true;
        }

        private void ResetGame() {
            _gameOver = false;

            _snake = new Snake(new Vector2((int)(_grid.X / 2) * _blockSize, (int)(_grid.Y / 2) * _blockSize));
            _snake.Initialize();
            _snake.LoadContent(Content);

            _score = 0;

            SetRandomFoodPosition();
        }
    }
}
