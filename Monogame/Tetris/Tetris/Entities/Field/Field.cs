using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    class Field : GameObject {
        private Vector2 size;
        private int[,] layout;

        public Field() {
        
        }

        public override void Initialize() {
            this.position = new Vector2(20, 20);

            this.size = new Vector2(14, 26);
            this.layout = new int[(int)this.size.Y, (int)this.size.X];

            for (int i = 0; i < this.size.Y; i++)
                for (int j = 0; j < this.size.X; j++)
                    this.layout[i, j] = (j == 0 || j == this.size.X - 1 || i == this.size.Y - 1) ? 1 : 0;
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");
        }

        public override void Update(float deltaTime) {
            
        }

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.size.Y; i++) {
                for (int j = 0; j < this.size.X; j++) {
                    if (j == 0 || j == this.size.X - 1 || i == this.size.Y - 1)
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), Color.LightGray);
                }
            }
        }
    }
}
