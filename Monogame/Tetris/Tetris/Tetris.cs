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
        private Tetromino tetromino;

        private int inputDelay;
        private float inputTick;
        private float timeSinceLastInputTick;

        public Tetris() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {
            this.screenSize = new Vector2(640, 480);
            
            this.graphics.PreferredBackBufferWidth = (int)this.screenSize.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenSize.Y;

            this.inputDelay = 50;
            this.inputTick = (float)this.inputDelay / 1000;
            this.timeSinceLastInputTick = 0;

            this.field = new Field(new Vector2(10, 10), new Vector2(14, 26));
            this.tetromino = new Tetromino(Tetromino.Type.T, new Vector2(70, 10));

            this.gameObjects = new List<GameObject>() {
                this.field,
                this.tetromino
            };

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Initialize(); });

            base.Initialize();
        }

        protected override void LoadContent() {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.LoadContent(this.Content); });
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            this.timeSinceLastInputTick += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.timeSinceLastInputTick >= this.inputTick) {
                this.timeSinceLastInputTick = 0;

                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Left) && this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.position + new Vector2(-this.tetromino.texture.Width, 0)))
                    this.tetromino.MoveLeft();

                if (state.IsKeyDown(Keys.Right) && this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.position + new Vector2(this.tetromino.texture.Width, 0)))
                    this.tetromino.MoveRight();

                if (state.IsKeyDown(Keys.Down) && this.field.DoesTetrominoFit(this.tetromino.layout, this.tetromino.position + new Vector2(0, this.tetromino.texture.Height)))
                    this.tetromino.MoveDown();
            }

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Update((float)gameTime.ElapsedGameTime.TotalSeconds); });

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Draw(this.spriteBatch); });

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
