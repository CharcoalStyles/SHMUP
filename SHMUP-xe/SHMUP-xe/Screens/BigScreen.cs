using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    class BigScreen:Screen
    {
        bool loaded;

        Texture2D bkgrnd;
        Texture2D sprite;

        Random r = new Random();

        public BigScreen()
        {
            loaded = false;
        }

        public override void Load()
        {
            bkgrnd = new Texture2D(Game1.graphics.GraphicsDevice, Game1.graphics.GraphicsDevice.DisplayMode.Width, Game1.graphics.GraphicsDevice.DisplayMode.Height,
                false, SurfaceFormat.Color);

            Color[] pixels = new Color[bkgrnd.Width * bkgrnd.Height];

            for (int y = 0; y < bkgrnd.Height; y++)
            {
                for (int x = 0; x < bkgrnd.Width; x++)
                {
                    if (r.Next(0, 100) > 50)
                    {
                        pixels[y * bkgrnd.Width + x] = Color.Black;
                    }
                    else
                    {
                        pixels[y * bkgrnd.Width + x] = Color.Gray;
                    }

                }
            }

            bkgrnd.SetData<Color>(pixels);


            sprite = new Texture2D(Game1.graphics.GraphicsDevice, 64, 64,
                false, SurfaceFormat.Color);

            pixels = new Color[sprite.Width * sprite.Height];

            for (int y = 0; y < sprite.Height; y++)
            {
                for (int x = 0; x < sprite.Width; x++)
                {
                    if (Math.Sin(x*y) > 0)
                    {
                        pixels[y * sprite.Width + x] = Color.DarkMagenta;
                    }
                    else
                    {
                        pixels[y * sprite.Width + x] = Color.Aqua;
                    }

                }
            }

            sprite.SetData<Color>(pixels);

            loaded = true;
        }

        public override bool IsLoaded()
        {
            return loaded;
        }
        string output = "";
        Vector2 pos = new Vector2(0.5f, 0.5f);
        float maxspeed = 0.005f;
        public override void Update(GameTime gameTime)
        {
            output = "Test";
            pos += (Game1.inpmgr.playerOneInput.leftStick * maxspeed);
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(black, new Vector2(0,0), Color.White);
            Game1.scrmgr.drawTexture(spriteBatch, bkgrnd, new Vector2(0.5f, 0.5f), Color.White);
            //spriteBatch.DrawString(Game1.debugFont, output, new Vector2(2, 2), Color.Black);
            //spriteBatch.DrawString(Game1.debugFont, output, new Vector2(3, 3), Color.White);
            Game1.scrmgr.drawString(spriteBatch, Game1.debugFont, output, new Vector2(0.03f, 0.03f), justification.centre);


            Game1.scrmgr.drawTexture(spriteBatch, sprite, pos, Color.White);
        }

      
    }
}