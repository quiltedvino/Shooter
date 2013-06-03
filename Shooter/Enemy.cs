using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Shooter
{
    class Enemy
    {
        //Animation for enemy
        public Animation EnemyAnimation;

        //Position of enemy relative to topelft screen
        public Vector2 Position;

        //State of enemy
        public bool Active;

        //Hitpoints
        public int Health;

        //Damage enemy inflicts
        public int Damage;

        //score
        public int Value;

        //Width of ship
        public int Width { get { return EnemyAnimation.FrameWidth; } }
        //Height
        public int Height { get { return EnemyAnimation.FrameHeight; } }

        float moveSpeed;

        public void Initialize(Animation animation, Vector2 position)
        {
            EnemyAnimation = animation;
            Position = position;
            Active = true;
            Health = 10;
            Damage = 10;
            moveSpeed = 6f;
            Value = 100;
        }
        public void Update(GameTime gameTime)
        {
            Position.X -= moveSpeed;
            EnemyAnimation.Position = Position;

            EnemyAnimation.Update(gameTime);

            if (Position.X < -Width || Health <= 0)
            {
                Active = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw animation
            EnemyAnimation.Draw(spriteBatch);
        }

    }
}
