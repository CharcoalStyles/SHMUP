using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    class GameOverScreen : Screen
    {
        bool loaded;

        float counter = 0;

        Texture2D blackground;
        Background background;

        bool goodGO;

        int inScore = 0;

        bool isHighScore;

        bool mainGame = false;

        //menu
        enum menuSelect
        {
            mainMenu,
            none
        }

        public GameOverScreen(int inS, bool isGoodGO)
        {
            loaded = false;

            inScore = inS;

            goodGO = isGoodGO;

            if (Game1.gammgr.levelSequence.Count == 15)
            {
                mainGame = true;
            }

            Game1.gammgr.Clear(true);
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

            if (goodGO)
            {
                //Green for good finish
                background = new Background(new Vector4(0.2f, 0.65f, 0.2f, 0.25f), new Vector4(0.1f, 0.75f, 0.2f, 0.25f), false);
            }
            else
            {
                //Red for Bad Finish
                background = new Background(new Vector4(0.65f, 0.2f, 0.2f, 0.25f), new Vector4(0.75f, 0.1f, 0.2f, 0.25f), false);
            }

            isHighScore = HighScoreManager.CheckHighScore(inScore, Game1.gammgr.currentPlayer);

            loaded = true;
        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        public override void Update(GameTime gameTime)
        {
            background.Update();
            counter++;
            if (counter == 20)
            {
                if (inScore > AwardsManager.currentPlayerAwards.stats[(int)AwardsManager.stats.highScore])
                {
                    AwardsManager.currentPlayerAwards.stats[(int)AwardsManager.stats.highScore] = inScore;
                    AwardsManager.SaveAwards(Game1.gammgr.currentPlayer);
                }
            }

            if (Game1.inpmgr.playerOneInput.Start == expButtonState.Pressed ||
            Game1.inpmgr.playerOneInput.A == expButtonState.Pressed)
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
            spriteBatch.Draw(blackground, new Rectangle(0, 0, 2000, 2000), Color.White);
            
            background.Draw(gametime, spriteBatch);

            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.GameOver, new Vector2(0.5f, 0.2f), 1.5f, justification.centre);
            
            if (goodGO)
            {
                if (mainGame) 
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.orgFin, new Vector2(0.5f, 0.24f), 0.9f, justification.centre);
                else
                    if (Game1.IS_DEMO)
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.DemoFinished, new Vector2(0.5f, 0.24f), 0.9f, justification.centre);
                    else
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SeqFin, new Vector2(0.5f, 0.24f), 0.9f, justification.centre);
            }

            if (isHighScore)
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.NewHighScore, new Vector2(0.5f, 0.4f), 0.9f + ((float)Math.Cos(counter / 30) * 0.15f), justification.centre);
            }
            else
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.YourScore, new Vector2(0.5f, 0.4f), 1.2f, justification.centre);
            }
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, inScore.ToString(), new Vector2(0.5f, 0.45f), 1.2f, justification.centre);

            //input guide
            Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.MainMenu, new Vector2(0.12f, 0.9f), justification.left);
        }
    }
}