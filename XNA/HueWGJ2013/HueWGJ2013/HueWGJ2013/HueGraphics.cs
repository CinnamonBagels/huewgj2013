using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace HueWGJ2013.minigames
{
    /// <summary>
    /// A class containing some generic useful methods for rendering graphics
    /// </summary>
    public class HueGraphics
    {
        Texture2D pixel1;
        SpriteFont instructionFont;
        SpriteBatch spriteBatch;
        GraphicsDevice gd;
        public HueGraphics(GraphicsDevice gd, SpriteFont ifont, SpriteBatch sb)
        {
            this.gd = gd;
            pixel1 = new Texture2D(gd, 1, 1);
            pixel1.SetData(new[] { Color.White });
            this.instructionFont = ifont;
            spriteBatch = sb;
        }

        public Texture2D getSolidTexture()
        {
            return pixel1;
        }

        public void drawInstructionText(String s)
        {
            Vector2 textSize = instructionFont.MeasureString(s);
            Vector2 textCenter = new Vector2(gd.Viewport.Width / 2, 50f);
            Vector2 pos = new Vector2((float) (textCenter.X - (textSize.X / 2)), (float) 80);
            spriteBatch.DrawString(instructionFont, s, pos, Color.Black);
        }
    }
}
