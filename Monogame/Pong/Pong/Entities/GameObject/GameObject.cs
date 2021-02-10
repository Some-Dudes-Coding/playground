using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong {
    public abstract class GameObject : IGameObject {
        public Vector2 position;
        protected Vector2 offset;

        public Texture2D texture;

        public GameObject() { }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(GameTime gameTime);
    }
}
