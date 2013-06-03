using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class UI
    {
        int score;
        SpriteFont spriteFont;
        Player player;

        public UI()
        {
            score = 0;
        }
        public void Initialize(Player player,ContentManager content,string datAsset)
        {
            this.player = player;
            spriteFont = content.Load<SpriteFont>(datAsset);
        }
        public void Update()
        {
            if (player.Health <= 0)
            {
                player.Health = 100;
                score = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.DrawString(spriteFont, "Score: " + score, new Vector2
                (viewport.TitleSafeArea.X, viewport.TitleSafeArea.Y), Color.White);
            spriteBatch.DrawString(spriteFont, "Health: " + player.Health, new Vector2
                (viewport.TitleSafeArea.X, viewport.TitleSafeArea.Y + 30), Color.White);
        }

        public void ChangeScore(int amount)
        {
            score += amount;
        }
    }
}
