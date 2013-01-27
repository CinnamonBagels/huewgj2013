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
using HueWGJ2013;


namespace HueWGJ2013.minigames
{
    class BelAir:AMinigame
    {
        Texture2D img_baller;
        Texture2D pixel1;

        Rectangle powerBg;
        Rectangle powerWin;
        Rectangle powerCur;

        Vector2 powerBgPos;
        Vector2 powerWinPos;
        Vector2 powerCurPos;
        Vector2 ballerPos;
        
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        float barDir;
        bool throwing;
        bool hasThrown;

        public BelAir(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font  = font;
            img_baller = Content.Load<Texture2D>("minigames/BelAir/baller");
            pixel1     = Game1.hueGraphics.getSolidTexture();
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "BelAir", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            switch (state)
            {
                case State.PLAY:
                    sb.Draw(img_baller, ballerPos, Color.White);
                    sb.Draw(pixel1, powerBg, Color.Red);
                    sb.Draw(pixel1, powerWin, Color.Green);
                    sb.Draw(pixel1, powerCur, Color.Black);
                    break;
                case State.WIN:
                    sb.Draw(img_baller, ballerPos, Color.White);
                    sb.DrawString(font, "WIN!", ballerPos, Color.Green);
                    break;
                case State.LOSE:
                    sb.Draw(img_baller, ballerPos, Color.White);
                    sb.DrawString(font, "LOSE!", ballerPos, Color.Green);
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

                    Random rand = new Random();

                    powerBgPos = new Vector2(30,380);
                    powerWinPos = new Vector2(rand.Next(45) + 45,380);
                    powerCurPos = new Vector2(30,380);
                    ballerPos = new Vector2(30, 400);

                    powerBg = new Rectangle();
                    powerBg.Width = 100;
                    powerBg.Height = 20;
                    powerBg.Offset((int)powerBgPos.X, (int)powerBgPos.Y);

                    powerWin = new Rectangle();
                    powerWin.Width = 20;
                    powerWin.Height = 20;
                    powerWin.Offset((int)powerWinPos.X, (int)powerWinPos.Y);

                    powerCur = new Rectangle();
                    powerCur.Width = 2;
                    powerCur.Height = 20;
                    powerCur.Offset((int)powerCurPos.X, (int)powerCurPos.Y);

                    throwing = false;
                    hasThrown = false;
                    barDir = 0;
                    //Ready to go!
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
                    if (kb.IsKeyDown(Keys.Space) && throwing == false && hasThrown == false)
                    {
                        throwing = true;
                        barDir = 1* speed;
                        powerCur.X += (int) Math.Ceiling(barDir);
                    }
                    if (kb.IsKeyDown(Keys.Space) && throwing == true && hasThrown == false)
                    {
                        if (powerCur.X >= (powerBg.X + powerBg.Width) && barDir > 0)
                        {
                            powerCur.X = powerBg.X + powerBg.Width;
                            barDir = -1 * speed;
                        }
                        else if(powerCur.X <= powerBg.X && barDir < 0)
                        {
                            barDir = 1 * speed;
                        }
                        powerCur.X += (int)Math.Ceiling(barDir);
                    }
                    if (!kb.IsKeyDown(Keys.Space) && throwing == true && hasThrown == false)
                    {
                        hasThrown = true;
                        if( (powerCur.X >= powerWin.X) && (powerCur.X <= (powerWin.X + powerWin.Width) ) )
                        {
                            stateTimer = 0.0f;
                            state = State.WIN;
                        }
                        else
                        {
                            stateTimer = 0.0f;
                            state = State.LOSE;
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
            return -1;
        }
    }
}

