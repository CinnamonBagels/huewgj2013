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
    class HotAir : AMinigame
    {
        Texture2D img_balloon;
        //Texture2D img_sad;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        // Scale for balloon
        float scale = 1.0f;
        Vector2 balloonPos = new Vector2(512.0f, 651.0f);

        SoundEffect snd_win;
        SoundEffect snd_lose;
        Song bgm;
        bool playedEndSound;

        // Pump mechanic
        bool down = false;

        public HotAir(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_balloon = Content.Load<Texture2D>("minigames/HotAir/balloon");
            snd_win = Content.Load<SoundEffect>("minigames/default_win");
            snd_lose = Content.Load<SoundEffect>("minigames/default_fail");
            bgm = Content.Load<Song>("minigames/HotAir/bgm_99");
            //img_sad   = Content.Load<Texture2D>("minigames/Example/sad");
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "HotAir", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);
            switch (state)
            {
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Blow hot air! (Up/Down Arrows)");
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    sb.Draw(img_balloon, balloonPos, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    sb.DrawString(font, "Playing", pos, Color.Red);
                    sb.Draw(img_balloon, balloonPos, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.LOSE:
                    sb.DrawString(font, "LOSE!", pos, Color.Green);
                    sb.Draw(img_balloon, balloonPos, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.WIN:
                    sb.DrawString(font, "WIN!", pos, Color.Green);
                    sb.Draw(img_balloon, balloonPos, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
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

                    scale = 0.25f;
                    balloonPos = new Vector2(512.0f, 651.0f);

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

                    scale -= 0.08f * speed;
                    balloonPos = new Vector2(balloonPos.X + (img_balloon.Width * 0.08f * speed)/2.0f, balloonPos.Y + (img_balloon.Height * 0.08f * speed));

                    if (stateTimer >= gamePlayTimer || scale <= 0.05)
                    {
                        stateTimer = 0.0f;
                        state = State.LOSE;
                    }
                    else
                    {
                        if (((kb.IsKeyDown(Keys.Up) && !down) || (kb.IsKeyDown(Keys.Down) && down)) && (kb.IsKeyUp(Keys.Down) || kb.IsKeyUp(Keys.Up)))
                        {
                            down = !down;

                            scale += 0.1f;
                            balloonPos = new Vector2(balloonPos.X - (img_balloon.Width*0.1f)/2.0f, balloonPos.Y - (img_balloon.Height*0.1f));

                            if (scale >= 2.75f)
                            {
                                stateTimer = 0.0f;
                                state = State.WIN;
                                break;
                            }
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
                    stateTimer += speed;
                    if (!playedEndSound)
                    {
                        snd_lose.Play();
                        playedEndSound = true;
                    }
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
            timer = 0.0f;
            return -1;
        }
    }
}
