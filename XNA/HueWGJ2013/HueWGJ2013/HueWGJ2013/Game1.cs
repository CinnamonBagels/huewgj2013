using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Minigame = HueWGJ2013.minigames.AMinigame;
using HueWGJ2013.minigames;
using System.Reflection;

namespace HueWGJ2013
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    public class Game1 : Microsoft.Xna.Framework.Game
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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState ks;
        MouseState ms;

        Texture2D characters;
        Animation character1;
        Animation character2;
        Animation character3;
        Animation character4;

        SpriteFont defaultFont;
        SpriteFont instructionFont;

        Hashtable mg = new Hashtable();
        List<string> games = new List<string>();
        string curGame = "minigame_Example";
        int gamesPlayed = 0;
        int curGameNum = 0;
        Random rand = null;
        string temp = "";
        int tempVal = 0;
        List<int> playerScore;
        int currentPlayer = 0;
        int numPlayers = -1;
        int winner = 0;
        bool candidate = false;

        State state = State.START;
        protected float stateTimer;              //Timer for current state
        protected float gameIntroTimer = 5.0f;
        protected float gamePlayTimer = 15.0f;
        protected float gameEndTimer = 5.0f;

        public static float timer = 0.0f;
        public static float speed = 0.05f;
        public static HueGraphics hueGraphics;

        public static List<Color> predefinedColors = new List<Color>();
        public bool escDown = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "HueWGJ2013Content";

            // Get all of the public static properties
            System.Reflection.PropertyInfo[] properties = typeof(Color).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (System.Reflection.PropertyInfo propertyInfo in properties)
            {
                // Check to make sure the property has a get method, and returns type "Color"
                if (propertyInfo.GetGetMethod() != null && propertyInfo.PropertyType == typeof(Color))
                {
                    // Get the color returned by the property by invoking it
                    Color color = (Color)propertyInfo.GetValue(null, null);
                    predefinedColors.Add(color);
                }
            }

            // MANDATORY
            mg["Menu"] = new Menu(Content);

            games.Add("Pear");
            mg["Pear"] = new Pear(Content);
            games.Add("HotAir");
            mg["HotAir"] = new HotAir(Content);
            games.Add("Trillionaire");
            mg["Trillionaire"] = new Trillionaire(Content);
            games.Add("Foursquare");
            mg["Foursquare"] = new Foursquare(Content);
            games.Add("BelAir");
            mg["BelAir"] = new BelAir(Content);
            games.Add("GrowDownThere");
            mg["GrowDownThere"] = new GrowDownThere(Content);
            games.Add("Hare");
            mg["Hare"] = new Hare(Content);
            games.Add("FightABear");
            mg["FightABear"] = new FightABear(Content);
            games.Add("Mare");
            mg["Mare"] = new Mare(Content);
            games.Add("Fair");
            mg["Fair"] = new Fair(Content);

            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            this.Window.Title = "GrowioWare - Team Hue 2013";

            this.IsMouseVisible = true;
            rand = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < numPlayers; i++)
            {
                playerScore[i] = 0;
            }
            speed = 0.05f;
            curGameNum = games.Count;
            curGame = newGame();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("defaultFont");
            instructionFont = Content.Load<SpriteFont>("instructionFont");
            hueGraphics = new HueGraphics(GraphicsDevice, instructionFont, spriteBatch);
            foreach (DictionaryEntry minigame in mg)
            {
                ((Minigame)minigame.Value).load(defaultFont);
            }
            // Create a new SpriteBatch, which can be used to draw textures.


            // TODO: use this.Content to load your game content here
            characters = Content.Load<Texture2D>("characters");
            character1 = new Animation(characters, 1, 4, 1);
            character2 = new Animation(characters, 1, 4, 1);
            character3 = new Animation(characters, 1, 4, 1);
            character4 = new Animation(characters, 1, 4, 1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timer++;
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            if (!escDown && ks.IsKeyDown(Keys.Escape))
            {
                if (numPlayers == -1)
                    Exit();
                else
                {
                    escDown = true;
                    ((Minigame)mg[curGame]).kill(ks, ms);
                    currentPlayer = 0;
                    numPlayers = -1;
                    state = State.START;
                }
            }
            else if (ks.IsKeyUp(Keys.Escape))
            {
                escDown = false;
            }

            switch (state)
            {
                case State.START:
                    Initialize();

                    winner = 0;
                    stateTimer = 0.0f;
                    currentPlayer = 0;

                    state = State.PLAY;
                    break;
                case State.PLAY:
                    if (numPlayers == -1)
                    {
                        tempVal = ((Minigame)mg["Menu"]).update(ks, ms);

                        if (tempVal > 0)
                        {
                            numPlayers = tempVal;
                            stateTimer = 0.0f;

                            playerScore = new List<int>();
                            for (int i = 0; i < numPlayers; i++)
                            {
                                playerScore.Add(0);
                            }
                        }
                    }
                    else
                    {
                        tempVal = ((Minigame)mg[curGame]).update(ks, ms);
                        if (tempVal >= 0)
                        {
                            if (tempVal == 1)
                                playerScore[currentPlayer]++;

                            if (gamesPlayed <= 20)
                            {
                                speed *= 1.025f;
                                gamesPlayed = 0;
                            }

                            currentPlayer++;
                            if (currentPlayer >= numPlayers)
                            {
                                // choose winner?
                                for (int j = 0; j < numPlayers; j++)
                                {
                                    if (playerScore[j] >= 5)
                                    {
                                        candidate = true;
                                        for (int i = 0; i < numPlayers; i++)
                                        {
                                            if (playerScore[j] <= playerScore[i] && j != i)
                                            {
                                                candidate = false;
                                                break;
                                            }
                                        }
                                        if (candidate)
                                        {
                                            winner = j + 1;
                                            stateTimer = 0.0f;
                                            state = State.WIN;
                                        }
                                    }
                                }
                                currentPlayer = 0;
                            }

                            if (state != State.WIN)
                            {
                                curGame = newGame();
                                ((Minigame)mg[curGame]).init();
                            }
                        }
                    }
                    break;
                case State.WIN:
                    stateTimer += speed;
                    currentPlayer = winner - 1;
                    if (stateTimer >= gameEndTimer * 5.0f)
                    {
                        numPlayers = -1;
                        currentPlayer = 0;
                        stateTimer = 0.0f;
                        state = State.START;
                    }
                    break;
                default:
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            spriteBatch.Begin();

            if (numPlayers == -1)
            {
                ((Minigame)mg["Menu"]).draw(spriteBatch);
            }
            else
            {

                ((Minigame)mg[curGame]).draw(spriteBatch);
            }

            if (numPlayers >= 1)
            {
                character1.updateRenderLocation(new Vector2(25.0f, 618.0f));
            }
            if (numPlayers >= 2)
            {
                character2.updateRenderLocation(new Vector2(125.0f, 618.0f));
            }
            if (numPlayers >= 3)
            {
                character3.updateRenderLocation(new Vector2(225.0f, 618.0f));
            }
            if (numPlayers >= 4)
            {
                character4.updateRenderLocation(new Vector2(325.0f, 618.0f));
            }

            switch (currentPlayer)
            {
                case 0:
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character1.getBorder(), Color.Maroon);
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character1.getBoundingBox(), Color.Red);
                    break;
                case 1:
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character2.getBorder(), Color.MediumVioletRed);
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character2.getBoundingBox(), Color.Pink);
                    break;
                case 2:
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character3.getBorder(), Color.Green);
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character3.getBoundingBox(), Color.Lime);
                    break;
                case 3:
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character4.getBorder(), Color.Brown);
                    spriteBatch.Draw(hueGraphics.getSolidTexture(), character4.getBoundingBox(), Color.Orange);
                    break;
            }
            // Current Player Display
            //spriteBatch.DrawString(defaultFont, "Current Player " + currentPlayer, new Vector2(25.0f - 1.0f, 453.0f), Color.White);

            //Score Display
            //if (numPlayers > 0)
            //{
            //    for (int i = 0; i < playerScore.Count; i++)
            //    {
            //        spriteBatch.DrawString(defaultFont, "Player " + i + ": " + playerScore[i], new Vector2(25.0f - 1.0f, 503.0f + i * 25.0f), Color.Black);
            //        spriteBatch.DrawString(defaultFont, "Player " + i + ": " + playerScore[i], new Vector2(25.0f + 1.0f, 503.0f + i * 25.0f), Color.Black);
            //        spriteBatch.DrawString(defaultFont, "Player " + i + ": " + playerScore[i], new Vector2(25.0f, 503.0f + i * 25.0f - 1.0f), Color.Black);
            //        spriteBatch.DrawString(defaultFont, "Player " + i + ": " + playerScore[i], new Vector2(25.0f, 503.0f + i * 25.0f + 1.0f), Color.Black);
            //        spriteBatch.DrawString(defaultFont, "Player " + i + ": " + playerScore[i], new Vector2(25.0f, 503.0f + i * 25.0f), Color.White);
            //    }
            //}

            if (numPlayers >= 1)
            {
                character1.goToFrame(0);
                character1.draw(spriteBatch, new Vector2(25.0f, 618.0f));
            }
            if (numPlayers >= 2)
            {
                character2.goToFrame(1);
                character2.draw(spriteBatch, new Vector2(125.0f, 618.0f));
            }
            if (numPlayers >= 3)
            {
                character3.goToFrame(2);
                character3.draw(spriteBatch, new Vector2(225.0f, 618.0f));
            }
            if (numPlayers >= 4)
            {
                character4.goToFrame(3);
                character4.draw(spriteBatch, new Vector2(325.0f, 618.0f));
            }

            switch (state)
            {
                case State.WIN:
                    Game1.hueGraphics.drawInstructionText((winner==1?"Ferbrache":winner==2?"Gaybrache":winner==3?"Zhukeeper":"Philtato") + " wins!");
                    break;
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected string newGame()
        {
            curGameNum++;
            if (curGameNum >= games.Count)
            {
                for (int i = 0; i < games.Count; i++)
                {
                    tempVal = rand.Next(games.Count);
                    temp = games[tempVal];
                    games[tempVal] = games[i];
                    games[i] = temp;
                }
                curGameNum = 0;
            }
            return games[curGameNum];
        }
    }
}
