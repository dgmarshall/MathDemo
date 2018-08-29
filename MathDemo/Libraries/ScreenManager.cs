////////////////////////////////////////////////////////////////////////////////
// ScreenManager library
// by David Marshall
// Last update: 23rd October 2014
//
// Classes in this library:
// Screen - base class for creating screen classes
// ScreenManager - class to handle initializing, drawing etc of active screen
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary
{
    public class Screen
    {
        protected ScreenManager manager;
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected GraphicsDeviceManager graphics;
        protected GameWindow Window;
        protected int screenWidth = 800;
        protected int screenHeight = 480;

        public virtual void Initialize(ScreenManager mgr)
        {
            if (mgr == null) throw new Exception("You must set up the screen manager before initializing screens!");
            manager = mgr;
            spriteBatch = manager.spriteBatch;
            Content = manager.content;
            graphics = manager.graphics;
            screenWidth = manager.screenWidth;
            screenHeight = manager.screenHeight;
            Window = manager.Window;
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void Start() { }
    }

    public class ScreenManager
    {
        public SpriteBatch spriteBatch;
        public ContentManager content;
        public GraphicsDeviceManager graphics;
        public GameWindow Window;

        Dictionary<string, Screen> screens = new Dictionary<string, Screen>();
        public Screen ActiveScreen;
        public int screenWidth = 800;
        public int screenHeight = 480;
        public SpriteFont defaultFont;
        public bool shutDown = false;
        public bool soundsOn = true;
        public bool musicOn = true;

        public ScreenManager(SpriteBatch batch, ContentManager contentMgr, GraphicsDeviceManager gdm, GameWindow win, int width, int height)
        {
            spriteBatch = batch;
            content = contentMgr;
            graphics = gdm;
            Window = win;
            screenWidth = width;
            screenHeight = height;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }

        public void GoToScreen(string name)
        {
            ActiveScreen = screens[name];
            ActiveScreen.Start();
        }

        public void Add(Screen ScreenToAdd, string name)
        {
            screens.Add(name, ScreenToAdd);
            if (ActiveScreen == null) ActiveScreen = ScreenToAdd;
        }

        public void InitializeAllScreens()
        {
            for(int i = 0; i<screens.Count(); i++)
                screens.ToArray()[i].Value.Initialize(this);
        }

        public void Update(GameTime gameTime)
        {
            if (ActiveScreen == null)
                throw new Exception("there is no active screen to update!");
            ActiveScreen.Update(gameTime);
        }

        public void DrawScreen(string name, GameTime gameTime)
        {
            screens[name].Draw(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (ActiveScreen == null)
                throw new Exception("there is no active screen to draw!");
            ActiveScreen.Draw(gameTime);
        }
    }
}
