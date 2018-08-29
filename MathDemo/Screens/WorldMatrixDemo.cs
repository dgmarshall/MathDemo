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
    public class WorldMatrixDemo : Screen
    {
        Texture2D testImage;
        Texture2D pixel;
        Vector2[] corners = new Vector2[4];
        Vector2 position;
        float rotation;
        float scale = 1;
        Matrix translateMat, rotateMat, scaleMat, worldMat;
        SpriteFont fontington;

        public WorldMatrixDemo()
        {
        }

        public override void Initialize(ScreenManager scr)
        {
            base.Initialize(scr);
            using (var f = new FileStream(@"Content\testImage.png", FileMode.Open))
                testImage = Texture2D.FromStream(graphics.GraphicsDevice, f);
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            corners[0] = new Vector2(0, 0);
            corners[1] = new Vector2(testImage.Width, 0);
            corners[2] = new Vector2(0, testImage.Height);
            corners[3] = new Vector2(testImage.Width, testImage.Height);
            fontington = Content.Load<SpriteFont>("fontington");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) position.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) position.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up)) position.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) position.Y += 1;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) rotation += 0.05f;
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) rotation -= 0.05f;
                if (Keyboard.GetState().IsKeyDown(Keys.Up)) scale += 0.1f;
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) scale -= 0.1f;
            }
            translateMat = Matrix.CreateTranslation(new Vector3(position,0));
            rotateMat = Matrix.CreateRotationZ(rotation);
            scaleMat = Matrix.CreateScale(scale);
            worldMat = rotateMat * scaleMat * translateMat;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) manager.GoToScreen("MainMenu");
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawUI();
            spriteBatch.Draw(testImage, position, null, Color.White, rotation, Vector2.Zero,scale, SpriteEffects.None, 0);
            Vector2[] renderCorners = new Vector2[4];
            for(int i = 0; i<4; i++)
            {
                renderCorners[i] = Vector2.Transform(corners[i], worldMat);
            }
            DrawRectangle(renderCorners);
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
            spriteBatch.DrawString(fontington, "Position: " + position.X + ", " + position.Y,
                new Vector2(left + 10, top + (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Rotation: " + rotation.ToString("0.#") + " Radians (" + MathHelper.ToDegrees(rotation).ToString("0.#") + " degrees)",
                new Vector2(left + 10, top + 2 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Scale: " + (scale * 100f).ToString("0.#") + "%",
                new Vector2(left + 10, top + 3 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Translation matrix: ",
                new Vector2(left + 10, top + 4 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            DrawMatrixText(translateMat, left + 150, right, 4 * (bottom - top) / 30, 8 * (bottom - top) / 30, textScale);
            spriteBatch.DrawString(fontington, "Rotation matrix: ",
                new Vector2(left + 10, top + 9 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            DrawMatrixText(rotateMat, left + 150, right, 9 * (bottom - top) / 30, 13 * (bottom - top) / 30, textScale);
            spriteBatch.DrawString(fontington, "Scale matrix: ",
                new Vector2(left + 10, top + 14 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            DrawMatrixText(scaleMat, left + 150, right, 14 * (bottom - top) / 30, 18 * (bottom - top) / 30, textScale);
            spriteBatch.DrawString(fontington, "World matrix: ",
                new Vector2(left + 10, top + 19 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            DrawMatrixText(worldMat, left + 150, right, 19 * (bottom - top) / 30, 23 * (bottom - top) / 30, textScale);
        }

        private void DrawMatrixText(Matrix mat, int left, int right, int top, int bottom, float textScale)
        {
            string f = "##0.#";
            int p = 4;
            spriteBatch.DrawString(fontington, " " + mat.M11.ToString(f).PadLeft(p) + " | " + mat.M12.ToString(f).PadLeft(p) + " | " + mat.M13.ToString(f).PadLeft(p) + " | " + mat.M14.ToString(f).PadLeft(p),
                new Vector2(left, top + 0 * (bottom - top) / 4), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, " " + mat.M21.ToString(f).PadLeft(p) + " | " + mat.M22.ToString(f).PadLeft(p) + " | " + mat.M23.ToString(f).PadLeft(p) + " | " + mat.M24.ToString(f).PadLeft(p),
                new Vector2(left, top + 1 * (bottom - top) / 4), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, " " + mat.M31.ToString(f).PadLeft(p) + " | " + mat.M32.ToString(f).PadLeft(p) + " | " + mat.M33.ToString(f).PadLeft(p) + " | " + mat.M34.ToString(f).PadLeft(p),
                new Vector2(left, top + 2 * (bottom - top) / 4), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, " " + mat.M41.ToString(f).PadLeft(p) + " | " + mat.M42.ToString(f).PadLeft(p) + " | " + mat.M43.ToString(f).PadLeft(p) + " | " + mat.M44.ToString(f).PadLeft(p),
                new Vector2(left, top + 3 * (bottom - top) / 4), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
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
