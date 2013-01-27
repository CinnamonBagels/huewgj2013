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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState ks;
        MouseState ms;

        SpriteFont defaultFont;

        Hashtable mg = new Hashtable();
        List<string> games = new List<string>();
        string curGame = "minigame_Example";

        public static float timer = 0.0f;
        public static float speed = 0.05f;
        public static HueGraphics hueGraphics;

        public Game1()
        {            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "HueWGJ2013Content";
            //games.Add("Pear");
            //mg["Pear"] = new Pear(Content);
            //games.Add("HotAir");
            //mg["HotAir"] = new HotAir(Content);
            //games.Add("Trillionaire");
            //mg["Trillionaire"] = new Trillionaire(Content);
            //games.Add("BelAir");
            //mg["BelAir"] = new BelAir(Content);
            games.Add("Foursquare");
            mg["Foursquare"] = new Foursquare(Content);

            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;

            this.IsMouseVisible = true;
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
            curGame = newGame();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hueGraphics = new HueGraphics(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("defaultFont");
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

            if (((Minigame)mg[curGame]).update(ks, ms) != -1)
            {
                speed *= 1.05f;
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
            ((Minigame)mg[curGame]).draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected string newGame()
        {
            int size = games.Count;
            Random rand = new Random();
            return games[rand.Next(size)];
        }
    }
}
