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
    class Hare : AMinigame
    {
        MouseState mstate;

        Texture2D img_hare;
        Animation hare;
        Texture2D ground;
        Texture2D img_mate;
        Texture2D img_heart;

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(250, 25);

        int currentGaps = 0;
        int maxGaps = 4;
        int maxSections = 11;
        float offset = 0.0f;
        bool alreadyGap = false;
        float winX = 0.0f;

        float gravity = -0.75f - Game1.speed;
        float yVelocity = 0.0f;
        bool onGround = false;
        Vector2 foot = new Vector2(440.0f, 192.0f);
        bool alive = true;

        Queue<Rectangle> sections = new Queue<Rectangle>();
        Queue<bool> gaps = new Queue<bool>();
        Queue<bool> mates = new Queue<bool>();
        List<Texture2D> mateTextures = new List<Texture2D>();
        List<Rectangle> mateRectangles = new List<Rectangle>();
        bool attemptingToMate = false;
        int attempts = 0;

        SoundEffect snd_win;
        SoundEffect snd_lose;
        Song bgm;
        bool playedEndSound;

        public Hare(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            this.font = font;
            img_hare = Content.Load<Texture2D>("minigames/Hare/hare");
            img_mate = Content.Load<Texture2D>("minigames/Hare/mate");
            img_heart = Content.Load<Texture2D>("minigames/Hare/heart");
            ground = Game1.hueGraphics.getSolidTexture();
            hare = new Animation(img_hare, 1, 3, 5);
            snd_win = Content.Load<SoundEffect>("minigames/default_win");
            snd_lose = Content.Load<SoundEffect>("minigames/default_fail");
            bgm = Content.Load<Song>("minigames/hare/bgm_letme");
        }

        public override void draw(SpriteBatch sb)
        {
            //sb.DrawString(font, "" + stateTimer, new Vector2(pos3.X, pos3.Y + 150.0f), Color.Red);
            switch (state)
            {
                case State.INTRO:
                    Game1.hueGraphics.drawInstructionText("Breed a hare!");
                    Game1.hueGraphics.drawInstructionText("\n(Space to jump)");
                    //sb.DrawString(font, "Intro", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    //sb.DrawString(font, "Playing", pos, Color.Red);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.LOSE:
                    //sb.DrawString(font, "LOSE!", pos, Color.Green);
                    Game1.hueGraphics.drawInstructionText("Fail!");
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
                case State.WIN:
                    //sb.DrawString(font, "WIN! " + winX, pos, Color.Green);
                    Game1.hueGraphics.drawInstructionText("Win!");
                    sb.Draw(img_heart, new Vector2(512.0f - img_heart.Width/2.0f, 384.0f + img_heart.Height/2.0f), Color.White);
                    //sb.Draw(img_happy, pos, Color.White);
                    break;
            }

            //string gapString = "";
            //foreach (bool gap in gaps)
            //{
            //    gapString = gapString + gap + " ";
            //}
            //sb.DrawString(font, alive + " Hare @ ", pos2, Color.Red);

            switch (state)
            {
                case State.WIN:
                    offset = 0.0f;
                    //sb.Draw(img_hare, new Vector2((float)(winX), (float)(foot.Y - img_hare.Height)), Color.White);
                    hare.goToFrame(0);
                    hare.draw(sb, new Vector2((float)(winX+12.0), (float)(foot.Y - img_hare.Height - 8.0)));
                    break;
                default:
                    //sb.Draw(img_hare, new Vector2((float)(foot.X - img_hare.Width / 2.0), (float)(foot.Y - img_hare.Height)), Color.White);
                    hare.draw(sb, new Vector2((float)(foot.X - 32.0), (float)(foot.Y - img_hare.Height)));
                    break;
            }

            foreach (Rectangle section in sections)
            {
                sb.Draw(ground, new Rectangle((int)(section.X - offset), section.Y, section.Width, section.Height), Color.SaddleBrown);
            }

            int i = 0;
            mateTextures.Clear();
            mateRectangles.Clear();
            foreach (bool mate in mates)
            {
                if (mate)
                {
                    Rectangle temp = new Rectangle((int)(i * 110.0 - offset + (55.0 - img_mate.Width / 2.0)), (int)(576.0 - img_mate.Height), img_mate.Width, img_mate.Height);
                    mateRectangles.Add(temp);
                    sb.Draw(img_mate, temp, Color.White);
                    mateTextures.Add(img_mate);
                }
                i++;
            }
        }

        public override int update(KeyboardState kb, MouseState ms)
        {
            mstate = ms;

            speed = Game1.speed;
            timer += speed;

            hare.update();

            offset += speed * 120.0f;
            if (offset >= 110.0f)
            {
                offset = 0.0f;
                attemptingToMate = false;
                generateNewGround();
            }

            //ground collision

            bool left = false;
            bool right = false;
            foreach (Rectangle section in sections)
            {
                left = section.Contains(new Point((int)(foot.X), (int)(foot.Y + 1.0)));
                right = section.Contains(new Point((int)(foot.X), (int)(foot.Y + 1.0)));                
                if (alive && yVelocity <= 0.0f && (left || right))
                {
                    onGround = true;
                    foot = new Vector2(foot.X, section.Y);
                    yVelocity = 0.0f;
                    break;
                }
                else if (alive)
                {
                    onGround = false;
                }

                else if (!alive && section.Contains(new Point((int)foot.X, (int)foot.Y)))
                {
                    foot = new Vector2((float)(section.X - 16.0), foot.Y);
                    break;
                }
            }

            if (!onGround)
            {
                yVelocity += gravity;
                foot = new Vector2(foot.X, foot.Y - yVelocity);
                hare.goToFrame(1);
            }
            else
            {
                yVelocity = 0.0f;
            }

            if (foot.Y > 600.0f)
            {
                alive = false;
            }

            switch (state)
            {
                case State.START:
                    playedEndSound = false;
                    MediaPlayer.Play(bgm);
                    gameStatus = -1;
                    alive = true;
                    foot = new Vector2(440.0f, 192.0f);

                    sections.Clear();
                    gaps.Clear();
                    mates.Clear();
                    mateRectangles.Clear();
                    attempts = 0;
                    currentGaps = 0;

                    Random rand = new Random();
                    for (int i = 0; i < maxSections; i++)
                    {
                        alreadyGap = false;
                        gaps.Enqueue(false);

                        mates.Enqueue(false);
                    }
                    generateNewGround();

                    state = State.INTRO;
                    break;
                case State.INTRO:
                    alive = true;
                    stateTimer += speed;
                    alreadyGap = true;

                    if (stateTimer >= gameIntroTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.PLAY;
                    }
                    break;
                case State.PLAY:
                    stateTimer += speed / 3.0f;
                    if (stateTimer >= gamePlayTimer)
                    {
                        stateTimer = 0.0f;
                        state = State.LOSE;
                    }
                    else
                    {
                        if (onGround && kb.IsKeyDown(Keys.Space))
                        {
                            yVelocity = 12.5f + 2 * Game1.speed;
                        }

                        if (!alive)
                        {
                            stateTimer = 0.0f;
                            state = State.LOSE;
                        }
                        else
                        {
                            Random rand2 = new Random();
                            foreach (Rectangle mate in mateRectangles)
                            {
                                if (mate.Contains(new Point((int)foot.X, (int)(foot.Y - 20.0))))
                                {
                                    if (!attemptingToMate)
                                    {
                                        attemptingToMate = true;
                                        if (rand2.Next(100) < 20)
                                        {
                                            stateTimer = 0.0f;
                                            state = State.WIN;
                                            winX = foot.X;
                                        }
                                        attempts++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case State.WIN:
                    if (!playedEndSound)
                    {
                        snd_win.Play();
                        playedEndSound = true;
                    }
                    stateTimer += speed;
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
            timer = 0.0f;
            return -1;
        }

        public void generateNewGround()
        {
            Random rand = new Random();
            sections.Clear();

            mates.Dequeue();
            if (gaps.Dequeue())
            {
                currentGaps--;
            }

            if (currentGaps < maxGaps)
            {
                if (!alreadyGap && rand.Next(100) < 33)
                {
                    gaps.Enqueue(true);
                    currentGaps++;
                    alreadyGap = true;
                    mates.Enqueue(false);
                }
                else
                {
                    if (rand.Next(100) < 33)
                    {
                        mates.Enqueue(true);
                    }
                    else
                        mates.Enqueue(false);
                    alreadyGap = false;
                    gaps.Enqueue(false);
                }
            }
            else
            {
                if (rand.Next(100) < 25)
                {
                    mates.Enqueue(true);
                }
                else
                    mates.Enqueue(false);
                alreadyGap = false;
                gaps.Enqueue(false);
            }

            int i = 0;
            foreach (bool gap in gaps)
            {
                if (!gap)
                {
                    sections.Enqueue(new Rectangle(i * 110, 576, 110, 384));
                }
                else
                {
                    sections.Enqueue(new Rectangle(i * 110, 576, 0, 0));
                }
                i++;
            }
        }
    }
}
