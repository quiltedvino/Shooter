using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Shooter
{
    class Player
    {
        //Animation representing the player
        public Animation PlayerAnimation;

        //Position of player relative to upperleft of screen
        public Vector2 Position;

        //State of the player;
        public bool Active;

        //Amount of HP player has
        public int Health;

        //Get width of player ship
        public int Width { get { return PlayerAnimation.FrameWidth; } }

        //Get height of player ship
        public int Height { get { return PlayerAnimation.FrameHeight; } }



        public void Initialize(Animation animation, Vector2 position) 
        {
            PlayerAnimation = animation;

            //Set starting position of player to argument position
            Position = position;

            //Player is active
            Active = true;
            //Has 100 health
            Health = 100;
        }

        public void Update(GameTime gameTime) 
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
