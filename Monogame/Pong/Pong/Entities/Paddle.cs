using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong {
    public class Paddle : GameObject {
        private bool isLeft;
        private Vector2 screenSize;
        private Ball ball;

        private float sideOffset;
        private float speed;

        public Paddle(bool isLeft, Vector2 screenSize, Ball ball) {
            this.isLeft = isLeft;
            this.screenSize = screenSize;
            this.ball = ball;
        }

        public override void Initialize() {
            this.sideOffset = 20f;

            this.position = new Vector2(
                this.isLeft ? this.sideOffset : this.screenSize.X - this.sideOffset,
                this.screenSize.Y / 2
            );

            this.speed = 150f;
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Paddle/Paddle");
            this.offset = new Vector2(this.texture.Width / 2, this.texture.Height / 2);
        }

        public override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();

            float newPosition = this.position.Y;

            if (this.isLeft) {
                if (state.IsKeyDown(Keys.W))
                    newPosition -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (state.IsKeyDown(Keys.S))
                    newPosition += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } else {
                if (state.IsKeyDown(Keys.Up))
                    newPosition -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (state.IsKeyDown(Keys.Down))
                    newPosition += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (newPosition - this.offset.Y > 0 && newPosition + this.offset.Y < this.screenSize.Y)
                this.position.Y = newPosition;

            if (new Rectangle(this.position.ToPoint(), this.texture.Bounds.Size).Intersects(new Rectangle(this.ball.position.ToPoint(), this.ball.texture.Bounds.Size)))
                this.ball.ChangeXDirection();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.position - this.offset, Color.White);
        }
    }
}
