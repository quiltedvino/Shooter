using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class ParallaxingBackground
    {
        //IMage representing background
        Texture2D texture;

        //Array of position of the parallaxing background
        Vector2[] positions;

        //speed of background movement
        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed) 
        { 
            //load background texture
            texture = content.Load<Texture2D>(texturePath);

            //set speed
            this.speed = speed;

            //if we divide the screen with the texture width then we can determine
            //  the number of tiles we need. Add 1 to it so there won't be a gap.
            positions = new Vector2[(screenWidth / texture.Width) + 1];

            //set initial positons of parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                //tiles to be side by side for tiling effect
                positions[i] = new Vector2(i * texture.Width, 0);
            }

        }
        public void Update() 
        {
            for (int i = 0; i < positions.Length; i++)
            {
                //Update position of the sc reen by adding the speed
                positions[i].X += speed;
                if (speed <= 0)
                {
                    //check the texture is out of view the put that texture
                    // at the end of the screen
                    if (positions[i].X <= -texture.Width)
                        positions[i].X = texture.Width * (positions.Length - 1);
                }
                else
                {
                    if (positions[i].X >= texture.Width * (positions.Length - 1))
                        positions[i].X = -texture.Width;
                }
                
            }
        }
        public void Draw(SpriteBatch spriteBatch, Color color) 
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture,positions[i],color);
            }
        }

    }
}
