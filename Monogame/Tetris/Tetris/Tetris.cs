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

        public Tetris() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {
            this.screenSize = new Vector2(640, 480);
            
            this.graphics.PreferredBackBufferWidth = (int)this.screenSize.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenSize.Y;

            this.gameObjects = new List<GameObject>() {
                new Tetromino(Tetromino.Type.I, new Vector2(20, 20)),
                new Tetromino(Tetromino.Type.J, new Vector2(120, 20)),
                new Tetromino(Tetromino.Type.L, new Vector2(220, 20)),
                new Tetromino(Tetromino.Type.O, new Vector2(320, 20)),
                new Tetromino(Tetromino.Type.S, new Vector2(20, 120)),
                new Tetromino(Tetromino.Type.Z, new Vector2(120, 120)),
                new Tetromino(Tetromino.Type.T, new Vector2(220, 120))
            };

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Initialize(); });

            base.Initialize();
        }

        protected override void LoadContent() {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.LoadContent(this.Content); });
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
