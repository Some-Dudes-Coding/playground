using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    abstract class GameObject {
        public Vector2 position;

        public Texture2D texture;

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(float deltaTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
