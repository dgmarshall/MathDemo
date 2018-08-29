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
    public class IntegrationDemo : Screen
    {
        Texture2D testImageSmall;
        Texture2D pixel;
        Vector2 startPos = new Vector2(50, 500);
        Vector2 position;
        Vector2 velocity;
        Vector2 launchVelocity = new Vector2(8,-20);
        Vector2 gravity = new Vector2(0, 0.98f);
        bool playing = false;
        SpriteFont fontington;
        KeyboardState oldKeyState;
        float timer = 0;
        int msPerDemoFrame = 15;
        float frameNum = 0;
        List<Vector2> actualPath = new List<Vector2>();
        float frameMultiplier = 1;

        public IntegrationDemo()
        {
        }

        public override void Initialize(ScreenManager scr)
        {
            base.Initialize(scr);
            using( var f = new FileStream(@"Content\TestImageSmall.png", FileMode.Open))
                testImageSmall = Texture2D.FromStream(graphics.GraphicsDevice, f);
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            fontington = Content.Load<SpriteFont>("fontington");
        }

        public override void Start()
        {
            base.Start();
            position = startPos;
            velocity = Vector2.Zero;
            actualPath.Add(position);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
            {
                playing = !playing;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus) && oldKeyState.IsKeyUp(Keys.OemPlus))
            {
                advanceDemo(frameMultiplier);
            }
            if(playing)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > msPerDemoFrame)
                {
                    timer -= msPerDemoFrame;
                    advanceDemo(frameMultiplier);
                }
            }
            //if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
            //{
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && oldKeyState.IsKeyUp(Keys.Right))
            {
                if (frameMultiplier >= 1)
                    frameMultiplier += 1;
                else
                    frameMultiplier += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && oldKeyState.IsKeyUp(Keys.Left))
            {
                if (frameMultiplier > 1)
                    frameMultiplier -= 1;
                else
                    frameMultiplier -= 0.1f;
            }
            //    if (Keyboard.GetState().IsKeyDown(Keys.Up)) position.Y -= 1;
            //    if (Keyboard.GetState().IsKeyDown(Keys.Down)) position.Y += 1;
            //}
            //else
            //{
            //    if (Keyboard.GetState().IsKeyDown(Keys.Right)) rotation += 0.05f;
            //    if (Keyboard.GetState().IsKeyDown(Keys.Left)) rotation -= 0.05f;
            //    if (Keyboard.GetState().IsKeyDown(Keys.Up)) scale += 0.1f;
            //    if (Keyboard.GetState().IsKeyDown(Keys.Down)) scale -= 0.1f;
            //}
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) manager.GoToScreen("MainMenu");
            oldKeyState = Keyboard.GetState();
        }

        private void advanceDemo(float frames)
        {
            // integrate using Euler's method
            position += velocity * frames;
            velocity += gravity * frames;

            // advance the demo to the next timestep
            frameNum += frames;

            // reset the timestep if at the end of the curve
            if (position.Y > startPos.Y)
            {
                position.Y = startPos.Y;
                frameNum = 0;
                position = startPos;
                velocity = launchVelocity;
                actualPath.Clear();
            }
            actualPath.Add(position);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 1; i < actualPath.Count; i++)
            {
                DrawLine(actualPath[i - 1], actualPath[i]);
            }

            for (int i = 0; i < 200; i++ )
            {
                float t0 = (float)i * 0.2f;
                float t1 = (float)(i + 1) * 0.2f;
                Vector2 x0 = PredictPosition(t0, launchVelocity, startPos, gravity);
                Vector2 x1 = PredictPosition(t1, launchVelocity, startPos, gravity);
                DrawLine(x0, x1, Color.Gray);
            }

            DrawUI();
            spriteBatch.Draw(testImageSmall, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
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
            spriteBatch.DrawString(fontington, "Position (P): " + position.X + ", " + position.Y,
                new Vector2(left + 10, top + (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Velocity (v): " + velocity.X + ", " + velocity.Y,
                new Vector2(left + 10, top + 2 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Gravity (g): " + gravity.X + ", " + gravity.Y,
                new Vector2(left + 10, top + 3 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Frame number :" + (int)frameNum,
                new Vector2(left + 10, top + 4 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Milliseconds per frame: " + msPerDemoFrame,
                new Vector2(left + 10, top + 5 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(fontington, "Frame multiplier: " + frameMultiplier.ToString("##0.#").PadLeft(4),
                new Vector2(left + 10, top + 6 * (bottom - top) / 30), Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
        }

        Vector2 PredictPosition(float t, Vector2 v0, Vector2 x0, Vector2 a)
        {
            return a * (0.5f * t * t) + v0 * t + x0;
        }

        private void DrawLine(Vector2 v1, Vector2 v2, Color? colour = null)
        {
            if (colour == null) colour = (Color?)Color.Black;
            float angle;
            Vector2 distance;
            distance = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
            if (distance.Length() < 1) distance.Normalize();
            angle = (float)Math.Atan2(distance.Y, distance.X);
            spriteBatch.Draw(pixel, new Rectangle((int)v1.X, (int)v1.Y,
                (int)distance.Length(), 1),
                null, (Color)colour, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
