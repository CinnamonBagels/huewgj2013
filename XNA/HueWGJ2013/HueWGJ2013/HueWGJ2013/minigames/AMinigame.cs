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
    public abstract class AMinigame
    {
        public enum State
        {
            PLAY,
            WIN,
            LOSE,
            INTRO,
            PAUSE,
            START,
            EXIT
        }

        protected String name = "Default Name";   //Name
        protected State state = State.START;      //What is the state of the game? (for draw, update function)
        protected int gameLength = 0;             //How long does the minigame last (in frames)?
        protected ContentManager Content;
        protected SpriteFont font;
        protected float timer;                   //Should not exceed gameLength
        protected float stateTimer;              //Timer for current state
        protected float gameIntroTimer = 10.0f;
        protected float gamePlayTimer = 20.0f;
        protected float gameEndTimer = 5.0f;
        protected float speed;
        protected int gameStatus = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public AMinigame(ContentManager c)
        {
            this.Content = c;
        }

        /// <summary>
        /// Init for game start
        /// </summary>
        public void init()
        {
            this.gameStatus = -1;
            this.stateTimer = 0.0f;
            state = State.START;
        }
        /// <summary>
        /// Loading for resources (should be called at the start of the game)
        /// </summary>
        public abstract void load(SpriteFont font);
        /// <summary>
        /// What to draw based on the state variable
        /// </summary>
        public abstract void draw(SpriteBatch sb);
        /// <summary>
        /// What to do during an update based on the state variable
        /// </summary>
        public abstract int update(KeyboardState kb, MouseState ms);
        /// <summary>
        /// Helper method to generate random numbers with given parameters
        /// </summary>
        public float randomGen(Random rand, float range, float offset)
        {
            return ((float)rand.Next((int)range)) - offset;
        }

        public void kill(KeyboardState kb, MouseState ms)
        {
            stateTimer = 0.0f;
            state = State.EXIT;
            this.update(kb, ms);
            this.init();
        }
    }
}
