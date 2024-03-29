﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Projectile
    {
        public Texture2D Texture;

        public Vector2 Position;

        public bool Active;

        public int Damage;

        Viewport viewport;

        public int Width { get { return Texture.Width; } }
        public int Height { get { return Texture.Height; } }

        public float speed;

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            this.viewport = viewport;
            Position = position;
            Active = true;
            Damage = 2;
            speed = 20f;
        }

        public void Update()
        {
            Position.X += speed;
            if (Position.X + Texture.Width / 2 > viewport.Width)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
                new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }


    }
}
