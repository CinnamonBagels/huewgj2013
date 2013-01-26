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
        public HueGraphics(GraphicsDevice gd)
        {
            pixel1 = new Texture2D(gd, 1, 1);
            pixel1.SetData(new[] { Color.White });
        }

        public Texture2D getSolidTexture()
        {
            return pixel1;
        }
    }
}
