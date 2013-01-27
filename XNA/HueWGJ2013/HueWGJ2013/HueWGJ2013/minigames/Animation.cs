using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HueWGJ2013
{
    class Animation
    {
        public Texture2D Texture { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        public bool animateThis { get; set; }
        private bool looping;
        private bool stopAfterFinish;
        private int loopFrom;
        private int loopTo;
        private int animationSpeed;
        private int frameskip;

        public Animation(Texture2D texture, int rows, int columns, int animationSpeed)
        {
            frameskip = 0;
            Texture = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
            animateThis = true;
            looping = false;
            this.animationSpeed = animationSpeed;
        }

        public void update()
        {
            if (animateThis)
            {
                frameskip++;
                if (frameskip % animationSpeed == 0)
                {
                    this.advanceFrame();
                    frameskip = 0;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / columns;
            int height = Texture.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public void advanceFrame()
        {
            if(looping)
            {
                if (!stopAfterFinish)
                {
                    currentFrame++;
                    if (currentFrame > loopTo)
                        currentFrame = loopFrom;
                }
                else
                {
                    currentFrame++;
                    if (currentFrame > loopTo)
                        currentFrame = loopTo;
                        animateThis = false;
                }
            }
            else
            {
                currentFrame++;
                if (currentFrame >= totalFrames)
                    currentFrame = 0;
            }
        }

        /// <summary>
        /// Go to a specified frame in the animation
        /// </summary>
        public void goToFrame(int frame)
        {
            currentFrame = frame;
            if (currentFrame >= totalFrames || currentFrame < 0)
                currentFrame = 0;
        }

        /// <summary>
        /// Go to the selected frame coordinates. Vector2 is (column, row) or (X, Y) where down is positive. Both X and Y
        /// are 0 indexed.
        /// </summary>
        /// <param name="coords"></param>
        public void goToFrame(Vector2 coords)
        {
            currentFrame = (int) (coords.Y * columns + coords.X);
            if (currentFrame >= totalFrames || currentFrame < 0)
                currentFrame = 0;
        }

        /// <summary>
        /// Loop the animation from frame from to frame to. Vector2 is (column, row) or (X, Y) where down is positive
        /// Both X and Y are 0 indexed
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void loopAnimation(Vector2 from, Vector2 to)
        {
            stopAfterFinish = false;
            looping = true;
            loopFrom = currentFrame = (int) (from.Y * columns + to.X);
            loopTo = currentFrame = (int) (from.Y * columns + to.X);
        }

        public void loopAnimation(int from, int to)
        {
            stopAfterFinish = false;
            looping = true;
            loopFrom = from;
            loopTo = to;
        }

        public void stopLoop()
        {
            looping = false;
        }
        /// <summary>
        /// Goes to the indicated frame from the indicated frame and stops the animation after it reaches the last frame.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void goToAndStop(Vector2 from, Vector2 to)
        {
            looping = true;
            stopAfterFinish = true;
            loopFrom = (int)(from.Y * columns + from.X);
            loopTo = (int)(to.Y * columns + to.X);
        }

        /// <summary>
        /// Goes to the indicated frame from the indicated frame and stops the animation after it reaches the last frame.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void goToAndStop(int from, int to)
        {
            stopAfterFinish = true;
            looping = true;
            loopFrom = from;
            loopTo = to;
        }

        /// <summary>
        /// Goes to the frame specified from the beginning and stops
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void goToAndStop(int to)
        {
            stopAfterFinish = true;
            looping = true;
            loopFrom = 0;
            loopTo = to;
        }

        public void goToAndStop(Vector2 to)
        {
            looping = true;
            stopAfterFinish = true;
            loopFrom = 0;
            loopTo = (int)(to.Y * columns + to.X);
        }

        public Rectangle getBoundingBox()
        {
            int width = Texture.Width / columns;
            int height = Texture.Height / rows;

            return new Rectangle(0, 0, width, height); 
        }
    }
}
