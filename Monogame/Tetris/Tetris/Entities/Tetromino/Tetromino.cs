using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris {
    class Tetromino : GameObject {
        public enum Type {
            I,
            J,
            L,
            O,
            S,
            Z,
            T
        }

        private Type type;
        private int[,] layout;

        private static Random random = new Random();
        private Color tint;

        public Tetromino(Type type, Vector2 position) {
            this.type = type;
            this.position = position;
        }

        public override void Initialize() {
            this.tint = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));

            DetermineLayout();
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Tetromino Piece/Tetromino Piece");
        }

        public override void Update(float deltaTime) {
        
        }

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.layout.GetLength(0); i++) {
                for (int j = 0; j < this.layout.GetLength(1); j++) {
                    if (this.layout[i, j] == 0)
                        continue;

                    spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), this.tint);
                }
            }
        }

        private void DetermineLayout() {
            switch (this.type) {
                case Type.I:
                    this.layout = new int[4, 4] {
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 0, 0}
                    };
                    break;

                case Type.J:
                    this.layout = new int[4, 4] {
                        {0, 0, 1, 0},
                        {0, 0, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0}
                    };
                    break;

                case Type.L:
                    this.layout = new int[4, 4] {
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0}
                    };
                    break;

                case Type.O:
                    this.layout = new int[4,4] {
                        {0, 0, 0, 0},
                        {0, 1, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0}
                    };
                    break;

                case Type.S:
                    this.layout = new int[4, 4] {
                        {0, 0, 0, 0},
                        {0, 1, 1, 0},
                        {1, 1, 0, 0},
                        {0, 0, 0, 0}
                    };
                    break;

                case Type.Z:
                    this.layout = new int[4, 4] {
                        {0, 0, 0, 0},
                        {0, 1, 1, 0},
                        {0, 0, 1, 1},
                        {0, 0, 0, 0}
                    };
                    break;


                case Type.T:
                    this.layout = new int[4, 4] {
                        {0, 0, 0, 0},
                        {0, 1, 1, 1},
                        {0, 0, 1, 0},
                        {0, 0, 0, 0}
                    };
                    break;
            }
        }
    }
}
