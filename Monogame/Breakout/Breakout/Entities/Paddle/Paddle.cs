using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Breakout {
    class Paddle : GameObject, ICollisionActor {
        private float _speed;

        public IShapeF Bounds { get; set; }

        public Paddle(Vector2 position) : base(position) {
            _speed = 400f;
        }

        public override void LoadContent(ContentManager contentManager) {
            _texture = contentManager.Load<Texture2D>("Paddle/Paddle");
            
            Bounds = new RectangleF(Position.X - (_texture.Width / 2), Position.Y, _texture.Width, _texture.Height);
        }

        public override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                Bounds.Position -= new Vector2(_speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            else if (state.IsKeyDown(Keys.Right))
                Bounds.Position += new Vector2(_speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, Bounds.Position, Color.White);
        }

        public void OnCollision(CollisionEventArgs collisionInfo) {
            _speed = 0;
        }
    }
}
