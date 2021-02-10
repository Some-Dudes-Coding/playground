using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong {
    public class Ball : GameObject {
        private Vector2 screenSize;

        private bool start;

        private float speed;
        private float speedY;
        private float speedX;
        private float angle;

        public Ball(Vector2 screenSize) {
            this.screenSize = screenSize;
        }

        public override void Initialize() {
            this.position = new Vector2(this.screenSize.X / 2, this.screenSize.Y / 2);

            this.start = false;
            this.speed = 150f;
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Ball/Ball");
            this.offset = new Vector2(this.texture.Width / 2, this.texture.Height / 2);
        }

        public override void Update(GameTime gameTime) {
            if (!this.start)
                return;

            this.position.X += this.speedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.position.Y += this.speedY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.position.Y + this.offset.Y > this.screenSize.Y || this.position.Y - this.offset.Y < 0)
                this.speedY *= -1;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.position - this.offset, Color.White);
        }

        public void Reset() {
            this.start = false;
            this.position = new Vector2(this.screenSize.X / 2, this.screenSize.Y / 2);
        }

        public void StartMoving() {
            if (this.start)
                return;

            this.start = true;

            Random random = new Random();
            this.angle = (float)(random.NextDouble() * Math.PI);

            this.speedY = this.speed * (float)Math.Sin(this.angle);
            this.speedX = this.speed * (random.Next(2) == 0 ? -1 : 1);
        }

        public void ChangeXDirection() {
            this.speedX *= -1;
        }
    }
}
