using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Breakout {
    public class Breakout : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Point _screenSize;

        private List<GameObject> _gameObjects;
        private FieldBoundary _fieldBoundary;
        private Paddle _paddle;

        private CollisionComponent _collisionComponent;

        public Breakout() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _screenSize = new Point(960, 720);

            _graphics.PreferredBackBufferWidth = _screenSize.X;
            _graphics.PreferredBackBufferHeight = _screenSize.Y;
            _graphics.ApplyChanges();

            _fieldBoundary = new FieldBoundary(new Vector2(50, 50));
            _paddle = new Paddle(new Vector2(_screenSize.X / 2, _screenSize.Y - 120));

            _gameObjects = new List<GameObject>() {
                 _fieldBoundary,
                _paddle
            };

            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, _screenSize.X, _screenSize.Y));
            
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameObjects.ForEach(delegate(GameObject gameObject) { gameObject.LoadContent(Content); });

            _collisionComponent.Insert(_paddle);
            _fieldBoundary.Colliders.ForEach(delegate (ICollisionActor collider) { _collisionComponent.Insert(collider); });
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Update(gameTime); });
            _collisionComponent.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _gameObjects.ForEach(delegate (GameObject gameObject) { gameObject.Draw(_spriteBatch); });

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
