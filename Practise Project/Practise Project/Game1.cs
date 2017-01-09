using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprites;
using System;

namespace Practise_Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //making the splash screen sprite
        Sprite _splash;

        //making game background
        private Texture2D background;

        //checking if the game started so we can swap backgrounds
        bool gameStart = false;

        //loading music
        Song music;

        //creating the engine to run the game
        Engine engine;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 900;
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //starting the Engine
            engine = new Engine(this);

            //loading splash screen image
            _splash = new Sprite(Content.Load<Texture2D>(@"Backgrounds\space_race_logo"), Vector2.Zero, 1);

            //load in background image
            background = Content.Load<Texture2D>(@"Backgrounds\background");

            //load in music
            music = Content.Load<Song>("greed");
            

            // TODO: use this.Content to load your game content here
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
        protected override void Update(GameTime gameTimes)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Update engine
            engine.Update(gameTimes);

            //starting game and switching from splash to game and playing music
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                gameStart = true;
                MediaPlayer.Play(music);
            }
            if (!gameStart)
            {
                _splash.Update(gameTimes);
            }

            // TODO: Add your update logic here

            base.Update(gameTimes);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (gameStart)
            {
                spriteBatch.Draw((background), (Vector2.Zero), Color.White);
            }

            spriteBatch.End();

            //drawing engine
            engine.Draw(gameTime);

            //draw splash screen, is an AnimatedSprite so doesnt need to be in spritebatch
            if (!gameStart)
            {
                _splash.Draw(spriteBatch);
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
