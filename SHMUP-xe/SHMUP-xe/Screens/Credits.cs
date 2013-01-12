using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    class Credits : Screen
    {
        Texture2D blackground;
        Background background;

        List<String> Names = new List<String>();
        List<String> Contribution = new List<String>();
        List<String> Website = new List<String>();

        float strHeight;

        bool loaded;
        public Credits()
        {
            loaded = false;
        }

        public override void Load()
        {
            blackground = new Texture2D(Game1.graphics.GraphicsDevice, 32, 32, false, SurfaceFormat.Color);

            Color[] pixels = new Color[blackground.Width * blackground.Height];

            for (int y = 0; y < blackground.Height; y++)
            {
                for (int x = 0; x < blackground.Width; x++)
                {
                    pixels[y * blackground.Width + x] = Color.Black;
                }
            }

            blackground.SetData<Color>(pixels);

            background = new Background(new Vector4(0.3f, 0.3f, 0.1f, 0.1f), new Vector4(0.8f, 0.8f, 0.4f, 0.25f), false);

            //me
            Names.Add("Aaron Charcoal Styles");
            Contribution.Add("Design, Programming & Art");
            Website.Add("charcoalstyles.com");
            //Mat
            Names.Add("Mat Teubert");
            Contribution.Add("Awards Art");
            Website.Add(" ");
            //Bart
            Names.Add("Bart Klepka");
            Contribution.Add("Main Menu & Boss Music");
            Website.Add("bartklepka.com");
            //Bart
            Names.Add("Bård Ericson (Multifaros)");
            Contribution.Add("In-Game Music");
            Website.Add("multifaros.info.se");
            //Sinnix
            Names.Add("Jeff 'Sinnix' Jenkins");
            Contribution.Add("XBox 360 Button Pack");
            Website.Add("360prophecy.com");

            //Thanks1
            Names.Add("Thanks To:");
            Contribution.Add("My Girlfriend, Geraldine");
            Website.Add("Gayle and Johnee");
            //Thanks2
            Names.Add("The teams at GDC & GDC China");
            Contribution.Add("Brian and Larry at Impulse");
            Website.Add("Everyone at Infinite Interactive");
            //thanks 3
            Names.Add("Alexander Bishop and Family");
            Contribution.Add("All my friends and family");
            Website.Add("Morgan Pretty and Simon Riley");
            //thanks 4
            Names.Add("My Grandfather and Tim Richards");
            Contribution.Add("And all the playtesters");
            Website.Add("");

            strHeight = Game1.consoleFont.MeasureString("WWWWW").Y / Game1.scrmgr.screenSize.Y;

            loaded = true;
        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        public override void Update(GameTime gameTime)
        {
            background.Update();

            if (Game1.inpmgr.playerOneInput.Back == expButtonState.Pressed ||
            Game1.inpmgr.playerOneInput.B == expButtonState.Pressed)
            {
#if WINDOWS
                Game1.scrmgr.changeScreen(new Screens.MainMenuScreen(true));
#elif XBOX
                Game1.scrmgr.changeScreen(new Screens.XBMainMenuScreen(true));
#endif
            }
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            background.Draw(gametime, spriteBatch);

            for (int i = 0; i < 5; i++)
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Names[i], new Vector2(0.1f, 0.1f + (strHeight * 3.5f * i) + (strHeight * 1)), 0.75f, justification.left);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Contribution[i], new Vector2(0.1f, 0.1f + (strHeight * 3.5f * i) + (strHeight * 1.8f)), 0.75f, justification.left);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Website[i], new Vector2(0.1f, 0.1f + (strHeight * 3.5f * i) + (strHeight * 2.6f)), 0.75f, justification.left);
            }
            for (int i = 5; i < Names.Count; i++)
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Names[i], new Vector2(0.9f, 0.1f + (strHeight * 2.6f * (i - 5)) + (strHeight * 1)), 0.75f, justification.right);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Contribution[i], new Vector2(0.9f, 0.1f + (strHeight * 2.6f * (i - 5)) + (strHeight * 1.8f)), 0.75f, justification.right);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Website[i], new Vector2(0.9f, 0.1f + (strHeight * 2.6f * (i - 5)) + (strHeight * 2.6f)), 0.75f, justification.right);
            }

            Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.MainMenu, new Vector2(0.12f, 0.9f), justification.left);
           
        }
    }
}
