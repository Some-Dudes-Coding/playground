using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris {
    public class Tetris : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Vector2 screenSize;

        private List<GameObject> gameObjects;

        private Field field;
        private Vector2 fieldPosition;

        private Vector2 tetrominoPosition;
        private Tetromino tetromino;
        private Vector2 nextTetrominoPosition;
        private Tetromino nextTetromino;
        private Vector2 nextFontPosition;

        private int inputDelay;
        private float inputTick;
        private float timeSinceLastInputTick;

        private float forceDownTick;
        private float timeSinceLastForceDownTick;
        private float maxForceDownSpeed;

        private SpriteFont font;

        private int score;
        private int lockScore;
        private int lineScore;
        private Vector2 scorePosition;

        private float difficultyIncreaseStep;
        private float lockedPiecesToIncreaseDifficulty;
        private int lockedPieces;

        private bool gameOver;
        private Texture2D gameOverScreen;

        public Tetris() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            Field.OnLinesCompleted += this.Field_AddLinesCompletedScore;
        }

        ~Tetris() {
            Field.OnLinesCompleted -= this.Field_AddLinesCompletedScore;
        }

        protected override void Initialize() {
            this.screenSize = new Vector2(480, 410);
            
            this.graphics.PreferredBackBufferWidth = (int)this.screenSize.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenSize.Y;
            this.graphics.ApplyChanges();

            this.inputDelay = 50;
            this.inputTick = (float)this.inputDelay / 1000;
            this.timeSinceLastInputTick = 0;

            this.forceDownTick = 1;
            this.timeSinceLastForceDownTick = 0;
            this.maxForceDownSpeed = 0.05f;

            this.fieldPosition = new Vector2(10, 10);
            this.field = new Field(this.fieldPosition);

            this.tetrominoPosition = new Vector2(85, 10);
            this.tetromino = new Tetromino(this.tetrominoPosition);

            this.nextTetrominoPosition = new Vector2(260, 165);
            this.nextTetromino = new Tetromino(this.nextTetrominoPosition);
            this.nextFontPosition= new Vector2(240, 130);

            this.score = 0;
            this.scorePosition = new Vector2(240, 40);

            this.lockScore = 50;
            this.lineScore = 100;

            this.difficultyIncreaseStep = 0.05f;
            this.lockedPieces = 0;
            this.lockedPiecesToIncreaseDifficulty = 30;

            this.gameOver = false;
            this.gameOverScreen = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            this.gameOverScreen.SetData(new[] { new Color(0, 0, 0, 200) });

            this.gameObjects = new List<GameObject>() {
                this.field,
                this.tetromino,
                this.nextTetromino
            };

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Initialize(); });

            base.Initialize();
        }

        protected override void LoadContent() {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.font = this.Content.Load<SpriteFont>("Font/Manaspace Regular");

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.LoadContent(this.Content); });
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (this.gameOver) {
                if (state.IsKeyDown(Keys.Space))
                    this.ResetGame();

                return;
            }

            this.timeSinceLastInputTick += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.timeSinceLastInputTick >= this.inputTick) {
                this.timeSinceLastInputTick = 0;

                if (state.IsKeyDown(Keys.Down)) {
                    if (this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.rotation, this.tetromino.GetDownMovementStep()))
                        this.tetromino.MoveDown();
                    else
                        this.LockTetromino();
                }

                if (state.IsKeyDown(Keys.Left) && this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.rotation, this.tetromino.GetLeftMovementStep()))
                    this.tetromino.MoveLeft();

                if (state.IsKeyDown(Keys.Right) && this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.rotation, this.tetromino.GetRightMovementStep()))
                    this.tetromino.MoveRight();

                if (state.IsKeyDown(Keys.Z) && this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.GetRotatedStep(), this.tetromino.position))
                    this.tetromino.Rotate();
            }

            this.timeSinceLastForceDownTick += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.timeSinceLastForceDownTick >= this.forceDownTick) {
                this.timeSinceLastForceDownTick = 0;

                if (this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.rotation, this.tetromino.GetDownMovementStep()))
                    this.tetromino.MoveDown();
                else
                    this.LockTetromino();
            }

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Update((float)gameTime.ElapsedGameTime.TotalSeconds); });

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();

            this.spriteBatch.DrawString(this.font, "Next: ", this.nextFontPosition, Color.White);
            this.spriteBatch.DrawString(this.font, "Score: " + this.score.ToString(), this.scorePosition, Color.White);

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Draw(this.spriteBatch); });

            if (this.gameOver) {
                this.spriteBatch.Draw(this.gameOverScreen, new Rectangle(0, 0, (int)screenSize.X, (int)this.screenSize.Y), Color.White);
                this.spriteBatch.DrawString(this.font, "       Game Over!\nPress Space to play again.", new Vector2(35, 180), Color.White);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LockTetromino() {
            this.score += this.lockScore;

            this.lockedPieces++;
            if (this.lockedPieces % this.lockedPiecesToIncreaseDifficulty == 0) {
                this.lockedPieces = 0;

                if (this.forceDownTick - this.difficultyIncreaseStep >= this.maxForceDownSpeed)
                    this.forceDownTick -= this.difficultyIncreaseStep;
            }

            this.field.LockTetromino(this.tetromino);
            this.field.HandleLineCompletion(this.tetromino.position);

            if (!this.field.DoesTetrominoFit(this.nextTetromino.layout, 0, this.tetrominoPosition)) {
                this.gameOver = true;
                return;
            }

            this.gameObjects.Remove(this.tetromino);
            this.tetromino = this.nextTetromino;
            this.tetromino.position = this.tetrominoPosition;
            this.gameObjects.Add(this.tetromino);

            this.gameObjects.Remove(this.nextTetromino);
            this.nextTetromino = new Tetromino(this.nextTetrominoPosition);
            this.gameObjects.Add(this.nextTetromino);

            this.nextTetromino.Initialize();
            this.nextTetromino.LoadContent(this.Content);
        }

        private void ResetGame() {
            this.gameOver = false;

            this.gameObjects.Clear();
            
            this.score = 0;

            this.field = new Field(this.fieldPosition);
            this.tetromino = new Tetromino(this.tetrominoPosition);
            this.nextTetromino = new Tetromino(this.nextTetrominoPosition);

            this.gameObjects = new List<GameObject>() {
                this.field,
                this.tetromino,
                this.nextTetromino
            };

            this.gameObjects.ForEach(delegate (GameObject gameObject) {
                gameObject.Initialize(); 
                gameObject.LoadContent(this.Content); 
            });
        }

        private void Field_AddLinesCompletedScore(int lines) {
            this.score += (1 << lines) * this.lineScore;
        }
    }
}
