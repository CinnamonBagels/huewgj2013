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
    class Share: AMinigame
    {
        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);
        Vector2 answerPos = new Vector2(512.0f, 334.0f);
        Random random = new Random();

        static Keys nextKey;
        static int randomSentence;
        static int letterNumber = 0;
        static char currentLetter;
        static List<string> sentence = new List<string>();


        public Share(ContentManager c)
            :base (c)
        {
            this.Content = c;
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "Share", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            switch (state)
            {
                case State.START:
                    sentence.Add("give me a share");
                  //  sentence.Add("give me blizzard shares");
                   // sentence.Add("give me microsoft shares");
                  //  sentence.Add("give me EA shares");
                    randomSentence = 0;
                   // randomSentence = random.Next(0, 4);
                    currentLetter = sentence[randomSentence][letterNumber];
                    break;
                case State.INTRO:
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.PLAY:
                    sb.DrawString(font, "Playing", pos, Color.Red);
                   // sb.DrawString();
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                  //  sb.DrawString();
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
                   // sb.DrawString();
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
            }
        }

        public override int update(KeyboardState kb, MouseState ms)
        {
            speed = Game1.speed;
            timer += speed;

            switch(state)
            {
                case State.START:
                    answerPos = new Vector2(512.0f, 334.0f);
                    state = State.INTRO;
                    break;
                case State.INTRO:
                    stateTimer += speed;
                    if (stateTimer >= gamePlayTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.PLAY;
                    }
                    break;
                case State.PLAY:
                    if (stateTimer >= gamePlayTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.LOSE;
                    }
                    else
                    {
                        switch (currentLetter)
                        {
                            case 'A':
                                nextKey = Keys.A;
                                break;
                            case 'B':
                                nextKey = Keys.B;
                                break;
                            case 'C':
                                nextKey = Keys.C;
                                break;
                            case 'D':
                                nextKey = Keys.D;
                                break;
                            case 'E':
                                nextKey = Keys.E;
                                break;
                            case 'F':
                                nextKey = Keys.F;
                                break;
                            case 'G':
                                nextKey = Keys.G;
                                break;
                            case 'H':
                                nextKey = Keys.H;
                                break;
                            case 'I':
                                nextKey = Keys.I;
                                break;
                            case 'J':
                                nextKey = Keys.J;
                                break;
                            case 'K':
                                nextKey = Keys.K;
                                break;
                            case 'L':
                                nextKey = Keys.L;
                                break;
                            case 'M':
                                nextKey = Keys.M;
                                break;
                            case 'N':
                                nextKey = Keys.N;
                                break;
                            case 'O':
                                nextKey = Keys.O;
                                break;
                            case 'P':
                                nextKey = Keys.P;
                                break;
                            case 'Q':
                                nextKey = Keys.Q;
                                break;
                            case 'R':
                                nextKey = Keys.R;
                                break;
                            case 'S':
                                nextKey = Keys.S;
                                break;
                            case 'T':
                                nextKey = Keys.T;
                                break;
                            case 'U':
                                nextKey = Keys.U;
                                break;
                            case 'V':
                                nextKey = Keys.V;
                                break;
                            case 'W':
                                nextKey = Keys.W;
                                break;
                            case 'X':
                                nextKey = Keys.X;
                                break;
                            case 'Y':
                                nextKey = Keys.Y;
                                break;
                            case 'Z':
                                nextKey = Keys.Z;
                                break;
                                
                        }
                        if (kb.IsKeyDown(nextKey))
                        {
                            letterNumber++;
                            currentLetter = sentence[randomSentence][letterNumber];
                        }
                        else
                        {
                            state = State.LOSE;
                        }
                        if (letterNumber == sentence[randomSentence].Length)
                        {
                            state = State.WIN;
                            
                        }
                    }
                    break;
                case State.WIN:
                    return 1;
                case State.LOSE:
                    return 0;
                default:
                    break;
                    
                }
            return -1;
        }
        public override void load(SpriteFont font)
        {
            this.font = font;
        }
    }
}
