using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    class Field : GameObject {
        public Vector2 size;

        private int[,] layout;

        public Field(Vector2 position, Vector2 size) {
            this.position = position;
            this.size = size;
        }

        public override void Initialize() {
            this.layout = new int[(int)this.size.Y, (int)this.size.X];

            for (int i = 0; i < this.size.Y; i++)
                for (int j = 0; j < this.size.X; j++)
                    this.layout[i, j] = (j == 0 || j == this.size.X - 1 || i == this.size.Y - 1) ? 1 : 0;
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");
        }

        public override void Update(float deltaTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.size.Y; i++) {
                for (int j = 0; j < this.size.X; j++) {
                    if (j == 0 || j == this.size.X - 1 || i == this.size.Y - 1)
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), Color.LightGray);

                    if (this.layout[i, j] == 0)
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), new Color(0, 0, 40));
                }
            }
        }

        public bool DoesTetrominoFit(int[,] tetrominoLayout, Vector2 tetrominoPosition) {
            Vector2 layoutIndex = new Vector2(
                (float)Math.Ceiling((tetrominoPosition.X - this.position.X) / this.texture.Width),
                (float)Math.Ceiling((tetrominoPosition.Y - this.position.Y) / this.texture.Height)
            );

            for (int i = 0; i < tetrominoLayout.GetLength(0); i++) {
                for (int j = 0; j < tetrominoLayout.GetLength(1); j++) {
                    if (tetrominoLayout[i, j] == 1 && this.layout[(int)layoutIndex.Y + i, (int)layoutIndex.X + j] == 1)
                        return false;
                }
            }

            return true;
        }
    }
}
