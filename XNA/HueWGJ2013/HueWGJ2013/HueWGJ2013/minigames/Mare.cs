﻿using System;
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
    class Mare: AMinigame
    {
        Texture2D img_horsehead;
        Texture2D img_horsebody;
        Texture2D img_carrot;

        static int oPosX = 700;
        static int oPosY = 500;

       // Random random = new Random();

        List<int> rectangleX = new List<int>();
        List<int> rectangleY = new List<int>();

        Rectangle carrotRectangle = new Rectangle(oPosX, oPosY, 121, 145);
        Rectangle headRectangle = new Rectangle(336, 135, 131, 171);

        //for debugging
        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(200, 25);

        //int fed;
        //int randomRectangle = 3;

        public Mare(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_horsehead = Content.Load<Texture2D>("minigames/Mare/horsehead");
            img_horsebody = Content.Load<Texture2D>("minigames/Mare/horsebody");
            img_carrot = Content.Load<Texture2D>("minigames/Mare/carrot");
        }

        public override void draw(SpriteBatch sb)
        {
            sb.Draw(img_horsebody, new Vector2(0.0f, 0.0f), Color.White);
            sb.Draw(img_horsehead, headRectangle, Color.White);
            //sb.Draw(img_carrot, carrotRectangle, Color.White);

            sb.DrawString(font, "Raise a Mare", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);

            switch (state)
            {
                case State.START:
                    rectangleX.Add(600);
                    rectangleX.Add(550);
                    rectangleX.Add(350);
                    rectangleX.Add(400);

                    rectangleY.Add(500);
                    rectangleY.Add(400);
                    rectangleY.Add(300);
                    rectangleY.Add(200);

                    //fed = 0;

                    break;
                case State.INTRO:
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    break;
                case State.PLAY:
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    
                    sb.Draw(img_carrot, carrotRectangle, Color.White);
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

            switch (state)
            {
                case State.START:
                    gameStatus = -1;
                    state = State.INTRO;
                   // randomRectangle = random.Next(0, 4);
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
                        if (ms.LeftButton == ButtonState.Pressed && carrotRectangle.Contains(ms.X, ms.Y))
                        {
                            carrotRectangle = new Rectangle(ms.X - img_carrot.Width/2, ms.Y - img_carrot.Height/2, 121, 145); 
                        }

                        if (ms.LeftButton == ButtonState.Pressed && carrotRectangle.Contains(ms.X, ms.Y) && headRectangle.Contains(ms.X, ms.Y))
                        {
                           /* fed++;
                            if (fed == 3)
                            {
                                state = State.WIN;
                            }
                            randomRectangle = random.Next(0, 4);*/

                            state = State.WIN;
                        }
                        
                    }
                    break;

                case State.WIN:
                    //fed = 0;
                    carrotRectangle.X = oPosX;
                    carrotRectangle.Y = oPosY;

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
            }
            timer = 0.0f;
            return -1;
        }
    }
}
