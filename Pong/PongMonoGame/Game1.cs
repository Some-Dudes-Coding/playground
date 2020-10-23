using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D box;
        private SpriteFont scoreFont;

        Vector2 screenSize;
        Vector2 sizePlayer;
        Vector2 posPlayer1;
        Vector2 posPlayer2;
        Vector2 sizeBall;
        Vector2 posBall;
        Vector2 dirBall;
        float velBall;
        float velPlayer;
        int gapPlayers;
        Vector2 limitsPlayer;
        Vector2 limitsBall;
        int score1;
        int score2;
        string scorePanel;
        Vector2 posScorePanel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenSize = new Vector2(800, 600);
            _graphics.PreferredBackBufferWidth = (int)screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            _graphics.ApplyChanges();

            score1 = score2 = 0;
            velPlayer = 300;
            velBall = 1.5f * velPlayer;
            sizePlayer = new Vector2(30, 150);
            sizeBall = new Vector2(sizePlayer.X, sizePlayer.X);
            limitsPlayer = new Vector2(screenSize.X - sizePlayer.X, screenSize.Y - sizePlayer.Y);
            limitsBall = new Vector2(screenSize.X - sizeBall.X, screenSize.Y - sizeBall.Y);
            posBall = new Vector2(limitsBall.X / 2, limitsBall.Y / 2);
            dirBall = velBall * new Vector2(1, 1);
            gapPlayers = 35;
            posPlayer1 = new Vector2(gapPlayers, limitsPlayer.Y / 2);
            posPlayer2 = new Vector2(limitsPlayer.X - gapPlayers, limitsPlayer.Y / 2);
            scorePanel = "";
            posScorePanel = new Vector2(screenSize.X / 2, 35);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            box = Content.Load<Texture2D>("pixel");
            scoreFont = Content.Load<SpriteFont>("score");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kb.IsKeyDown(Keys.S))    posPlayer1.Y += velPlayer * dt;
            if(kb.IsKeyDown(Keys.W))    posPlayer1.Y -= velPlayer * dt;
            if(kb.IsKeyDown(Keys.Down)) posPlayer2.Y += velPlayer * dt;
            if(kb.IsKeyDown(Keys.Up))   posPlayer2.Y -= velPlayer * dt;
            
            if(posPlayer1.Y < 0) posPlayer1.Y = 0;
            else if(posPlayer1.Y > limitsPlayer.Y) posPlayer1.Y = limitsPlayer.Y;
            if(posPlayer2.Y < 0) posPlayer2.Y = 0;
            else if(posPlayer2.Y > limitsPlayer.Y) posPlayer2.Y = limitsPlayer.Y;

            posBall += dirBall * dt;
            if(posBall.X < 0) {
                posBall.X = 0;
                dirBall.X = velBall;
                score2++;
            }
            else if(posBall.X > limitsBall.X) {
                posBall.X = limitsBall.X;
                dirBall.X = -velBall;
                score1++;
            }
            if(posBall.Y < 0) {
                posBall.Y = 0;
                dirBall.Y = velBall;
            }
            else if(posBall.Y > limitsBall.Y) {
                posBall.Y = limitsBall.Y;
                dirBall.Y = -velBall;
            }
            
            scorePanel = score1 + "  /  " + score2;
            posScorePanel.X = (screenSize.X - scoreFont.MeasureString(scorePanel).X) / 2;

            if(new Rectangle(posPlayer1.ToPoint(), sizePlayer.ToPoint()).Intersects(
                new Rectangle(posBall.ToPoint(), sizeBall.ToPoint()))) {
                posBall.X = posPlayer1.X + sizePlayer.X;
                dirBall.X = velBall;
            }
            if(new Rectangle(posPlayer2.ToPoint(), sizePlayer.ToPoint()).Intersects(
                new Rectangle(posBall.ToPoint(), sizeBall.ToPoint()))) {
                posBall.X = posPlayer2.X - sizeBall.X;
                dirBall.X = -velBall;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(box, posPlayer1, null, Color.White, 0, Vector2.Zero, sizePlayer, SpriteEffects.None, 0);
            _spriteBatch.Draw(box, posPlayer2, null, Color.White, 0, Vector2.Zero, sizePlayer, SpriteEffects.None, 0);
            _spriteBatch.Draw(box, posBall, null, Color.DarkGray, 0, Vector2.Zero, sizeBall, SpriteEffects.None, 0);
            _spriteBatch.DrawString(scoreFont, scorePanel, posScorePanel, Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
