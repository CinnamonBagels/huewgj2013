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
    class GrowDownThere : AMinigame
    {
        Texture2D img_guy;
        Texture2D img_lady;
        Animation anim_lady;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);
        
        Vector2 ladyInitalPos = new Vector2(600, 400);
        Vector2 guyInitialPos = new Vector2(200, 400);

        Rectangle guyColl;
        Rectangle ladyColl;

        int stateReturn;

        SoundEffect snd_win;
        SoundEffect snd_lose;
        Song bgm;
        bool playedEndSound;

        public GrowDownThere(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_guy = Content.Load<Texture2D>("minigames/GrowDownThere/guy");
            img_lady = Content.Load<Texture2D>("minigames/GrowDownThere/lady");
            anim_lady = new Animation(img_lady, 1, 2, 10);
            anim_lady.animateThis = false;
            snd_win = Content.Load<SoundEffect>("minigames/default_win");
            snd_lose = Content.Load<SoundEffect>("minigames/default_fail");
            bgm = Content.Load<Song>("minigames/GrowDownThere/bgm_careless");
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "GrowDownThere", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            switch (state)
            {
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Grow down there! (Right Arrow)");
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    sb.Draw(img_guy, guyColl, Color.White);
                    anim_lady.draw(sb, ladyInitalPos);
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    sb.Draw(img_guy, guyColl, Color.White);
                    anim_lady.draw(sb, ladyInitalPos);
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                    sb.Draw(img_guy, guyColl, Color.White);
                    anim_lady.draw(sb, ladyInitalPos);
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
                    sb.Draw(img_guy, guyColl, Color.White);
                    anim_lady.draw(sb, ladyInitalPos);
                    break;
            }
        }

        public override int update(KeyboardState kb, MouseState ms)
        {
            speed = Game1.speed;
            timer += speed;
            anim_lady.update();

            switch (state)
            {
                case State.START:
                    MediaPlayer.Play(bgm);
                    playedEndSound = false;
                    MediaPlayer.Play(bgm);
                    anim_lady.animateThis = false;
                    anim_lady.goToFrame(0);
                    guyColl = new Rectangle((int) guyInitialPos.X, (int) guyInitialPos.Y, img_guy.Width, img_guy.Height);
                    ladyColl = anim_lady.getBoundingBox();
                    ladyColl.X = (int) ladyInitalPos.X;
                    ladyColl.Y = (int) ladyInitalPos.Y;
                    state = State.INTRO;
                    stateReturn = -1;
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
                        //Move right
                        if (kb.IsKeyDown(Keys.Right) && guyColl.X < 500)
                        {
                            guyColl.X += (int) Math.Ceiling(50 * speed); 
                        }
                        //Check for winstate
                        if (guyColl.X >= 500)
                        {
                            guyColl.X = 500;
                            anim_lady.animateThis = true;
                            anim_lady.goToAndStop(0,1);
                            stateTimer = 0.0f;
                            state = State.WIN;
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
                        state = State.EXIT;
                    }
                    stateReturn = 1;
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
                        state = State.EXIT;
                    }
                    stateReturn = 0;
                    break;
                    
                case State.EXIT:
                    MediaPlayer.Stop();
                    return stateReturn;
                default:
                    
                    break;
            }
            timer = 0.0f;
            return -1;
        }
    }
}
