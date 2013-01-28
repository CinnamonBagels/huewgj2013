using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HueWGJ2013.minigames
{
    class FightABear : AMinigame
    {
        private Texture2D imgBear = null;
        private Texture2D imgMan = null;
        private Texture2D ground = null;

        private Texture2D imgPunch = null;

        private Rectangle collBear;
        private Rectangle collPunch;
        private Rectangle collMan;

        private int bearHP = 10;
        private int stateReturned = 0;

        private Boolean isPunching = false;
        private Boolean punchCooldown = false;
        private float punchTimer = 0.0f;
        private int waveTimer = 0;

        public FightABear(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            imgBear = Content.Load<Texture2D>("minigames/FightABear/zbear");
            imgMan = Content.Load<Texture2D>("minigames/FightABear/overlymanlyman");
            ground = Game1.hueGraphics.getSolidTexture();
            imgPunch = Content.Load<Texture2D>("minigames/FightABear/falconpunch");
        }

        public override void draw(SpriteBatch sb)
        {
            Vector2 pos = new Vector2(50, 50);
            sb.DrawString(font, "FightABear", new Vector2(25, 25), Color.Red);
            sb.DrawString(font, "" + stateTimer, new Vector2(250, 25), Color.Red);
            switch (state)
            {
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Fight the bear! (Arrow keys to move, space to punch.)");
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    
                    
                    sb.Draw(imgMan, collMan, Color.White);
                    sb.Draw(imgBear, collBear, Color.White);
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    
                    sb.Draw(imgBear, collBear, Color.White);
                    sb.Draw(imgMan, collMan, Color.White);
                    if (isPunching)
                    {
                        punchTimer += speed;
                        if (punchTimer <= 1.0f)
                        {
                            sb.Draw(imgPunch, collPunch, Color.White);
                        }
                    }
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                    sb.Draw(imgMan, collMan, Color.White);
                    sb.Draw(imgBear, collBear, Color.White);
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
                    sb.Draw(imgMan, collMan, Color.White);
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
                    collBear = new Rectangle(100, 200, imgBear.Width, imgBear.Height);
                    collMan = new Rectangle(700, 200, imgMan.Width, imgMan.Height);
                    collPunch = new Rectangle(0, 0, imgPunch.Width, imgPunch.Height);
                    state = State.INTRO;
                    stateReturned = -1;
                    bearHP = 10;
                    waveTimer = 0;
                    break;

                case State.INTRO:
                    stateTimer += speed;
                    if (stateTimer >= gameIntroTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.PLAY;
                    }
                    break;

                case State.PLAY:
                    stateTimer += speed;
                    Random rand = new Random();

                    waveTimer += rand.Next(10);

                    collBear.X = (int)(collBear.X + 20 * Math.Sin(((float)waveTimer / 20.0f)/ 2.0));
                    if (stateTimer >= gamePlayTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.LOSE;
                    }
                    else
                    {
                        //Move right
                        if (kb.IsKeyDown(Keys.Right))
                        {
                            collMan.X += (int)Math.Ceiling(300 * speed);
                        }
                        else if (kb.IsKeyDown(Keys.Left))
                        {
                            collMan.X -= (int)Math.Ceiling(300 * speed);
                        }
                        if (!isPunching && kb.IsKeyDown(Keys.Space))
                        {
                            isPunching = true;
                            collPunch.X = collMan.X + 90 - imgPunch.Width;
                            collPunch.Y = collMan.Y + 130;

                            if (Rectangle.Intersect(collPunch, collBear).Height > 0
                            && Rectangle.Intersect(collPunch, collBear).Width > 0)
                            {
                                bearHP--;
                            }
                        }
                        else if (kb.IsKeyUp(Keys.Space)) 
                        {
                            isPunching = false;
                            punchTimer = 0.0f;
                        }
                        //Check for winstate
                        if (bearHP <= 0)
                        {
                            stateTimer = 0.0f;
                            state = State.WIN;
                        }
                    }
                    break;

                case State.WIN:
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.EXIT;
                    }
                    stateReturned = 1;
                    break;
                case State.LOSE:
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.EXIT;
                    }
                    stateReturned = 0;
                    break;

                case State.EXIT:
                    return stateReturned;
                default:

                    break;
            }
            timer = 0.0f;
            return -1;
        }
    }
}
