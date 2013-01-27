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

namespace HueWGJ2013
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState ks;
        MouseState ms;

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
        int numPlayers = 5;

        public static float timer = 0.0f;
        public static float speed = 0.05f;
        public static HueGraphics hueGraphics;

        public Game1()
        {            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "HueWGJ2013Content";
            games.Add("Pear");
            mg["Pear"] = new Pear(Content);
            games.Add("HotAir");
            mg["HotAir"] = new HotAir(Content);
            games.Add("Trillionaire");
            mg["Trillionaire"] = new Trillionaire(Content);
            games.Add("BelAir");
            mg["BelAir"] = new BelAir(Content);
            games.Add("GrowDownThere");
            mg["GrowDownThere"] = new GrowDownThere(Content);
            games.Add("Hare");
            mg["Hare"] = new Hare(Content);

            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;

            this.IsMouseVisible = true;
            rand = new Random();

            playerScore = new List<int>();
            for (int i = 0; i < numPlayers; i++)
            {
                playerScore.Add(0);
            }

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
            hueGraphics = new HueGraphics(GraphicsDevice,instructionFont,spriteBatch);
            foreach (DictionaryEntry minigame in mg)
            {
                ((Minigame)minigame.Value).load(defaultFont);
            }
            // Create a new SpriteBatch, which can be used to draw textures.
            
            
            // TODO: use this.Content to load your game content here
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

            if (ks.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            tempVal = ((Minigame) mg[curGame]).update(ks, ms);
            if (tempVal >= 0)
            {
                if (tempVal == 1)
                    playerScore[currentPlayer]++;
                
                if (gamesPlayed <= 10)
                {
                    speed *= 1.025f;
                    gamesPlayed = 0;
                }

                currentPlayer++;
                if (currentPlayer >= numPlayers)
                    currentPlayer = 0;

                curGame = newGame();
                ((Minigame)mg[curGame]).init();
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
            GraphicsDevice.Clear(Color.White);
            
            spriteBatch.Begin();
            spriteBatch.DrawString(defaultFont, "Current player: " + (currentPlayer + 1), 
                    new Vector2(25.0f, 359.0f), Color.Red);
            for (int i = 0; i < playerScore.Count; i++)
            {
                spriteBatch.DrawString(defaultFont, "Player " + i + ": "
                    + playerScore[i], new Vector2(25.0f, 384.0f + i * 25.0f), Color.Red);
            }
            ((Minigame)mg[curGame]).draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected string newGame()
        {
            curGameNum++;
            if (curGameNum >= games.Count)
            {
                for (int i = 0; i < games.Count; i++ )
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
