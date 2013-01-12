using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    class AwardsStatsScreen : Screen
    {
        Texture2D blackground;
        Background background;

        bool loaded;

        int index = 0;

        int scrollCounter = 0;

        public AwardsStatsScreen()
        {
            loaded = false;
            
#if XBOX
            Game1.gammgr.playerShip.visible = false;
#endif
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

            background = new Background(new Vector4(0.2f, 0.2f, 0.4f, 0.25f), new Vector4(0.4f, 0.4f, 0.8f, 0.25f), false);

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

            if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
            {
                scrollCounter++;
            }
            else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
            {
                scrollCounter--;
            }

#if XBOX
            if (Game1.inpmgr.playerOneInput.leftStick.Y <= -0.5f)
            {
                scrollCounter++;
            }
            else
                if (Game1.inpmgr.playerOneInput.leftStick.Y >= 0.5f) 
            {
                scrollCounter--;
            }
#endif

            if (scrollCounter <= -1)
            {
                scrollCounter = 0;
                if (index + 9 != AwardsManager.currentPlayerAwards.awards.Count)
                    index++;
            }
            else if (scrollCounter >= 1)
            {
                scrollCounter = 0;
                if (index != 0)
                    index--;
            }
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            background.Draw(gametime, spriteBatch);

            String title = "";
            String desc = "";

            for (int i = 0; i < 9; i++)
            {
                switch ((AwardsManager.awards)(i + index))
                {
                    case AwardsManager.awards.Killed1k:
                        title = Strings.Killed1k;
                        desc = Strings.Killed1kDescription;
                        break;
                    case AwardsManager.awards.Killed5k:
                        title = Strings.Killed5k;
                        desc = Strings.Killed5kDescription;
                        break;
                    case AwardsManager.awards.Killed10k:
                        title = Strings.Killed10k;
                        desc = Strings.Killed10kDescription;
                        break;
                    case AwardsManager.awards.Killed100k:
                        title = Strings.Killed100k;
                        desc = Strings.Killed100kDescription;
                        break;
                    case AwardsManager.awards.Killed500k:
                        title = Strings.Killed500k;
                        desc = Strings.Killed500kDescription;
                        break;
                    case AwardsManager.awards.Total10mPoints:
                        title = Strings.Total10mPoints;
                        desc = Strings.Total10mPointsDescription;
                        break;
                    case AwardsManager.awards.Total50mPoints:
                        title = Strings.Total50mPoints;
                        desc = Strings.Total50mPointsDescription;
                        break;
                    case AwardsManager.awards.Total100mPoints:
                        title = Strings.Total100mPoints;
                        desc = Strings.Total100mPointsDescription;
                        break;
                    case AwardsManager.awards.MaxMultiplier:
                        title = Strings.AwdDefender;
                        desc = Strings.AwdDefenderDescription;
                        break;
                    case AwardsManager.awards.NoShipDied:
                        title = Strings.AwdShipNotDied;
                        desc = Strings.AwdShipNotDiedDesc;
                        break;
                    case AwardsManager.awards.NoShipHit:
                        title = Strings.AwdShipNotHit;
                        desc = Strings.AwdShipNotHitDesc;
                        break;
                    case AwardsManager.awards.ShieldNotHit:
                        title = Strings.AwdWallNotHIt;
                        desc = Strings.AwdWallNotHItDesc;
                        break;
                    case AwardsManager.awards.noMissiles:
                        title = Strings.AwdNoMiss;
                        desc = Strings.AwdNoMissDisc;
                        break;
                    case AwardsManager.awards.Total1kMissiles:
                        title = Strings.Shot1kMissiles;
                        desc = Strings.Shot1kMissilesDesc;
                        break;
                    case AwardsManager.awards.Total10kMissiles:
                        title = Strings.Shot10kMissiles;
                        desc = Strings.Shot10kMissilesDesc;
                        break;
                    case AwardsManager.awards.Total50kMissiles:
                        title = Strings.Shot50kMissiles;
                        desc = Strings.Shot50kMissilesDesc;
                        break;
                    case AwardsManager.awards.Total100kMissiles:
                        title = Strings.Shot100kMissiles;
                        desc = Strings.Shot100kMissilesDesc;
                        break;
                    case AwardsManager.awards.Total100Pickups:
                        title = Strings.Got100Pickups;
                        desc = Strings.Got100PickupsDesc;
                        break;
                    case AwardsManager.awards.Total1kPickups:
                        title = Strings.Got1kPickups;
                        desc = Strings.Got1kPickupsDesc;
                        break;
                    case AwardsManager.awards.Total10kPickups:
                        title = Strings.Got10kPickups;
                        desc = Strings.Got10kPickups;
                        break;
                    case AwardsManager.awards.OneShipLeft:
                        title = Strings.AwdOneShip;
                        desc = Strings.AwdOneShipDesc;
                        break;
                    case AwardsManager.awards.MaxWeapons:
                        title = Strings.AwdMaxWeapons;
                        desc = Strings.AwdMaxWeaponsDesc;
                        break;
                    case AwardsManager.awards.MaxEverything:
                        title = Strings.AwdMaxEverything;
                        desc = Strings.AwdMaxEverythingDesc;
                        break;
                }
                if (AwardsManager.currentPlayerAwards.awards[i + index])
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, title, new Vector2(0.1f, 0.1f + (i * 0.085f)), Color.White, 1f, justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, desc, new Vector2(0.1f, 0.125f + (i * 0.085f)), Color.White, 0.8f, justification.left);
                    Game1.scrmgr.drawTexture(spriteBatch, AwardsManager.icons[i+index], new Vector2(0.075f, 0.1125f + (i * 0.085f)), Color.White, 0.75f, 0);
                }
                else
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, title, new Vector2(0.1f, 0.1f + (i * 0.085f)), Color.DarkGray, 1f, justification.left);
                    Game1.scrmgr.drawTexture(spriteBatch, AwardsManager.icons[i + index], new Vector2(0.075f, 0.1125f + (i * 0.085f)), new Color(0.05f, 0.05f, 0.05f, 0.5f), 0.75f, 0);
                }
            }

            for (int i = 0; i < AwardsManager.currentPlayerAwards.stats.Count; i++)
            {
                switch ((AwardsManager.stats)i)
                {
                    case AwardsManager.stats.highScore:
                        title = Strings.personHigh;
                        break;
                    case AwardsManager.stats.totalKilled:
                        title = Strings.TotalKills;
                        break;
                    case AwardsManager.stats.totalScore:
                        title = Strings.TotalScore;
                        break;
                    case AwardsManager.stats.totalMissiles:
                        title = Strings.TotalMissiles;
                        break;
                    case AwardsManager.stats.totalPickups:
                        title = Strings.TotalPickups;
                        break;
                    case AwardsManager.stats.totalBossesKilled:
                        title = Strings.TotalLevelsCompletes;
                        break;
                }

                desc = AwardsManager.currentPlayerAwards.stats[i].ToString();

                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, title, new Vector2(0.9f, 0.1f + (i * 0.085f)), Color.White, 1f, justification.right);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, desc, new Vector2(0.9f, 0.125f + (i * 0.085f)), Color.White, 0.8f, justification.right);
            }

            Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.MainMenu, new Vector2(0.12f, 0.9f), justification.left);

            Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.MoreAwards, new Vector2(0.12f, 0.86f), justification.left);
           
        }
    }
}
