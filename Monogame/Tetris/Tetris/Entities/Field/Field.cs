using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    class Field : GameObject {
        private int sizeX;
        private int sizeY;

        private List<Color> layout;
        private Color emptyColor;
        private Texture2D lineBlockTexture;
        private Color completeColor;

        public Field(Vector2 position) {
            this.position = position;
        }

        public override void Initialize() {
            this.sizeX = 14;
            this.sizeY = 26;

            this.emptyColor = new Color(0, 0, 40);
            this.completeColor = new Color(0, 0, 0);

            this.layout = new List<Color>();

            for (int i = 0; i < this.sizeY; i++)
                for (int j = 0; j < this.sizeX; j++)
                    this.layout.Add((j == 0 || j == this.sizeX - 1 || i == this.sizeY - 1) ? Color.LightGray : this.emptyColor);
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");
            this.lineBlockTexture = contentManager.Load<Texture2D>("Block/Line Block");
        }

        public override void Update(float deltaTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.sizeY; i++) {
                for (int j = 0; j < this.sizeX; j++) {
                    if (this.layout[Utils.GetRotatedIndex(0, sizeX, j, i)] == this.completeColor)
                        spriteBatch.Draw(this.lineBlockTexture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), Color.White);
                    else
                        spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), this.layout[Utils.GetRotatedIndex(0, sizeX, j, i)]);
                }
            }
                
        }

        public bool DoesTetrominoFit(List<int> tetrominoLayout, int rotation, Vector2 tetrominoPosition) {
            Vector2 layoutIndex = this.GetTetrominoLayoutIndex(tetrominoPosition);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++) 
                    if (tetrominoLayout[Utils.GetRotatedIndex(rotation, 4, j, i)] == 1 && this.layout[Utils.GetRotatedIndex(0, this.sizeX, (int)layoutIndex.X + j, (int)layoutIndex.Y + i)] != this.emptyColor)
                        return false;

            return true;
        }

        public void LockTetromino(Tetromino tetromino) {
            Vector2 layoutIndex = this.GetTetrominoLayoutIndex(tetromino.position);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (tetromino.layout[Utils.GetRotatedIndex(tetromino.rotation, 4, j, i)] == 1)
                        this.layout[Utils.GetRotatedIndex(0, this.sizeX, (int)layoutIndex.X + j, (int)layoutIndex.Y + i)] = tetromino.tint;
        }

        private Vector2 GetTetrominoLayoutIndex(Vector2 tetrominoPosition) {
            return new Vector2(
                (float)Math.Ceiling((tetrominoPosition.X - this.position.X) / this.texture.Width),
                (float)Math.Ceiling((tetrominoPosition.Y - this.position.Y) / this.texture.Height)
            );
        }

        public void HandleLineCompletion(Vector2 tetrominoPosition) {
            Vector2 layoutIndex = this.GetTetrominoLayoutIndex(tetrominoPosition);

            for (int i = 0; i < 4; i++) {
                if ((int)layoutIndex.Y + i >= this.sizeY - 1)
                    break;

                bool lineComplete = true;
                for (int j = 1; j < this.sizeX - 1; j++)
                    lineComplete = lineComplete && this.layout[Utils.GetRotatedIndex(0, this.sizeX, j, (int)layoutIndex.Y + i)] != this.emptyColor;

                if (lineComplete)
                    for (int j = 1; j < this.sizeX - 1; j++)
                        this.layout[Utils.GetRotatedIndex(0, this.sizeX, j, (int)layoutIndex.Y + i)] = this.completeColor;
            }
        }
    }
}
