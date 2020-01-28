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
        Vector2 ballPosition = new Vector2(900, 389);
        Vector2 ballVelocity;
        Vector2 wallPosition = Vector2.Zero;

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
            float initXBallVelocity = -1 * (Math.Max((float)random.NextDouble(), initYBallVelocity));

            //ballVelocity = new Vector2(
            //    (float)random.NextDouble(),
            //    (float)random.NextDouble()
            //);

            ballVelocity = new Vector2(initXBallVelocity, initYBallVelocity);

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
            float ballSpeedVariable = .75F;
            ballPosition += (float)gameTime.ElapsedGameTime.TotalMilliseconds * ballSpeedVariable * ballVelocity;
//            ballPosition += ballVelocity;

            // Check for wall collisions
            //if(ballPosition.Y < -5)
            //{
            //    ballVelocity.Y *= -1;
            //    float delta = -5 - ballPosition.Y;
            //    ballPosition.Y += 2 * delta;
            //}
            //if (ballPosition.Y > graphics.PreferredBackBufferHeight - 45)
            //{
            //    ballVelocity.Y *= -1;
            //    float delta = graphics.PreferredBackBufferHeight - 45 - ballPosition.Y;
            //    ballPosition.Y += 2 * delta;
            //}
            if (ballPosition.Y < -5)
            {
                if(Math.Abs(ballVelocity.Y) > Math.Sqrt(2) * Math.Abs(ballVelocity.X))
                {
                    float delta = -5 - ballPosition.Y;
                    ballPosition.Y += graphics.PreferredBackBufferHeight - 2 * delta;
                }
                else
                {
                    ballVelocity.Y *= -1;
                    float delta = -5 - ballPosition.Y;
                    ballPosition.Y += 2 * delta;
                }
                
            }
            if (ballPosition.Y > graphics.PreferredBackBufferHeight - 45)
            {
                if (Math.Abs(ballVelocity.Y) > Math.Sqrt(2) * Math.Abs(ballVelocity.X))
                {
                    float delta = -5 - ballPosition.Y;
                    ballPosition.Y += -1 * graphics.PreferredBackBufferHeight + 2 * delta;
                }
                else
                {
                    ballVelocity.Y *= -1;
                    float delta = graphics.PreferredBackBufferHeight - 45 - ballPosition.Y;
                    ballPosition.Y += 2 * delta;
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
                                                    50, 
                                                    50), Color.White);
            spriteBatch.Draw(wall, new Rectangle(-25, -3, 70, 1042), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
