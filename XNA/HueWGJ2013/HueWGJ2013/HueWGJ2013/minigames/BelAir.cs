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
    class BelAir : AMinigame
    {
        Texture2D img_baller;
        Animation anim_baller;
        Texture2D img_hoop;
        Texture2D pixel1;

        Rectangle powerBg;
        Rectangle powerWin;
        Rectangle powerCur;

        Vector2 powerBgPos;
        Vector2 powerWinPos;
        Vector2 powerCurPos;
        Vector2 ballerPos;
        Vector2 hoopPos;

        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        SoundEffect snd_win;
        SoundEffect snd_lose;
        Song bgm;

        float barDir;
        bool throwing;
        bool hasThrown;
        bool playedEndSound = false;

        public BelAir(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_baller = Content.Load<Texture2D>("minigames/BelAir/baller");
            img_hoop = Content.Load<Texture2D>("minigames/BelAir/hoop");
            anim_baller = new Animation(img_baller, 1, 7, 10);
            pixel1 = Game1.hueGraphics.getSolidTexture();
            snd_win = Content.Load<SoundEffect>("minigames/BelAir/slam");
            snd_lose = Content.Load<SoundEffect>("minigames/default_fail");
            bgm = Content.Load<Song>("minigames/BelAir/bgm_cmonand");
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "BelAir", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            switch (state)
            {
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Prince of Bel-Air! (Space)");
                    anim_baller.draw(sb, ballerPos);
                    //sb.Draw(img_baller, ballerPos, Color.White);
                    //sb.Draw(pixel1, powerBg, Color.Red);
                    //sb.Draw(pixel1, powerWin, Color.Green);
                    //sb.Draw(pixel1, powerCur, Color.Black);
                    sb.Draw(img_hoop, hoopPos, Color.White);
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    anim_baller.draw(sb, ballerPos);
                    //sb.Draw(img_baller, ballerPos, Color.White);
                    sb.Draw(pixel1, powerBg, Color.Red);
                    sb.Draw(pixel1, powerWin, Color.Green);
                    sb.Draw(pixel1, powerCur, Color.Black);
                    sb.Draw(img_hoop, hoopPos, Color.White);
                    break;
                case State.WIN:
                    anim_baller.draw(sb, ballerPos);
                    //sb.Draw(img_baller, ballerPos, Color.White);
                    sb.Draw(img_hoop, hoopPos, Color.White);
                    sb.DrawString(font, "WIN!", ballerPos, Color.Green);
                    break;
                case State.LOSE:
                    anim_baller.draw(sb, ballerPos);
                    //sb.Draw(img_baller, ballerPos, Color.White);
                    sb.Draw(img_hoop, hoopPos, Color.White);
                    sb.DrawString(font, "LOSE!", ballerPos, Color.Green);
                    break;
            }
        }

        public override int update(KeyboardState kb, MouseState ms)
        {
            speed = Game1.speed;
            timer += speed;
            anim_baller.update();

            switch (state)
            {
                case State.START:
                    MediaPlayer.Play(bgm);
                    playedEndSound = false;
                    anim_baller.stopLoop();
                    anim_baller.goToFrame(0);
                    anim_baller.animateThis = false;
                    gameStatus = -1;

                    Random rand = new Random();

                    hoopPos = new Vector2(768, 500);
                    powerBgPos = new Vector2(30, 380);
                    powerWinPos = new Vector2(rand.Next(45) + 45, 380);
                    powerCurPos = new Vector2(30, 380);
                    ballerPos = new Vector2(hoopPos.X - 4 * (anim_baller.getBoundingBox().Width), hoopPos.Y + img_hoop.Height - anim_baller.getBoundingBox().Height);

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
                    if (kb.IsKeyDown(Keys.Space) && throwing == false && hasThrown == false)
                    {
                        anim_baller.goToFrame(1);
                        throwing = true;
                        barDir = 1 * speed;
                        powerCur.X += (int)Math.Ceiling(barDir);
                    }
                    if (kb.IsKeyDown(Keys.Space) && throwing == true && hasThrown == false)
                    {
                        anim_baller.goToFrame(1);
                        if (powerCur.X >= (powerBg.X + powerBg.Width) && barDir > 0)
                        {
                            powerCur.X = powerBg.X + powerBg.Width;
                            barDir = -1 * speed;
                        }
                        else if (powerCur.X <= powerBg.X && barDir < 0)
                        {
                            barDir = 1 * speed;
                        }
                        powerCur.X += (int)Math.Ceiling(barDir);
                    }
                    if (!kb.IsKeyDown(Keys.Space) && throwing == true && hasThrown == false)
                    {
                        hasThrown = true;
                        if ((powerCur.X >= powerWin.X) && (powerCur.X <= (powerWin.X + powerWin.Width)))
                        {
                            anim_baller.animateThis = true;
                            anim_baller.goToFrame(5);
                            anim_baller.loopAnimation(5, 6);
                            ballerPos = new Vector2(hoopPos.X - 75, hoopPos.Y - 35);
                            stateTimer = 0.0f;
                            state = State.WIN;
                        }
                        else
                        {
                            anim_baller.goToFrame(4);
                            ballerPos = new Vector2(hoopPos.X - 75, hoopPos.Y - 35);
                            stateTimer = 0.0f;
                            state = State.LOSE;
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
                    return gameStatus;
                default:
                    break;
            }
            return -1;
        }
    }
}

