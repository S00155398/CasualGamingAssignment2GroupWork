using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprites;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Sprites
{
    public class Player : Sprite
    {
        //creating enum to determine direction the player is going
        public enum DIRECTION { LEFT, RIGHT, UP, DOWN, STANDING };
        DIRECTION _direction = DIRECTION.STANDING;

        public DIRECTION Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        float _speed;
        Texture2D[] _textures;

        //inheriting from AnimatedSprite
        public Player(Texture2D[] texture, Vector2 pos, int framecount, float speed)
            :base(texture[0], pos, framecount)
        {
            _speed = speed;
            _textures = texture;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //setting the dfault direction to standing
            _direction = DIRECTION.STANDING;

            //moving in various directions and setting the direction variable to the direction 
            //it corresponds to
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = DIRECTION.LEFT;
                base.Move(new Vector2(-1, 0) * _speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _direction = DIRECTION.UP;
                base.Move(new Vector2(0, -1) * _speed);
            }
            if
            (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _direction = DIRECTION.DOWN;
                base.Move(new Vector2(0, 1) * _speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = DIRECTION.RIGHT;
                base.Move(new Vector2(1, 0) * _speed);
            }

            //setting the image we are using to the direction variable that is cast as an int
            SpriteImage = _textures[(int)_direction];
        }
    }
}
