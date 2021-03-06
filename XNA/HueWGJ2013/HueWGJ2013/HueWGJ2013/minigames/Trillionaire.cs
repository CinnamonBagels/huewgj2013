﻿using System;
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
    class Trillionaire : AMinigame
    {
        //Texture2D img_happy;
        //Texture2D img_sad;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        static int zeroes = 0;
        static string total = "$";
        Vector2 moneyPos = new Vector2(512.0f, 334.0f);
        bool keyDown = false;
        bool pressed1 = false;

        Texture2D atm;

        SoundEffect snd_win;
        SoundEffect snd_lose;
        Song bgm;
        bool playedEndSound = false;

        public Trillionaire(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            snd_win = Content.Load<SoundEffect>("minigames/Trillionare/getmoneygetpaid");
            snd_lose = Content.Load<SoundEffect>("minigames/default_fail");
            bgm = Content.Load<Song>("minigames/Trillionare/bgm_omf");
            atm = Content.Load<Texture2D>("minigames/Trillionare/cashmoney");
        }

        public override void draw(SpriteBatch sb)
        {
            sb.Draw(atm, new Vector2(512 - atm.Width/2,  334 - atm.Height/2), Color.White);
            //sb.DrawString(font, "Trillionaire", pos2, Color.Red);
            //sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            switch (state)
            {
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Become a trillionaire!");
                    Game1.hueGraphics.drawInstructionText("\n(1/0 keys to type amount)");
                    //sb.DrawString(font, "Intro", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    //sb.DrawString(font, "Playing", pos, Color.Red);
                    sb.DrawString(font, total, moneyPos, Color.Green);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.LOSE:
                    //sb.DrawString(font, "LOSE!", pos, Color.Green);
                    Game1.hueGraphics.drawInstructionText("Fail!");
                    sb.DrawString(font, total, moneyPos, Color.Green);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.WIN:
                    //sb.DrawString(font, "WIN!", pos, Color.Green);
                    Game1.hueGraphics.drawInstructionText("Win!");
                    sb.DrawString(font, total, moneyPos, Color.Green);
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
                    playedEndSound = false;
                    MediaPlayer.Play(bgm);
                    gameStatus = -1;

                    moneyPos = new Vector2(512.0f, 334.0f);
                    total = "$";
                    zeroes = 0;
                    pressed1 = false;

                    stateTimer = 0.0f;
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
                        if(!keyDown && !pressed1 && zeroes == 0 && !total.Equals("$1") && (kb.IsKeyDown(Keys.D1) || kb.IsKeyDown(Keys.NumPad1)))
                        {
                            keyDown = true;
                            total = total + "1";
                            moneyPos = new Vector2(moneyPos.X - 6.0f, moneyPos.Y);
                            pressed1 = true;
                        }
                        else if (!keyDown && pressed1 && (zeroes == 0 || zeroes < 12) && (kb.IsKeyDown(Keys.D0) || kb.IsKeyDown(Keys.NumPad0)))
                        {
                            keyDown = true;
                            total = total + "0";
                            zeroes++;
                            moneyPos = new Vector2(moneyPos.X - 6.0f, moneyPos.Y);
                        }
                        else if (zeroes == 12)
                        {
                            stateTimer = 0.0f;
                            state = State.WIN;
                        }
                        else if (!kb.IsKeyDown(Keys.D1) && !kb.IsKeyDown(Keys.D0) && !kb.IsKeyDown(Keys.NumPad0) && !kb.IsKeyDown(Keys.NumPad1))
                        {
                            keyDown = false;
                        }
                    }
                    break;
                case State.WIN:
                    stateTimer += speed;
                    if (!playedEndSound)
                    {
                        snd_win.Play();
                        playedEndSound = true;
                    }
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        gameStatus = 1;
                        state = State.EXIT;
                    }
                    break;
                case State.LOSE:
                    if (!playedEndSound)
                    {
                        snd_lose.Play();
                        playedEndSound = true;
                    }
                    stateTimer += speed;
                    if (stateTimer >= gameEndTimer)
                    {
                        stateTimer = 0.0f;
                        gameStatus = 0;
                        state = State.EXIT;
                    }
                    break;
                case State.EXIT:
                    MediaPlayer.Stop();
                    int temp = gameStatus;
                    gameStatus = -1;
                    state = State.START;
                    return temp;
                default:
                    break;
            }
            timer = 0.0f;
            return -1;
        }
    }
}
