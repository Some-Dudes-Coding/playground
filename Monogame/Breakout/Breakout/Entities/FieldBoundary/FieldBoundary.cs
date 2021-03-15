using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Breakout {
    class FieldBoundary : GameObject {
        public List<ICollisionActor> Colliders { get; private set; }

        private RectangleCollider _rightCollider;
        private RectangleCollider _leftCollider;

        private int _boundaryWidth;

        public FieldBoundary(Vector2 position) : base(position) {
            _boundaryWidth = 5;
        }

        public override void LoadContent(ContentManager contentManager) {
            _texture = contentManager.Load<Texture2D>("Field Boundary/Field Boundary");

            _leftCollider = new RectangleCollider(
                new RectangleF(
                    Position.X, 
                    Position.Y + _boundaryWidth, 
                    _boundaryWidth * 2, 
                    _texture.Height - (_boundaryWidth * 2)
                )
            );

            _rightCollider = new RectangleCollider(
                new RectangleF(
                    Position.X + _texture.Width - _boundaryWidth * 2,
                    Position.Y + _boundaryWidth,
                    _boundaryWidth,
                    _texture.Height - (_boundaryWidth * 2)
                )
            );

            Colliders = new List<ICollisionActor>() {
                _leftCollider,
                _rightCollider
            };
        }

        public override void Update(GameTime gameTime) {}

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
