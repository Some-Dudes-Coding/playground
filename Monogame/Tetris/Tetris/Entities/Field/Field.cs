using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    class Field : GameObject {
        private int sizeX;
        private int sizeY;

        private List<Texture2D> layout;

        public Field(Vector2 position) {
            this.position = position;
        }

        public override void Initialize() {
            this.sizeX = 14;
            this.sizeY = 26;

            this.layout = new List<Texture2D>();
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");

            for (int i = 0; i < this.sizeY; i++)
                for (int j = 0; j < this.sizeX; j++)
                    this.layout.Add((j == 0 || j == this.sizeX - 1 || i == this.sizeY - 1) ? this.texture : null);
        }

        public override void Update(float deltaTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.sizeY; i++) {
                for (int j = 0; j < this.sizeX; j++) {
                    int index = Utils.GetRotatedIndex(0, sizeX, j, i);

                    if (this.layout[index] == null)
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), new Color(0, 0, 40));
                    else
                        spriteBatch.Draw(this.layout[index], this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), Color.LightGray);
                }
            }
        }

        public bool DoesTetrominoFit(List<int> tetrominoLayout, int rotation, Vector2 tetrominoPosition) {
            Vector2 layoutIndex = this.GetTetrominoLayoutIndex(tetrominoPosition);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++) 
                    if (tetrominoLayout[Utils.GetRotatedIndex(rotation, 4, j, i)] == 1 && this.layout[Utils.GetRotatedIndex(0, this.sizeX, (int)layoutIndex.X + j, (int)layoutIndex.Y + i)] != null)
                        return false;

            return true;
        }

        public void LockTetromino(List<int> tetrominoLayout, int tetrominoRotation, Texture2D tetrominoTexture, Vector2 tetrominoPosition) {
            Vector2 layoutIndex = this.GetTetrominoLayoutIndex(tetrominoPosition);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (tetrominoLayout[Utils.GetRotatedIndex(tetrominoRotation, 4, j, i)] == 1)
                        this.layout[Utils.GetRotatedIndex(0, this.sizeX, (int)layoutIndex.X + j, (int)layoutIndex.Y + i)] = tetrominoTexture;
        }

        private Vector2 GetTetrominoLayoutIndex(Vector2 tetrominoPosition) {
            return new Vector2(
                (float)Math.Ceiling((tetrominoPosition.X - this.position.X) / this.texture.Width),
                (float)Math.Ceiling((tetrominoPosition.Y - this.position.Y) / this.texture.Height)
            );
        }
    }
}
