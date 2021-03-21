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
        private Color completeColor;

        private int lineBlockAnimationDelay;

        public static event Action<int> OnLinesCompleted;

        public Field(Vector2 position) {
            this.position = position;
        }

        public override void Initialize() {
            this.sizeX = 14;
            this.sizeY = 26;

            this.emptyColor = new Color(0, 0, 60);
            this.completeColor = Color.White;

            this.lineBlockAnimationDelay = 30;

            this.layout = new List<Color>();

            for (int i = 0; i < this.sizeY; i++)
                for (int j = 0; j < this.sizeX; j++)
                    this.layout.Add((j == 0 || j == this.sizeX - 1 || i == this.sizeY - 1) ? Color.DarkGray : this.emptyColor);
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");
        }

        public override void Update(float deltaTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.sizeY; i++)
                for (int j = 0; j < this.sizeX; j++)
                    spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), this.layout[Utils.GetRotatedIndex(0, sizeX, j, i)]);
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

            List<int> linesCompleted = new List<int>();
            for (int i = 0; i < 4; i++) {
                if ((int)layoutIndex.Y + i >= this.sizeY - 1)
                    break;

                bool lineComplete = true;
                for (int j = 1; j < this.sizeX - 1; j++)
                    lineComplete = lineComplete && this.layout[Utils.GetRotatedIndex(0, this.sizeX, j, (int)layoutIndex.Y + i)] != this.emptyColor;

                if (lineComplete)
                    linesCompleted.Add((int)layoutIndex.Y + i);
            }
            
            if (linesCompleted.Count > 0)
                new Thread(this.CompleteLinesAnimation).Start(linesCompleted);
        }

        private void CompleteLinesAnimation(object linesObject) {
            List<int> lines = (List<int>)linesObject;

            for (int i = 1; i < this.sizeX - 1; i++) {
                Thread.Sleep(this.lineBlockAnimationDelay);
                lines.ForEach(line => { this.layout[Utils.GetRotatedIndex(0, this.sizeX, i, line)] = this.completeColor; });
            }

            Thread.Sleep(this.lineBlockAnimationDelay * 3);

            lines.ForEach(delegate (int line) {
                for (int i = 1; i < this.sizeX - 1; i++)
                    for (int j = line; j > 0; j--)
                        this.layout[Utils.GetRotatedIndex(0, this.sizeX, i, j)] = this.layout[Utils.GetRotatedIndex(0, this.sizeX, i, j - 1)];
            });

            OnLinesCompleted?.Invoke(lines.Count);
        }
    }
}
