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
    class Pear : AMinigame
    {
        private MouseState oldState;
        MouseState mstate;
        Color mouseCoordColor = Color.Blue;

        Texture2D img_pear;
        Texture2D img_tree;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(100, 25);

        List<Texture2D> pears = new List<Texture2D>();
        List<Vector2> pearPos = new List<Vector2>();
        List<float> pearScale = new List<float>();
        List<bool> pearClicked = new List<bool>();

        public Pear(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_pear = Content.Load<Texture2D>("minigames/Pear/pear");
            img_tree   = Content.Load<Texture2D>("minigames/Pear/tree");
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "Pear", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            sb.Draw(img_tree, new Vector2(128.0f, 0.0f), Color.White);
            switch (state)
            {
                case State.INTRO:
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.PLAY:
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    for (int i = 0; i < pearPos.Count; i++)
                    {
                        sb.Draw(pears[i], pearPos[i], null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), pearScale[i], SpriteEffects.None, 0.0f);
                    }
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                    for (int i = 0; i < pearPos.Count; i++)
                    {
                        sb.Draw(pears[i], pearPos[i], null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), pearScale[i], SpriteEffects.None, 0.0f);
                    }
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
                    for (int i = 0; i < pearPos.Count; i++)
                    {
                        sb.Draw(pears[i], pearPos[i], null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), pearScale[i], SpriteEffects.None, 0.0f);
                    }
                    break;
            }
        }

        public override bool update(KeyboardState kb, MouseState ms)
        {
            mstate = ms;
            speed = Game1.speed;
            timer += speed;

            switch (state)
            {
                case State.START:
                    pearPos.Clear();
                    pearScale.Clear();
                    pears.Clear();
                    pearClicked.Clear();
                    Vector2 temp;
                    Random rand = new Random();
                    for (int i = 0; i < 5; i++)
                    {
                        temp = new Vector2(randomGen(rand, 102.4f, 0.0f) + 102.4f * i + 204.8f, randomGen(rand, 334.0f, 0.0f) + 117.0f);
                        pearPos.Add(temp);
                        pearScale.Add(1.0f);
                        pears.Add(img_pear);
                        pearClicked.Add(false);
                    }
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
                        if (ms.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                        {
                            var mousePos = new Point(ms.X, ms.Y);

                            for (int i = 0; i < pearPos.Count; i++)
                            {
                                if (pears[i].Bounds.Contains(new Point((int)(mousePos.X - pearPos[i].X), (int)(mousePos.Y - pearPos[i].Y))) && !pearClicked[i])
                                {
                                    mouseCoordColor = Color.Green;
                                    pearPos[i] = new Vector2(pearPos[i].X - img_pear.Width/2.0f, pearPos[i].Y - img_pear.Height/2.0f);
                                    pearScale[i] += 1.0f;
                                    pearClicked[i] = true;

                                    for (int j = 0; j < pearClicked.Count; j++)
                                    {
                                        if (!pearClicked[j])
                                            break;
                                        else if (j == pearClicked.Count - 1)
                                        {
                                            stateTimer = 0.0f;
                                            state = State.WIN;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                            mouseCoordColor = Color.Blue;

                        oldState = ms; // this reassigns the old state so that it is ready for next time
                    }
                    break;
                case State.WIN:
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.EXIT;
                    }
                    break;
                case State.LOSE:
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.EXIT;
                    }
                    break;
                case State.EXIT:
                    return false;
                default:
                    break;
            }
            timer = 0.0f;
            return true;
        }
    }
}

