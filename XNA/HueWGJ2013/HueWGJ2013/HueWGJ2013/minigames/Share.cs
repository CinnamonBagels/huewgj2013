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
        static int sentenceNumber = 0;
        static int letterNumber = 0;
        static char currentLetter;
        string sentence = "give me a share";
        bool g = false;
        bool i = false;
        bool v = false;
        bool e = false;
        bool space = false;
        bool m = false;
        bool e2 = false;
        bool space2 = false;
        bool a = false;
        bool space3 = false;
        bool s = false;
        bool h = false;
        bool a2 = false;
        bool r = false;
        bool e3 = false;


        public Share(ContentManager c)
            :base (c)
        {
            this.Content = c;
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "Share", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            sb.DrawString(font, "Type: give me a share", answerPos, Color.Red);
            switch (state)
            {
                case State.START:
                    break;
                case State.INTRO:
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    break;
                case State.PLAY:
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
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
                    gameStatus = -1;
                    currentLetter = sentence[letterNumber];
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
                    stateTimer += speed;
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
                            case ' ':
                                nextKey = Keys.Space;
                                break; 
                        }
                        if (g == true)
                            if (i == true)
                                if (v == true)
                                    if (e == true)
                                        if (space == true)
                                            if (m == true)
                                                if (e2 == true)
                                                    if (space2 == true)
                                                        if (a == true)
                                                            if (space3 == true)
                                                                if (s == true)
                                                                    if (h == true)
                                                                        if (a2 == true)
                                                                            if (r == true)
                                                                                if (e3 == true)
                                                                                {
                                                                                    stateTimer = 0.0f;
                                                                                    state = State.WIN;
                                                                                }

                        if(kb.IsKeyDown(Keys.G))
                        {
                            g = true;
                        }
                        if(kb.IsKeyDown(Keys.I))
                        {
                            i = true;
                        }
                        if(kb.IsKeyDown(Keys.V))
                        {
                            v = true;
                        }
                        if (kb.IsKeyDown(Keys.E) || e == true || e2 == true)
                        {
                            e = true;

                            if (kb.IsKeyDown(Keys.E) && e == true)
                            {
                                e2 = true;

                                if (kb.IsKeyDown(Keys.E) && e == true && e2 == true)
                                {
                                    e3 = true;
                                }
                            }
                        }
                        if(kb.IsKeyDown(Keys.Space))
                        {
                            space = true;

                            if(kb.IsKeyDown(Keys.Space) && space == true)
                            {
                                space2 = true;
                                if(kb.IsKeyDown(Keys.Space) && space == true && space2 == true)
                                {
                                    space3 = true;
                                }
                            }
                        }
                        if(kb.IsKeyDown(Keys.M))
                        {
                            m = true;
                        }
                        if(kb.IsKeyDown(Keys.A))
                        {
                            a = true;

                            if(kb.IsKeyDown(Keys.A) && a == true)
                            {
                                a2 = true;
                            }
                        }
                        if(kb.IsKeyDown(Keys.S))
                        {
                            s = true;
                        }
                        if(kb.IsKeyDown(Keys.H))
                        {
                            h = true;
                        }
                        if(kb.IsKeyDown(Keys.R))
                        {
                            r = true;
                        }

                    }
                    break;
                case State.WIN:
                    g = false;
                    i = false;
                    v = false;
                    e = false;
                    space = false;
                    m = false;
                    e2 = false;
                    space2 = false;
                    a = false;
                    space3 = false;
                    s = false;
                    h = false;
                    a2 = false;
                    r = false;
                    e3 = false;

                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        gameStatus = 1;
                        state = State.EXIT;
                    }
                    break;
                case State.LOSE:
                    g = false;
                    i = false;
                    v = false;
                    e = false;
                    space = false;
                    m = false;
                    e2 = false;
                    space2 = false;
                    a = false;
                    space3 = false;
                    s = false;
                    h = false;
                    a2 = false;
                    r = false;
                    e3 = false;

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
                }
            return -1;
        }
        public override void load(SpriteFont font)
        {
            this.font = font;
        }
    }
}
