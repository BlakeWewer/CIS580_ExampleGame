using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        Texture2D ball;
        Texture2D wall;
        Texture2D paddle;
        Texture2D game_over;
        SpriteFont font;
        Vector2 ballPosition = new Vector2(900, 359);
        Vector2 ballVelocity;
        Vector2 wallPosition = Vector2.Zero;
        Vector2 paddlePosition = new Vector2(950, 322);
        Rectangle paddleRect, wallRect, ballRect, game_over_Rect;
        Vector2 paddleVelocity = Vector2.Zero;
        int hits = 0;
        int hitsForNextBallSpeedInc = 7;
        int num_lives = 2;
        int points = 0;
        Vector2 pointsPosition;
        float defaultBallSpeedVariable = 0.5F;
        float ballSpeedVariable = .5F;
        float maxBallSpeedVariable = 2F;
        Texture2D live;
        int live_dim = 20;
        Rectangle[] lives;
        int old_points;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run. 
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here 
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            float initYBallVelocity = (float)random.NextDouble();
            float initXBallVelocity = -2 * (Math.Max((float)random.NextDouble(), initYBallVelocity));
            ballVelocity = new Vector2(initXBallVelocity, initYBallVelocity);

            ballVelocity.Normalize();


            ballRect.X = (int)ballPosition.X;
            ballRect.Y = (int)ballPosition.Y;
            ballRect.Height = 50;
            ballRect.Width = 50;

            paddleRect.X = (int)paddlePosition.X;
            paddleRect.Y = (int)paddlePosition.Y;
            paddleRect.Height = 125;
            paddleRect.Width = 20;

            wallRect.X = -25;
            wallRect.Y = -3;
            wallRect.Height = 1042;
            wallRect.Width = 70;

            game_over_Rect.X = 171;
            game_over_Rect.Y = 34;
            game_over_Rect.Width = 700;
            game_over_Rect.Height = 700;

            pointsPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width - 200, 10);

            lives = new Rectangle[] { new Rectangle(graphics.PreferredBackBufferWidth - 30, 10, live_dim, live_dim),
                        new Rectangle(graphics.PreferredBackBufferWidth - 55, 10, live_dim, live_dim),
                        new Rectangle(graphics.PreferredBackBufferWidth - 80, 10, live_dim, live_dim)};

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = Content.Load<Texture2D>("neon_ball");
            wall = Content.Load<Texture2D>("neon_wall");
            paddle = Content.Load<Texture2D>("neon_paddle");
            live = Content.Load<Texture2D>("neon_ball");
            font = Content.Load<SpriteFont>("font");
            game_over = Content.Load<Texture2D>("game_over");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            // Update Ball Position

            if (hits >= hitsForNextBallSpeedInc)
            {
                ballSpeedVariable = Math.Min(ballSpeedVariable + .5F, maxBallSpeedVariable);
                hitsForNextBallSpeedInc *= 2;
            }
            ballPosition += (float)gameTime.ElapsedGameTime.TotalMilliseconds * ballSpeedVariable * ballVelocity;
            //            ballPosition += ballVelocity;

            // Update paddle position
            float defaultPaddleSpeedVariable = 1.0F;
            float fastPaddleSpeedVariable = 2.0F;
            float slowPaddleSpeedVariable = .5F;
            float paddleSpeedVariable = defaultPaddleSpeedVariable;
            if (Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                paddleSpeedVariable = defaultPaddleSpeedVariable;
                paddleVelocity = Vector2.Zero;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl))
                {
                    paddleSpeedVariable = slowPaddleSpeedVariable;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                {
                    paddleSpeedVariable = fastPaddleSpeedVariable;
                }
                else paddleSpeedVariable = defaultPaddleSpeedVariable;

                paddleVelocity = new Vector2(0, -1);
                paddlePosition += (float)gameTime.ElapsedGameTime.TotalMilliseconds * paddleSpeedVariable * paddleVelocity;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl))
                {
                    paddleSpeedVariable = slowPaddleSpeedVariable;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                {
                    paddleSpeedVariable = fastPaddleSpeedVariable;
                }
                else paddleSpeedVariable = defaultPaddleSpeedVariable;

                paddleVelocity = new Vector2(0, 1);
                paddlePosition += (float)gameTime.ElapsedGameTime.TotalMilliseconds * paddleSpeedVariable * paddleVelocity;
            }

            // Check for paddle at boundary
            if (paddlePosition.Y < 25)
            {
                float delta = 25 - paddlePosition.Y;
                paddlePosition.Y += delta;
            }
            if (paddlePosition.Y > graphics.PreferredBackBufferHeight - 150)
            {
                float delta = graphics.PreferredBackBufferHeight - 150 - paddlePosition.Y;
                paddlePosition.Y += delta;
            }

            // Check for paddle - ball collisions
            if (ballVelocity.X > 0)
            {
                if (ballPosition.Y > paddlePosition.Y - 46 && ballPosition.Y < paddlePosition.Y + 13)
                {
                    if (ballPosition.X > 904 && ballPosition.X < 921)
                    {
                        ballVelocity.X = -1 * Math.Abs(ballVelocity.X);
                        ballVelocity.Y -= 0.5F;
                        ballVelocity.Normalize();
                        hits++;
                    }
                }
                if (ballPosition.Y > paddlePosition.Y + 12 && ballPosition.Y < paddlePosition.Y + 63)
                {
                    if (ballPosition.X > 904 && ballPosition.X < 920)
                    {
                        ballVelocity.X = -1 * Math.Abs(ballVelocity.X);
                        ballVelocity.Y /= 2;
                        ballVelocity.Normalize();
                        hits++;
                    }
                }
                if (ballPosition.Y > paddlePosition.Y + 62 && ballPosition.Y < paddlePosition.Y + 123)
                {
                    if (ballPosition.X > 904 && ballPosition.X < 921)
                    {
                        ballVelocity.X = -1 * Math.Abs(ballVelocity.X);
                        ballVelocity.Y += 0.5F;
                        ballVelocity.Normalize();
                        hits++;
                    }
                }
                // ballPosition.X < 963 would be a collision on the backside of the paddle
            }


            // Check for ball - wall collisions
            if (ballPosition.Y < -5)
            {
                if (Math.Abs(ballVelocity.Y) <= Math.Abs(ballVelocity.X))
                {
                    ballVelocity.Y *= -1;
                    float delta = -5 - ballPosition.Y;
                    ballPosition.Y += 2 * delta;
                }
                else
                {
                    if (ballPosition.Y < -25)
                    {
                        float delta = -25 - ballPosition.Y;
                        ballPosition.Y += graphics.PreferredBackBufferHeight - 2 * delta;
                    }
                }
            }
            if (ballPosition.Y > graphics.PreferredBackBufferHeight - 45)
            {
                if (Math.Abs(ballVelocity.Y) <= Math.Abs(ballVelocity.X))
                {
                    ballVelocity.Y *= -1;
                    float delta = graphics.PreferredBackBufferHeight - 45 - ballPosition.Y;
                    ballPosition.Y += 2 * delta;
                }
                else
                {
                    if (ballPosition.Y > graphics.PreferredBackBufferHeight - 25)
                    {
                        float delta = graphics.PreferredBackBufferHeight - 25 - ballPosition.Y;
                        ballPosition.Y += -1 * graphics.PreferredBackBufferHeight - 2 * delta;
                    }
                }
            }

            if (ballPosition.X < 25)
            {
                ballVelocity.X *= -1;
                float delta = 25 - ballPosition.X;
                ballPosition.X += 2 * delta;
            }
            if (ballPosition.X > graphics.PreferredBackBufferWidth)
            {
                ballVelocity.X *= -1;
                float delta = graphics.PreferredBackBufferWidth - 45 - ballPosition.X;
                ballPosition.X += 2 * delta;
                num_lives--;

                // Reset Ball
                ballSpeedVariable = defaultBallSpeedVariable;
                float initYBallVelocity = (float)random.NextDouble();
                float initXBallVelocity = -2 * (Math.Max((float)random.NextDouble(), initYBallVelocity));
                ballVelocity = new Vector2(initXBallVelocity, initYBallVelocity);

                ballVelocity.Normalize();

                ballPosition = new Vector2(900, 359);
                hits = 0;
                hitsForNextBallSpeedInc = 7;

            }
            paddleRect.Location = new Point((int)paddlePosition.X, (int)paddlePosition.Y);
            ballRect.Location = new Point((int)ballPosition.X, (int)ballPosition.Y);

            if(num_lives > 0)
            {
                old_points = points;
                points++;
            }
            

            if(points > 10000 && old_points <= 10000)
            {
                num_lives += 1;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            
            spriteBatch.Begin();
            if(num_lives > 0)
            {
                spriteBatch.Draw(ball, ballRect, Color.White);
                spriteBatch.Draw(wall, wallRect, Color.White);
                spriteBatch.Draw(paddle, paddleRect, Color.White);
                int i = 0;
                while (i < num_lives)
                {
                    spriteBatch.Draw(live, lives[i], Color.White);
                    i++;
                }
                spriteBatch.DrawString(font, "Score: " + points.ToString(), pointsPosition, Color.White);
            }
            else
            {
                // GAME OVER & Show score.
                spriteBatch.Draw(game_over, game_over_Rect, Color.White);
                Vector2 fontCentered = font.MeasureString("Score: " + points.ToString()) / 2;
                spriteBatch.DrawString(font, "Score: " + points.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - fontCentered.X, (graphics.GraphicsDevice.Viewport.Height / 2) + 100), Color.Black);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
