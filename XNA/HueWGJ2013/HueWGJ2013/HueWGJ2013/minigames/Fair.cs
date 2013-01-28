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
    class Fair : AMinigame
    {
        Texture2D img_card;

        static int rect1oPosX = 331 - 91;
        static int rect1oPosY = 384 - 133;
        static int rect2oPosX = 662 - 91;
        static int rect2oPosY = 384 - 133;

        Rectangle rect11 = new Rectangle(rect1oPosX, rect1oPosY, 193, 266);
        Rectangle rect12 = new Rectangle(rect1oPosX, rect1oPosY, 193, 266);
        Rectangle rect13 = new Rectangle(rect1oPosX, rect1oPosY, 193, 266);
        Rectangle rect2 = new Rectangle(rect2oPosX, rect2oPosY, 193, 266);

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(200, 25);

        List<object> stack1 = new List<object>();
        List<object> stack2 = new List<object>();

        SoundEffect snd_win;
        SoundEffect snd_lose;
        Song bgm;
        bool playedEndSound;

        public Fair(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            img_card = Content.Load<Texture2D>("minigames/Fair/card");
            snd_win = Content.Load<SoundEffect>("minigames/default_win");
            snd_lose = Content.Load<SoundEffect>("minigames/default_fail");
            bgm = Content.Load<Song>("minigames/bgm_default");
            this.font = font;
        }

        public override void draw(SpriteBatch sb)
        {
            //sb.DrawString(font, "Make it Fair", pos2, Color.Red);
            //sb.DrawString(font, "" + stateTimer, pos3, Color.Red);

            sb.Draw(img_card, rect11, Color.White);
            sb.Draw(img_card, rect12, Color.White);
            sb.Draw(img_card, rect13, Color.White);
            sb.Draw(img_card, rect2, Color.White);
            switch (state)
            {
                case State.START:
                    break;
                case State.INTRO:
                    //sb.DrawString(font, "Intro", pos, Color.Red);
                    Game1.hueGraphics.drawInstructionText("Make it fair!");
                    Game1.hueGraphics.drawInstructionText("\n(Move a card from left to right)");
                    break;
                case State.PLAY:
                    if (stateTimer < 3f)
                    {
                        Game1.hueGraphics.drawInstructionText("GO!!!");
                    }
                    //sb.DrawString(font, "Playing", pos, Color.Red);
                    break;
                case State.LOSE:
                    Game1.hueGraphics.drawInstructionText("Fail!");
                    //Game1.hueGraphics.drawInstructionText("Fail!");
                    //sb.DrawString(font, "LOSE!", pos, Color.Green);
                    break;
                case State.WIN:
                    Game1.hueGraphics.drawInstructionText("Win!");
                    //Game1.hueGraphics.drawInstructionText("Win!");
                    //sb.DrawString(font, "WIN!", pos, Color.Green);
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
                    playedEndSound = false;
                    MediaPlayer.Play(bgm);

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
                    if (ms.LeftButton == ButtonState.Pressed && rect11.Contains(ms.X, ms.Y))
                    {
                        rect11 = new Rectangle(ms.X - img_card.Width / 2, ms.Y - img_card.Height / 2, 196, 266);
                    }

                    if (ms.LeftButton == ButtonState.Pressed && rect11.Contains(ms.X, ms.Y) && rect2.Contains(ms.X, ms.Y))
                    {
                        stateTimer = 0.0f;
                        state = State.WIN;
                    }
                    break;
                case State.WIN:
                    rect11.X = rect1oPosX;
                    rect11.Y = rect1oPosY;
                    rect12.X = rect1oPosX;
                    rect12.Y = rect1oPosY;
                    rect13.X = rect1oPosX;
                    rect13.Y = rect1oPosY;
                    rect2.X = rect2oPosX;
                    rect2.Y = rect2oPosY;

                    stack1.Clear();
                    stack2.Clear();
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
                    rect11.X = rect1oPosX;
                    rect11.Y = rect1oPosY;
                    rect12.X = rect1oPosX;
                    rect12.Y = rect1oPosY;
                    rect13.X = rect1oPosX;
                    rect13.Y = rect1oPosY;
                    rect2.X = rect2oPosX;
                    rect2.Y = rect2oPosY;

                    stateTimer += speed;
                    stack1.Clear();
                    stack2.Clear();
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
                    int temp = gameStatus;
                    gameStatus = -1;
                    state = State.START;
                    return temp;
            }
            return -1;
        }
    }
}
