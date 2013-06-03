using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace Shooter
{
    class Animation
    {
        //Image representing th e collection of images used for animation
        Texture2D spriteStrip;

        //Scale used to display the spriteship
        float scale;

        //Time elapsed since we last updated the frame
        int elapsedTime;

        //Time we dispaly a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCount;
        
        //the index of the current frame we are displaying
        int currentFrame;

        //The color of the frame we are displaying
        Color color;

        //The area of the image strip we are displaying
        Rectangle sourceRect = new Rectangle();

        //Area where we want to display the image strip in game
        Rectangle destinationRect = new Rectangle();

        //width of a given frame
        public int FrameWidth;

        //Height of a given fram
        public int FrameHeight;

        //State of animation
        public bool Active;

        //Determines if animation will keep looping or just play once
        public bool Looping;

        //Width of a given frame
        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 position,
            int frameWidth, int frameHeight, int frameCount,
            int frameTime, Color color, float scale, bool looping)
        {
            this.spriteStrip = texture;
            this.Position = position;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.color = color;
            this.scale = scale;
            this.Looping = looping;

            elapsedTime = 0;
            currentFrame = 0;

            Active = true;
        }
                
        public void Update(GameTime gameTime) 
        { 
            //Do not update the game if we are not active
            if (!Active)
                return;
            //Update elapsed gametime
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //If the elapsed time is larger than frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                //Move to next frame
                currentFrame++;

                //if currentframe is equal to framecount reset currentframe to 0
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    if (!Looping)
                        Active = false;
                }
                elapsedTime = 0;
            }
            //grab the correct frame in the image strip by multiplying the currentframe
            // index by the frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
                (int)Position.Y - (int)(FrameHeight * scale) / 2,
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale));
        }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            //only draw the animation whne we are active
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
            }
        }

    }
}
