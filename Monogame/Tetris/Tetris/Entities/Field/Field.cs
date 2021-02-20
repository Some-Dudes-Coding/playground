using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    class Field : GameObject {
        private int sizeX;
        private int sizeY;

        private List<int> layout;

        public Field(Vector2 position) {
            this.position = position;
        }

        public override void Initialize() {
            this.sizeX = 14;
            this.sizeY = 26;

            this.layout = new List<int>();

            for (int i = 0; i < this.sizeY; i++)
                for (int j = 0; j < this.sizeX; j++)
                    this.layout.Add((j == 0 || j == this.sizeX - 1 || i == this.sizeY - 1) ? 1 : 0);
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");
        }

        public override void Update(float deltaTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.sizeY; i++) {
                for (int j = 0; j < this.sizeX; j++) {
                    int index = Utils.GetRotatedIndex(0, sizeX, j, i);

                    if (this.layout[index] == 1)
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), Color.LightGray);

                    if (this.layout[index] == 0)
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), new Color(0, 0, 40));
                }
            }
        }

        public bool DoesTetrominoFit(List<int> tetrominoLayout, int rotation, Vector2 tetrominoPosition) {
            Vector2 layoutIndex = new Vector2(
                (float)Math.Ceiling((tetrominoPosition.X - this.position.X) / this.texture.Width),
                (float)Math.Ceiling((tetrominoPosition.Y - this.position.Y) / this.texture.Height)
            );

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++) 
                    if (tetrominoLayout[Utils.GetRotatedIndex(rotation, 4, j, i)] == 1 && this.layout[Utils.GetRotatedIndex(0, this.sizeX, (int)layoutIndex.X + j, (int)layoutIndex.Y + i)] == 1)
                        return false;

            return true;
        }
    }
}
