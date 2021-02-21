using System;
using System.Collections.Generic;

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

        public List<int> layout;
        public Color tint;
        public int rotation;

        private Type type;
        private int sideSize;
        private int rotationStep;

        public Tetromino(Vector2 position) {
            this.position = position;
        }

        public override void Initialize() {
            this.sideSize = 4;
            
            this.rotation = 0;
            this.rotationStep = 90;

            this.tint = Utils.GetHighSaturatedRandomColor();
            this.type = this.GetRandomType();

            DetermineLayout();
        }

        public override void LoadContent(ContentManager contentManager) {
            this.texture = contentManager.Load<Texture2D>("Block/Block");
        }

        public override void Update(float deltaTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < this.sideSize; i++) {
                for (int j = 0; j < this.sideSize; j++) {
                    if (this.layout[Utils.GetRotatedIndex(this.rotation, this.sideSize, j, i)] == 0)
                        continue;

                    spriteBatch.Draw(this.texture, this.position + new Vector2(j * this.texture.Width, i * this.texture.Height), this.tint);
                }
            }
        }

        private Type GetRandomType() {
            Array values = Enum.GetValues(typeof(Type));
            return (Type)values.GetValue(Utils.random.Next(values.Length));
        }

        public Vector2 GetRightMovementStep() {
            return this.position + new Vector2(this.texture.Width, 0);
        }

        public void MoveRight() {
            this.position = this.GetRightMovementStep();
        }

        public Vector2 GetLeftMovementStep() {
            return this.position - new Vector2(this.texture.Width, 0);
        }

        public void MoveLeft() {
            this.position = this.GetLeftMovementStep();
        }

        public Vector2 GetDownMovementStep() {
            return this.position + new Vector2(0, this.texture.Height);
        }

        public void MoveDown() {
            this.position = this.GetDownMovementStep();
        }

        public int GetRotatedStep() {
            return this.rotation + this.rotationStep;
        }

        public void Rotate() {
            this.rotation = this.GetRotatedStep();
        }

        private void DetermineLayout() {
            switch (this.type) {
                case Type.I:
                    this.layout = new List<int>() {
                        0, 1, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 0, 0
                    };
                    break;

                case Type.J:
                    this.layout = new List<int>() {
                        0, 0, 1, 0,
                        0, 0, 1, 0,
                        0, 1, 1, 0,
                        0, 0, 0, 0
                    };
                    break;

                case Type.L:
                    this.layout = new List<int>() {
                        0, 1, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 1, 0,
                        0, 0, 0, 0
                    };
                    break;

                case Type.O:
                    this.layout = new List<int>() {
                        0, 0, 0, 0,
                        0, 1, 1, 0,
                        0, 1, 1, 0,
                        0, 0, 0, 0
                    };
                    break;

                case Type.S:
                    this.layout = new List<int>() {
                        0, 0, 0, 0,
                        0, 1, 1, 0,
                        1, 1, 0, 0,
                        0, 0, 0, 0
                    };
                    break;

                case Type.Z:
                    this.layout = new List<int>() {
                        0, 0, 0, 0,
                        0, 1, 1, 0,
                        0, 0, 1, 1,
                        0, 0, 0, 0
                    };
                    break;


                case Type.T:
                    this.layout = new List<int>() {
                        0, 0, 0, 0,
                        0, 1, 1, 1,
                        0, 0, 1, 0,
                        0, 0, 0, 0
                    };
                    break;
            }
        }
    }
}
