using GameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using UIControls;

namespace MathDemo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pixel;
        SpriteFont fontington;
        int currentScreenWidth, currentScreenHeight;
        ScreenManager manager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            currentScreenWidth = 1024;
            currentScreenHeight = 768;
            graphics.PreferredBackBufferWidth = currentScreenWidth;
            graphics.PreferredBackBufferHeight = currentScreenHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            //TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 50);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width != currentScreenWidth || Window.ClientBounds.Height != currentScreenHeight)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                currentScreenHeight = Window.ClientBounds.Height;
                currentScreenWidth = Window.ClientBounds.Width;
                graphics.ApplyChanges();    // note this will also trigger the clientSizeChanged event!
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            fontington = Content.Load<SpriteFont>("fontington");
            DrawText.Initialize(spriteBatch, graphics);
            manager = new ScreenManager(spriteBatch, Content, graphics, Window, currentScreenWidth, currentScreenHeight);
            manager.defaultFont = Content.Load<SpriteFont>("fontington");
            manager.defaultFont.Spacing = 0;
            // Add game screens here.  The first one to be added will be the active screen.
            manager.Add(new MenuScreen(), "MainMenu");
            manager.Add(new WorldMatrixDemo(), "WorldMatrixDemo");
            manager.Add(new IntegrationDemo(), "IntegrationDemo");
            manager.Add(new InterpolationDemo(), "InterpolationDemo");
            manager.Add(new OBBdemo(), "OBBDemo");
            manager.InitializeAllScreens();
            Window.Position = new Point(0, 0);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            manager.Update(gameTime);
            if (manager.shutDown) this.Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            manager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
