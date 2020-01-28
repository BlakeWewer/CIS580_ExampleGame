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
        Vector2 ballPosition = new Vector2(900, 359);
//        Vector2 ballPosition = new Vector2(900, 416);
        Vector2 ballVelocity;
        Vector2 wallPosition = Vector2.Zero;
        Vector2 paddlePosition = new Vector2(950, 322);
        Vector2 paddleVelocity = Vector2.Zero;
        int hits = 0;
        int hitsForNextBallSpeedInc = 7;
        int hearts = 2;
        int points = 0;
        float defaultBallSpeedVariable = 0.5F;
        float ballSpeedVariable = .5F;
        float maxBallSpeedVariable = 2F;


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

            //float initYBallVelocity = (float)random.NextDouble();
            //float initXBallVelocity = -1 * (Math.Max((float)random.NextDouble(), initYBallVelocity));
            //ballVelocity = new Vector2(initXBallVelocity, initYBallVelocity);

            ballVelocity = new Vector2(1, 0);

            ballVelocity.Normalize();

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

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here

            // Update Ball Position
            
            if(hits >= hitsForNextBallSpeedInc)
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
            if(paddlePosition.Y < 25)
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
            if(ballVelocity.X > 0)
            {
                if (ballPosition.Y > paddlePosition.Y - 46 && ballPosition.Y < paddlePosition.Y + 13)
                {
                    if (ballPosition.X > 904 && ballPosition.X < 921)
                    {
                        ballVelocity.X = -1 * Math.Abs(ballVelocity.X);
                        ballVelocity.Y -= 0.5F;
                        ballVelocity.Normalize();
                        hits++;
                        Console.WriteLine(hits + " - " + ballSpeedVariable);
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
                        Console.WriteLine(hits + " - " + ballSpeedVariable);
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
                        Console.WriteLine(hits + " - " + ballSpeedVariable);
                    }
                }
                // ballPosition.X < 963 would be a collision on the backside of the paddle
            }


            // Check for ball - wall collisions
            if (ballPosition.Y < -5)
            {
                if(Math.Abs(ballVelocity.Y) <= Math.Abs(ballVelocity.X))
                {
                    ballVelocity.Y *= -1;
                    float delta = -5 - ballPosition.Y;
                    ballPosition.Y += 2 * delta;
                }
                else
                {
                    if(ballPosition.Y < -25)
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
                    if(ballPosition.Y > graphics.PreferredBackBufferHeight - 25)
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
            if (ballPosition.X > graphics.PreferredBackBufferWidth - 45)
            {
                ballVelocity.X *= -1;
                float delta = graphics.PreferredBackBufferWidth - 45 - ballPosition.X;
                ballPosition.X += 2 * delta;
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
            spriteBatch.Draw(ball, new Rectangle((int)(ballPosition.X),
                                                    (int)(ballPosition.Y), 
                                                    50, 50), Color.White);
            spriteBatch.Draw(wall, new Rectangle(-25, -3, 70, 1042), Color.White);
            spriteBatch.Draw(paddle, new Rectangle((int)(paddlePosition.X),
                                                    (int)(paddlePosition.Y),
                                                    20, 125), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
