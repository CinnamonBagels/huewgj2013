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
    class Foursquare : AMinigame
    {
        MouseState mstate = new MouseState();
        //Texture2D img_happy;
        //Texture2D img_sad;
        Texture2D img_foursquare1;
        Texture2D img_foursquare2;
        Texture2D img_foursquare3;
        Texture2D img_foursquare4;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        List<Rectangle> zones = new List<Rectangle>();

        Vector2 center = new Vector2(512, 384);
        float scale;

        bool mouseDown = false;

        int panel = 1;

        public Foursquare(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_foursquare1 = Content.Load<Texture2D>("minigames/Foursquare/foursquare1");
            img_foursquare2 = Content.Load<Texture2D>("minigames/Foursquare/foursquare2");
            img_foursquare3 = Content.Load<Texture2D>("minigames/Foursquare/foursquare3");
            img_foursquare4 = Content.Load<Texture2D>("minigames/Foursquare/foursquare4");
            //img_sad   = Content.Load<Texture2D>("minigames/Example/sad");
        }

        public override void draw(SpriteBatch sb)
        {
            scale = 768.0f / (float)img_foursquare1.Height;
            Vector2 tempCenter = new Vector2((float)(center.X - img_foursquare1.Width*scale/2.0), (float)(center.Y - img_foursquare1.Height*scale/2.0));            

            sb.DrawString(font, "" + stateTimer, new Vector2(pos3.X - 100.0f, pos3.Y + 100.0f), Color.Red);

            switch (panel)
            {
                case 1:
                    sb.Draw(img_foursquare1, tempCenter, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    break;
                case 2:
                    sb.Draw(img_foursquare2, tempCenter, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    break;
                case 3:
                    sb.Draw(img_foursquare3, tempCenter, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    break;
                case 4:
                    sb.Draw(img_foursquare4, tempCenter, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    break;
                default:
                    break;
            }

            sb.DrawString(font, "Foursqare " + panel + "  @  " + mstate.ToString(), pos2, Color.Red);

            switch (state)
            {
                case State.INTRO:
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.PLAY:
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
            }
        }

        public override int update(KeyboardState kb, MouseState ms)
        {
            mstate = ms;
            speed = Game1.speed;
            timer += speed * 2.0f;

            mouseDown = (ms.LeftButton == ButtonState.Pressed);

            switch (state)
            {
                case State.START:
                    panel = 1;

                    zones.Add(new Rectangle(435, 449, 46, 46));
                    zones.Add(new Rectangle(294, 156, 406, 72));
                    zones.Add(new Rectangle(586, 31, 154, 56));
                    zones.Add(new Rectangle(294, 574, 436, 50));

                    gameStatus = -1;

                    state = State.INTRO;
                    break;
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Check-in Foursquare! (Left Mouse Button)");

                    stateTimer += speed;
                    if (stateTimer >= gameIntroTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.PLAY;
                    }
                    break;
                case State.PLAY:
                    stateTimer += speed;
                    if (stateTimer >= gamePlayTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.LOSE;
                    }
                    else
                    {
                        if (mouseDown)
                        {
                            switch (panel)
                            {
                                case 1:
                                    if (zones[0].Contains(new Point(ms.X, ms.Y)))
                                    {
                                        panel = 2;
                                    }
                                    break;
                                case 2:
                                    if (zones[1].Contains(new Point(ms.X, ms.Y)))
                                    {
                                        panel = 3;
                                    }
                                    break;
                                case 3:
                                    if (zones[2].Contains(new Point(ms.X, ms.Y)) || zones[3].Contains(new Point(ms.X, ms.Y)))
                                    {
                                        panel = 4;
                                        stateTimer = 0.0f;
                                        state = State.WIN;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case State.WIN:
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        gameStatus = 1;
                        state = State.EXIT;
                    }
                    break;
                case State.LOSE:
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        gameStatus = 0;
                        state = State.EXIT;
                    }
                    break;
                case State.EXIT:
                    return gameStatus;
                default:
                    break;
            }
            timer = 0.0f;
            return -1;
        }
    }
}
