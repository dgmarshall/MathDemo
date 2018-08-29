using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using UIControls;
using Microsoft.Xna.Framework.Media;

namespace MathDemo
{
    public class MenuScreen : Screen
    {
        ////////////////////////////////////////////////////////////////////////////////
        // DECLARATIONS
        ////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////
        // INITIALIZE THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Initialize(ScreenManager mgr)
        {
            base.Initialize(mgr);
        }

        ////////////////////////////////////////////////////////////////////////////////
        // UPDATE THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (KeyboardHelper.NewKeyPressed(Keys.D1, gameTime))
            {
                manager.GoToScreen("InterpolationDemo");
            }
            if (KeyboardHelper.NewKeyPressed(Keys.D2, gameTime))
            {
                manager.GoToScreen("OBBDemo");
            }
            if (KeyboardHelper.NewKeyPressed(Keys.D3, gameTime))
            {
                manager.GoToScreen("WorldMatrixDemo");
            }
            if (KeyboardHelper.NewKeyPressed(Keys.D4, gameTime))
            {
                manager.GoToScreen("IntegrationDemo");
            }
            if (KeyboardHelper.NewKeyPressed(Keys.Q, gameTime))
            {
                manager.shutDown = true;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        // DRAW THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            DrawText.Aligned("1: Interpolation Demo", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.1f, Color.White, manager.defaultFont);
            DrawText.Aligned("2: OBB Demo", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.2f, Color.White, manager.defaultFont);
            DrawText.Aligned("3: World Matrix Demo", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.3f, Color.White, manager.defaultFont);
            DrawText.Aligned("4: Integration Demo", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.4f, Color.White, manager.defaultFont);
            DrawText.Aligned("Q to quit!", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.6f, Color.White, manager.defaultFont);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
