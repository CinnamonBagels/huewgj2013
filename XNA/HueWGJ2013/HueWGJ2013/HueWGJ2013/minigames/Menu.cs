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
    class Menu : AMinigame
    {
        //Texture2D img_happy;
        //Texture2D img_sad;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        public Menu(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            //img_happy = Content.Load<Texture2D>("minigames/Example/happy");
            //img_sad   = Content.Load<Texture2D>("minigames/Example/sad");
        }

        public override void draw(SpriteBatch sb)
        {
            Random rand = new Random();

            switch (state)
            {
                case State.PLAY:
                    String space = "          ";
                    Game1.hueGraphics.drawInstructionText("GrowioWare", Game1.predefinedColors[rand.Next(Game1.predefinedColors.Count)]); // Intro text
                    Game1.hueGraphics.drawInstructionText("\nby Team Hue");
                    Game1.hueGraphics.drawInstructionText("\n\n\nNumber of players!"); // Intro text
                    Game1.hueGraphics.drawInstructionText("\n\n\n\nType"+space+space); // Intro text
                    Game1.hueGraphics.drawInstructionText("\n\n\n\n"+"1", Color.Red); // Intro text
                    Game1.hueGraphics.drawInstructionText("\n\n\n\n"+space+"2", Color.Pink); // Intro text
                    Game1.hueGraphics.drawInstructionText("\n\n\n\n"+space+space+"3", Color.Lime); // Intro text
                    Game1.hueGraphics.drawInstructionText("\n\n\n\n"+space+space+space+"4", Color.Orange); // Intro text
                    break;
            }
        }

        public override int update(KeyboardState kb, MouseState ms)
        {
            speed = Game1.speed;
            timer += speed;

            switch (state)
            {
                case State.START:
                    gameStatus = -1;
                    state = State.PLAY;
                    break;
                case State.PLAY:
                    if (kb.IsKeyDown(Keys.D1) || kb.IsKeyDown(Keys.NumPad1))
                    {
                        gameStatus = 1;
                    }
                    else if (kb.IsKeyDown(Keys.D2) || kb.IsKeyDown(Keys.NumPad2))
                    {
                        gameStatus = 2;
                    }
                    else if (kb.IsKeyDown(Keys.D3) || kb.IsKeyDown(Keys.NumPad3))
                    {
                        gameStatus = 3;
                    }
                    else if (kb.IsKeyDown(Keys.D4) || kb.IsKeyDown(Keys.NumPad4))
                    {
                        gameStatus = 4;
                    }

                    if (gameStatus != -1)
                    {
                        state = State.EXIT;
                    }
                    break;
                case State.EXIT:
                    state = State.START;
                    int temp = gameStatus;
                    gameStatus = -1;
                    return temp;
                default:
                    break;
            }
            timer = 0.0f;
            return -1;
        }
    }
}
