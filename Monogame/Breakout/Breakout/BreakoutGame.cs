using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout {
    public class BreakoutGame : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Point _screenSize;

        private int _boundaryOffset;
        private Rectangle _boundaryDimensions;

        private Texture2D _boundaryTexture;
        private int _boundaryLineWidth;

        public BreakoutGame() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _screenSize = new Point(960, 720);

            _graphics.PreferredBackBufferWidth = _screenSize.X;
            _graphics.PreferredBackBufferHeight = _screenSize.Y;
            _graphics.ApplyChanges();

            _boundaryOffset = 50;
            _boundaryDimensions = new Rectangle(_boundaryOffset, _boundaryOffset, _screenSize.X - _boundaryOffset * 2, _screenSize.Y - _boundaryOffset * 2);

            _boundaryLineWidth = 5;

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _boundaryTexture = Content.Load<Texture2D>("Field Boundary/Field Boundary");
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_boundaryTexture, new Vector2(_boundaryOffset, _boundaryOffset), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
