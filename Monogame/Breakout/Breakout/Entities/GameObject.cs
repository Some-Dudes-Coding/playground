using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout {
    abstract class GameObject {
        public Vector2 Position { get; set; }

        protected Texture2D _texture;

        public GameObject(Vector2 position) {
            Position = position;
        }

        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
