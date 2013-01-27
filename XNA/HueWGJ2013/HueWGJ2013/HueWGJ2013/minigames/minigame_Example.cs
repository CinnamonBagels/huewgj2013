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
    class minigame_Example : AMinigame
    {
        //Texture2D img_happy;
        //Texture2D img_sad;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        public minigame_Example(ContentManager c)
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
            sb.DrawString(font, "minigame_Example", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
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
            speed = Game1.speed;
            timer += speed;

            switch (state)
            {
                case State.START:
                    state = State.INTRO;
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
                    if (stateTimer >= gamePlayTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.LOSE;
                    }
                    else
                    {
                        if (kb.IsKeyDown(Keys.Space))
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
