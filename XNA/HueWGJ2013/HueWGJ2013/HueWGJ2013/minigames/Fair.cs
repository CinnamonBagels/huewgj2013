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

        static int rect1oPosX = 256;
        static int rect1oPosY = 384;
        static int rect2oPosX = 768;
        static int rect2oPosY = 384;

        Rectangle rect11 = new Rectangle(rect1oPosX, rect1oPosY, 193, 266);
        Rectangle rect12 = new Rectangle(rect1oPosX, rect1oPosY, 193, 266);
        Rectangle rect13 = new Rectangle(rect1oPosX, rect1oPosY, 193, 266);
        Rectangle rect2 = new Rectangle(rect2oPosX, rect2oPosY, 193, 266);

        Vector2 pos = new Vector2(50, 50);
        Vector2 pos2 = new Vector2(25, 25);
        Vector2 pos3 = new Vector2(200, 25);

        List<object> stack1 = new List<object>();
        List<object> stack2 = new List<object>();

        public Fair(ContentManager c)
            : base(c)
        {
            this.Content = c;
        }

        public override void load(SpriteFont font)
        {
            img_card = Content.Load<Texture2D>("minigames/Fair/card");
            this.font = font;
        }

        public override void draw(SpriteBatch sb)
        {
            sb.DrawString(font, "Make it Fair", pos2, Color.Red);
            sb.DrawString(font, "" + stateTimer, pos3, Color.Red);

            switch (state)
            {
                case State.START:
                    break;
                case State.INTRO:
                    sb.DrawString(font, "Intro", pos, Color.Red);
                    break;
                case State.PLAY:
                    sb.Draw(img_card, rect11, Color.White);
                    sb.Draw(img_card, rect12, Color.White);
                    sb.Draw(img_card, rect13, Color.White);
                    sb.Draw(img_card, rect2, Color.White);
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

            switch (state)
            {
                case State.START:
                    gameStatus = -1;
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
    }
}
