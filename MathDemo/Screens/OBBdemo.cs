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
using System.IO;

namespace MathDemo
{
    public class OBBdemo : Screen
    {
        Texture2D testImage;
        Vector2 Pos1 = new Vector2(50, 100);
        Vector2 Pos2 = new Vector2(250, 500);
        float rot1, rot2;
        SpriteFont fontington;
        Texture2D pixel;
        OBB2D p1box, p2box;

        public OBBdemo()
        {
        }

        public override void Initialize(ScreenManager scr)
        {
            base.Initialize(scr);
            using( var f = new FileStream(@"Content\TestImage.png", FileMode.Open))
                testImage = Texture2D.FromStream(graphics.GraphicsDevice, f);
            fontington = Content.Load<SpriteFont>("fontington");
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public override void Start()
        {
            base.Start();
            p1box = new OBB2D(Pos1, rot1, new Vector2(testImage.Width, testImage.Height));
            p2box = new OBB2D(Pos2, rot2, new Vector2(testImage.Width, testImage.Height));
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W)) Pos1.Y -= 2;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) Pos1.Y += 2;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) Pos1.X -= 2;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) Pos1.X += 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) rot1 -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.E)) rot1 += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) Pos2.Y -= 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) Pos2.Y += 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) Pos2.X -= 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) Pos2.X += 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Home)) rot2 -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.End)) rot2 += 0.1f;

            p1box.AxisAngle = rot1;
            p1box.origin = Pos1;
            p2box.AxisAngle = rot2;
            p2box.origin = Pos2;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();


            DrawUI();
            p1box.Draw(spriteBatch, pixel);
            p2box.Draw(spriteBatch, pixel);
            spriteBatch.Draw(testImage, Pos1, null, Color.White, rot1,
                new Vector2(testImage.Width / 2, testImage.Height / 2),
                1, SpriteEffects.None, 0);
            spriteBatch.Draw(testImage, Pos2, null, Color.White, rot2,
                new Vector2(testImage.Width / 2, testImage.Height / 2),
                1, SpriteEffects.None, 0);

            if (p1box.Intersects(p2box))
                spriteBatch.DrawString(fontington, "Collision", new Vector2(10, 10), Color.Black, 0, Vector2.Zero, 5, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawUI()
        {
            int left = (int)((float)Window.ClientBounds.Width * 0.6f);
            int right = Window.ClientBounds.Width;
            int top = 0;
            int bottom = Window.ClientBounds.Height;
            spriteBatch.Draw(pixel, new Rectangle(left, top, right - left, bottom - top), Color.Black);
            float textScale = 1;
            spriteBatch.DrawString(fontington, "Position (1): " + Pos1.X + ", " + Pos1.Y,
                new Vector2(left + 10, top + (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Rotation (1): " + rot1,
                new Vector2(left + 10, top + 2 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Position (2): " + Pos2.X + ", " + Pos2.Y,
                new Vector2(left + 10, top + 3 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Rotation (2): " + rot2,
                new Vector2(left + 10, top + 4 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
        }
    }
}
