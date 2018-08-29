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
    public class InterpolationDemo : Screen
    {
        Texture2D testImage;
        Texture2D pixel;
        Vector2[] position = new Vector2[3];
        SpriteFont fontington;

        public InterpolationDemo()
        {
        }

        public override void Initialize(ScreenManager scr)
        {
            base.Initialize(scr);
            using (var f = new FileStream(@"Content\testImage.png", FileMode.Open))
                testImage = Texture2D.FromStream(graphics.GraphicsDevice, f);
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            fontington = Content.Load<SpriteFont>("fontington");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) manager.GoToScreen("MainMenu");
        }

        float LERP(float start, float end, float interpAmt)
        {
            return start * (1 - interpAmt) + end * interpAmt;
        }

        float CosInterp(float start, float end, float interpAmt)
        {
            float interpCos = (float)(0.5 * Math.Cos(interpAmt * Math.PI) + 0.5);
            return start * (1 - interpCos) + end * interpCos;
        }

        float CubicInterp(float start, float end, float interpAmt)
        {
            float interpCubic =
                -2 * interpAmt * interpAmt * interpAmt
                + 3 * interpAmt * interpAmt;
            return start * (1 - interpCubic) + end * interpCubic;
        }

        float top = 50, bottom = 250, intp = 0, dir = 0.01f;

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawUI();
            intp += dir;
            if (intp >= 1 || intp<=0) dir *= -1;
            position[0] = new Vector2(50, LERP(top, bottom, intp));
            position[1] = new Vector2(150, CosInterp(top, bottom, 1-intp));
            position[2] = new Vector2(250, CubicInterp(top, bottom, intp));
            for (int i=0; i<3; i++)
                spriteBatch.Draw(testImage, position[i], null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
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
            spriteBatch.DrawString(fontington, "Position A: " + position[0].X + ", " + position[0].Y.ToString("n0") + " (LERP)",
                new Vector2(left + 10, top + (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Position B: " + position[1].X + ", " + position[1].Y.ToString("n0") + " (Cos Interp)",
                new Vector2(left + 10, top + (bottom - top) * 2 / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Position C: " + position[2].X + ", " + position[2].Y.ToString("n0") + " (Cubic Interp)",
                new Vector2(left + 10, top + (bottom - top) * 3 / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
        }

        private void DrawRectangle(Vector2[] vectors)
        {
                DrawLine(vectors[0], vectors[1]);
                DrawLine(vectors[1], vectors[3]);
                DrawLine(vectors[3], vectors[2]);
                DrawLine(vectors[2], vectors[0]);
        }

        private void DrawLine(Vector2 v1, Vector2 v2)
        {
            float angle;
            Vector2 distance;
            distance = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
            angle = (float)Math.Atan2(distance.Y, distance.X);
            spriteBatch.Draw(pixel, new Rectangle((int)v1.X, (int)v1.Y,
                (int)distance.Length(), 1),
                null, Color.Black, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
