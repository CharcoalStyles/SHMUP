using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    public class LoadingScreen
    {
        float counter = 0;
        Vector2 txtpos;

        Texture2D black;

        public LoadingScreen()
        {
        }

        public void Load()
        {

            black = new Texture2D(Game1.graphics.GraphicsDevice, 32, 32, false, SurfaceFormat.Color);

            Color[] pixels = new Color[black.Width * black.Height];

            for (int y = 0; y < black.Height; y++)
            {
                for (int x = 0; x < black.Width; x++)
                {
                    pixels[y * black.Width + x] = Color.Black;

                }
            }

            black.SetData<Color>(pixels);
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.scrmgr.screenState == ScreenState.loadScreen)
            {
                counter += 0.05f;
                txtpos = new Vector2(300 + (float)Math.Sin(counter) * 100, 300 + (float)Math.Cos(counter) * 100);
            }
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Color rendercolor)
        {
            spriteBatch.Draw(black, new Rectangle(0, 0, 2000, 2000), rendercolor);

            //if (Game1.scrmgr.screenState == ScreenState.loadScreen)
            //{
            //    spriteBatch.DrawString(Game1.debugFont, "LOADING SCREEN", txtpos, rendercolor);
            //}
        }
    }
}