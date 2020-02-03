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
        Texture2D k1;
        Texture2D k5;
        Texture2D k10;
        SpriteFont font;
        Vector2 ballPosition = new Vector2(900, 359);
        Vector2 ballVelocity;
        float ballRadius;
        Vector2 wallPosition = Vector2.Zero;
        Vector2 paddlePosition = new Vector2(950, 322);
        Rectangle paddleRect, wallRect, ballRect, game_over_Rect, k1Rect, k5Rect, k10Rect;
        Vector2 paddleVelocity = Vector2.Zero;
        Vector2 k10Position;
        Vector2 k10Velocity;
        float k1Radius;
        float k5Radius;
        float k10Radius;
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
        float end_game_timer = 0;
        float game_timer = 0;
        float k1_timer = 0;
        float k5_timer = 0;
        float k10_timer = 0;
        bool k1_exists = false;
        bool k5_exists = false;
        bool k10_exists = false;
        float minDistK1Collision;
        float minDistK5Collision;
        float minDistK10Collision;



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
            ballRadius = (ballRect.Height - 20) - 2;

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

            k1Rect.X = random.Next(100, graphics.GraphicsDevice.Viewport.Width - 200);
            k1Rect.Y = random.Next(50, graphics.GraphicsDevice.Viewport.Height - 50);
            k1Rect.Height = 50;
            k1Rect.Width = 50;
            k1Radius = (k1Rect.Height - 20) / 2;

            k5Rect.X = random.Next(100, graphics.GraphicsDevice.Viewport.Width - 200);
            k5Rect.Y = random.Next(50, graphics.GraphicsDevice.Viewport.Height - 50);
            k5Rect.Height = 50;
            k5Rect.Width = 50;
            k5Radius = (k5Rect.Height - 20) / 2;

            k10Position = new Vector2(random.Next(100, graphics.GraphicsDevice.Viewport.Width - 200), random.Next(50, graphics.GraphicsDevice.Viewport.Height - 50));
            k10Rect.X = (int)k10Position.X;
            k10Rect.Y = (int)k10Position.Y;
            k10Rect.Height = 50;
            k10Rect.Width = 50;
            k10Radius = (k10Rect.Height - 20) / 2;
            k10Velocity = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            k10Velocity.Normalize();

            pointsPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width - 200, 10);

            lives = new Rectangle[] { new Rectangle(graphics.PreferredBackBufferWidth - 30, 10, live_dim, live_dim),
                        new Rectangle(graphics.PreferredBackBufferWidth - 55, 10, live_dim, live_dim),
                        new Rectangle(graphics.PreferredBackBufferWidth - 80, 10, live_dim, live_dim)};

            minDistK1Collision = k1Radius + ballRadius;
            minDistK5Collision = k5Radius + ballRadius;
            minDistK10Collision = k10Radius + ballRadius;

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
            k1 = Content.Load<Texture2D>("1K");
            k5 = Content.Load<Texture2D>("5K");
            k10 = Content.Load<Texture2D>("10K");
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

            if(k1_exists)
            {
                // Check for k1 - ball collisions
                float minDistK1Collision = k1Radius + ballRadius;
                if (Math.Sqrt(Math.Pow(k1Rect.X - ballRect.X, 2) + Math.Pow(k1Rect.Y - ballRect.Y, 2)) < minDistK1Collision)
                {
                    k1_exists = false;
                    points += 1000;
                    k1_timer = 0;
                    k1Rect.X = random.Next(100, graphics.GraphicsDevice.Viewport.Width - 200);
                    k1Rect.Y = random.Next(50, graphics.GraphicsDevice.Viewport.Height - 50);
                }
            }

            if(k5_exists)
            {
                // Check for k5 - ball collisions
                if (Math.Sqrt(Math.Pow(k5Rect.X - ballRect.X, 2) + Math.Pow(k5Rect.Y - ballRect.Y, 2)) < minDistK5Collision)
                {
                    k5_exists = false;
                    points += 5000;
                    k5_timer = 0;
                    k5Rect.X = random.Next(100, graphics.GraphicsDevice.Viewport.Width - 200);
                    k5Rect.Y = random.Next(50, graphics.GraphicsDevice.Viewport.Height - 50);
                }
            }

            // Check for k10 - wall collisions
            if(k10_exists)
            {
                k10Position += (float)gameTime.ElapsedGameTime.TotalMilliseconds * .25F * k10Velocity;
                
                if (k10Position.Y < -5)
                {
                    k10Velocity.Y *= -1;
                    float delta = -5 - k10Position.Y;
                    k10Position.Y += 2 * delta;
                }
                if (k10Position.Y > graphics.PreferredBackBufferHeight - 45)
                {
                    k10Velocity.Y *= -1;
                    float delta = graphics.PreferredBackBufferHeight - 45 - k10Position.Y;
                    k10Position.Y += 2 * delta;
                }
                if (k10Position.X < 25)
                {
                    k10Velocity.X *= -1;
                    float delta = 25 - k10Position.X;
                    k10Position.X += 2 * delta;
                }
                if (k10Position.X > graphics.PreferredBackBufferWidth - 45)
                {
                    k10Velocity.X *= -1;
                    float delta = graphics.PreferredBackBufferWidth - 45 - k10Position.X;
                    k10Position.X += 2 * delta;
                }
                k10Rect.Location = new Point((int)k10Position.X, (int)k10Position.Y);

                // Check for k10 - ball collisions
                float minDistK10Collision = k10Radius + ballRadius;
                if (Math.Sqrt(Math.Pow(k10Rect.X - ballRect.X, 2) + Math.Pow(k10Rect.Y - ballRect.Y, 2)) < minDistK10Collision)
                {
                    k10_exists = false; 
                    points += 10000;
                    k10_timer = 0;
                    k5Rect.X = random.Next(100, graphics.GraphicsDevice.Viewport.Width - 200);
                    k5Rect.Y = random.Next(50, graphics.GraphicsDevice.Viewport.Height - 50);
                }
            }

            if (num_lives > 0)
            {
                old_points = points;
                points++;
            }
            

            if(points > 20000 && old_points <= 20000)
            {
                num_lives += 1;
            }

            if(end_game_timer > 300)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    points = 0;
                    num_lives = 2;
                    end_game_timer = 0;
                    game_timer = 0;
                    hits = 0;
                    hitsForNextBallSpeedInc = 7;
                    ballPosition = new Vector2(900, 359);
                    paddlePosition = new Vector2(950, 322);
                    float initYBallVelocity = (float)random.NextDouble();
                    float initXBallVelocity = -2 * (Math.Max((float)random.NextDouble(), initYBallVelocity));
                    ballVelocity = new Vector2(initXBallVelocity, initYBallVelocity);
                    ballSpeedVariable = defaultBallSpeedVariable;
                    k1_timer = 0;
                    k5_timer = 0;
                    k10_timer = 0;

                    ballVelocity.Normalize();


                    ballRect.X = (int)ballPosition.X;
                    ballRect.Y = (int)ballPosition.Y;
                }
            }

            if(game_timer > 100 && !k1_exists && k1_timer > 500)
            {
                if(random.NextDouble() > .0005)
                {
                    k1_exists = true;
                }
            }
            if(game_timer > 500 && !k5_exists && k5_timer > 500)
            {
                if (random.NextDouble() > .00025)
                {
                    k5_exists = true;
                }
            }
            if(game_timer > 1000 && !k10_exists && k10_timer > 500)
            {
                if (random.NextDouble() > .0001)
                {
                    k10_exists = true;
                }
            }

            k1_timer++;
            k5_timer++;
            k10_timer++;
            game_timer++;

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
                spriteBatch.DrawString(font, "Move Paddle -> Arrow Keys(Up & Down)      Change Paddle Speed -> Shift(Faster), Nothing(Normal), Ctrl(Slower)", new Vector2(35, 5), Color.White);

                if(k1_exists)
                {
                    spriteBatch.Draw(k1, k1Rect, Color.White);
                    k1_timer++;
                }
                if (k5_exists)
                {
                    spriteBatch.Draw(k5, k5Rect, Color.White);
                    k5_timer++;
                }
                if (k10_exists)
                {
                    spriteBatch.Draw(k10, k10Rect, Color.White);
                    k10_timer++;
                }
            }
            else
            {
                // GAME OVER & Show score.
                spriteBatch.Draw(game_over, game_over_Rect, Color.White);
                Vector2 fontCentered = font.MeasureString("Score: " + points.ToString()) / 2;
                spriteBatch.DrawString(font, "Score: " + points.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - fontCentered.X, (graphics.GraphicsDevice.Viewport.Height / 2) + 100), Color.Black);
                end_game_timer++;
                if(end_game_timer > 300)
                {
                    Vector2 end_game_fontCentered = font.MeasureString("Press Enter to Restart") / 2;
                    spriteBatch.DrawString(font, "Press Enter to Restart", new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - end_game_fontCentered.X, (graphics.GraphicsDevice.Viewport.Height / 2) + 200), Color.Black);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
