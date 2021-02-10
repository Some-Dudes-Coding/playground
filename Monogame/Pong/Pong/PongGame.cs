using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong {
    public class PongGame : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont font;

        private List<GameObject> gameObjects;

        private Ball ball;
        private Paddle leftPaddle;
        private Paddle rightPaddle;

        private int leftScore;
        private int rightScore;

        private Vector2 screenSize;

        public PongGame() {
            this.graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            this.screenSize = new Vector2(512, 288);

            this.graphics.PreferredBackBufferWidth = (int)this.screenSize.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenSize.Y;
            this.graphics.ApplyChanges();

            this.ball = new Ball(this.screenSize);

            this.leftPaddle = new Paddle(true, this.screenSize, this.ball);
            this.rightPaddle = new Paddle(false, this.screenSize, this.ball);

            this.gameObjects = new List<GameObject>() {
                this.ball,
                this.leftPaddle,
                this.rightPaddle
            };

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Initialize(); });

            base.Initialize();
        }

        protected override void LoadContent() {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            this.font = Content.Load<SpriteFont>("Font/Bit5x3");

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.LoadContent(this.Content); });
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.Space))
                this.ball.StartMoving();

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Update(gameTime); });

            if (this.ball.position.X < 0) {
                this.rightScore++;
                this.ball.Reset();
            }

            if (this.ball.position.X > this.screenSize.X) {
                this.leftScore++;
                this.ball.Reset();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();

            this.spriteBatch.DrawString(this.font, this.leftScore.ToString(), new Vector2(this.screenSize.X / 4, 30), Color.White);
            this.spriteBatch.DrawString(this.font, this.rightScore.ToString(), new Vector2(this.screenSize.X * 3 / 4, 30), Color.White);

            this.gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Draw(gameTime, this.spriteBatch); });

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
