using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Sprites
{
    class Engine
    {
        //creating spritebatch to draw stuff
        SpriteBatch spriteBatch;

        //need to see game to access viewport
        private Game _gameOwnedBy;

        SpriteFont scoreFont;
        Vector2 centreScreen;

        //exit sprite
        Sprite exit;

        //variable to see win state
        int win = 0;

        SoundEffect success;
        SoundEffect fail;

        int screenWidth;
        int screenHeight;

        int score = 0;
        int health = 100;
        int lives = 3;
        int collectedAll;
        int collectedAmount = 0;
        string scoreMessage = "";

        //creating player, chasers and collectables
        Player myPlayer;
        ChasingEnemy[] chasers;
        Sprite[] collectables = new Sprite[20];

        public Engine(Game game)
        {
            //referring to game
            _gameOwnedBy = game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            //sound effects
            success = game.Content.Load<SoundEffect>("Winning Track");

            fail = game.Content.Load<SoundEffect>("lose");

            //centre vector
            centreScreen = new Vector2(game.GraphicsDevice.Viewport.Width / 2, 
                game.GraphicsDevice.Viewport.Height / 2);

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            //calcualating the number of collectables generated
            collectedAll = collectables.Length;

            //creating player
            myPlayer = new Player(new Texture2D[] {game.Content.Load<Texture2D>(@"Player Images\PlayerLeft_strip14"),
                                                game.Content.Load<Texture2D>(@"Player Images\PlayerRight_strip14"),
                                                game.Content.Load<Texture2D>(@"Player Images\PlayerUpDown_strip14"),
                                                game.Content.Load<Texture2D>(@"Player Images\PlayerUpDown_strip14"),
                                                game.Content.Load<Texture2D>(@"Player Images\PlayerIdle_strip14")}, centreScreen, 14, 6f);

            //loading exit sprite
            exit = new Sprite(game.Content.Load<Texture2D>(@"Collectables and Exit Image\Exit Image"), centreScreen, 1);

            //creating a collectable in a randomly generated 
            //position to match the size of the collectables array
            for (int i = 0; i < collectables.Length; i++)
            {
                int x = Utility.NextRandom(10, screenWidth - 10);
                int y = Utility.NextRandom(10, screenHeight - 10);
                collectables[i] = new Sprite(game.Content.Load<Texture2D>(@"Collectables and Exit Image\Collectable_strip32"), new Vector2(x, y), 32);
            }

            //creating an array to hold a random amount of chasers between 5 and 10
            chasers = new ChasingEnemy[Utility.NextRandom(5, 10)];

            //creating a chaser depending on the number 
            //generated from the random size of the array
            for (int i = 0; i < chasers.Count(); i++)
            {
                chasers[i] = new ChasingEnemy(game,
                        game.Content.Load<Texture2D>(@"Enemy Images/chasingEnemy"),
                        new Vector2(Utility.NextRandom(game.GraphicsDevice.Viewport.Width),
                            Utility.NextRandom(game.GraphicsDevice.Viewport.Height)), 1);
                chasers[i].Velocity = (float)Utility.NextRandom(2, 5);
                chasers[i].CollisionDistance = Utility.NextRandom(1, 3);
            }

            //loading a font
            scoreFont = game.Content.Load<SpriteFont>(@"Fonts\Font");
        }

        public void Update(GameTime gameTime)
        {
            //updating the player
            myPlayer.Update(gameTime);

            //clamping the player to the border
            myPlayer.position = Vector2.Clamp(myPlayer.position, Vector2.Zero,
                (new Vector2(screenWidth, screenHeight)
                - new Vector2(myPlayer.SpriteWidth, myPlayer.SpriteHeight)));

            //updating the collectables
            for (int i = 0; i < collectables.Length; i++)
            {
                collectables[i].Update(gameTime);
            }

            //updating the collectables collision detection
            for (int i = 0; i < collectables.Length; i++)
            {
                if (myPlayer.collisionDetect(collectables[i]))
                {
                    if (collectables[i].visible)
                    {
                        collectedAmount++;
                        score += Utility.NextRandom(10, 30);
                        collectables[i].visible = false;
                    }
                }
            }

            //updating collision detect for chasers
            for (int i = 0; i < chasers.Length; i++)
            {
                if (myPlayer.collisionDetect(chasers[i]))
                {
                    if (chasers[i].visible)
                    {
                        health -= Utility.NextRandom(40, 60);
                        score -= Utility.NextRandom(10, 30);
                    }
                    chasers[i].visible = false;
                }
            }

            //dealing with health when hitting enemies
            if (health <= 0)
            {
                lives -= 1;
                myPlayer.position = centreScreen;
                health = 100;
            }

            //updating chasers
            foreach (ChasingEnemy chaser in chasers)
            {
                chaser.follow(myPlayer);
                chaser.Update(gameTime);
            }

            //detecting win 
            if (myPlayer.collisionDetect(exit)  && collectedAmount == collectedAll)
            {
                success.Play();
                win = 2;
            }

            //detecting loss
            if(lives == 0)
            {
                fail.Play();
                win = 1;
            }

            //turning the score int into a string so we can write it
            scoreMessage = Convert.ToString(score);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //drawing win message
            if(win == 2)
            {
                spriteBatch.DrawString(scoreFont, ("YOU WIN, CONGRATULATIONS!"), new Vector2(10, -20) + myPlayer.position, Color.White);
            }

            //drawing lose message
            if (win == 1)
            {
                spriteBatch.DrawString(scoreFont, ("YOU LOSE, TRY AGAIN!"), new Vector2(10, -20) + myPlayer.position, Color.White);
            }

            //drawing player stats
            spriteBatch.DrawString(scoreFont, ("Lives: " + lives + " Health: " + health + " Score: " + scoreMessage), new Vector2(10, -5) + myPlayer.position, Color.White);

            exit.draw(spriteBatch);

            spriteBatch.End();

            //these are made from the AnimatedSprite class 
            //which has its own draw so its not in the spritebatch

            //drawing player
            myPlayer.Draw(spriteBatch);

            //drawing colectables
            for (int i = 0; i < collectables.Length; i++)
            {
                collectables[i].Draw(spriteBatch);
            }

            //drawing chasers
            foreach (ChasingEnemy chaser in chasers)
            {
                chaser.Draw(spriteBatch);
            }
        }
    }
}
