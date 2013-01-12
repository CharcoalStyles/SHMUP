using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens.Levels
{
    class TutorialLevel : Screen
    {
        bool loaded;

        public Texture2D bkgrnd;
        public Background background;

        Color basicEnemyColor;

        int spawnCounterSwitch = 4500;

        //counter
        int i;

        int wavecounter = 0;
        int spawnCounter;

        Color messageColor = Color.White;
        Color messageShadow = Color.Black;
        String message = "";
        Vector2 messagePos = new Vector2(0.5f, 0.35f);

        public TutorialLevel()
        {
            loaded = false;
            Game1.gammgr.playerShip.visible = true;
        }

        public override void Load()
        {
            //basic black background
            bkgrnd = new Texture2D(Game1.graphics.GraphicsDevice, Game1.graphics.GraphicsDevice.DisplayMode.Width, Game1.graphics.GraphicsDevice.DisplayMode.Height,
                false, SurfaceFormat.Color);

            Color[] pixels = new Color[bkgrnd.Width * bkgrnd.Height];

            for (int y = 0; y < bkgrnd.Height; y++)
            {
                for (int x = 0; x < bkgrnd.Width; x++)
                {
                        pixels[y * bkgrnd.Width + x] = Color.Black;
                }
            }

            bkgrnd.SetData<Color>(pixels);

            //new background.
            //Soothing light blue.
            background = new Background(new Vector4(0.1f, 0.6f, 1, 0.25f), true);
            basicEnemyColor = new Color(Vector3.One - background.topColor.ToVector3());
            PlayerBuffs.topColor = background.topColor;
            PlayerBuffs.botColor = background.botColor;

            for (int i = 0; i < Game1.gammgr.saveGameData.totalPlayerBuffs; i++)
            {
                //Add a single player Buff to the GameManager
                PlayerBuffs pb = new PlayerBuffs();
                pb.resetPosition(Game1.gammgr.playerShip.position);
                Game1.gammgr.playerBuffs.Add(pb);
            }

            Game1.gammgr.isPlaying = true;
            Game1.gammgr.isLevelOver = false;
            Game1.gammgr.lh.reset();
            Game1.gammgr.mb.Reset();
            Game1.gammgr.inTutLevel = true;

            loaded = true;

            Game1.musman.PlayNewSong(4);
        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.gammgr.isPlaying)
            {
                Game1.gammgr.playerShip.visible = true;
                background.Update();
                
                spawnCounter += gameTime.ElapsedGameTime.Milliseconds;

                if (spawnCounter > spawnCounterSwitch)
                {
                    wavecounter++;
                    spawnCounter = 0;

                    switch (wavecounter)
                    {
                        case 1:
                            messageColor = Color.White;
                            if (Game1.inpmgr.playerOneKeyboard)
                                message = Strings.TUT_0k;
                            else
                                message = Strings.TUT_0p;
                            break;
                        case 2:
                            messageColor = Color.White;
                            message = Strings.TUT_1;
                            break;
                        case 3:
                            messageColor = Color.White;
                            if (Game1.inpmgr.playerOneKeyboard)
                                message = Strings.TUT_2k;
                            else
                                message = Strings.TUT_2p;
                            break;
                        case 4:
                            Game1.gammgr.enemies.Add(new Enemies.Solid3(new Vector2(1.1f, 0.5f),
                                background.topColor, background.botColor));
                            message = "";
                            break;
                        case 5:
                            messageColor = Color.White;
                            if (Game1.inpmgr.playerOneKeyboard)
                                message = Strings.TUT_3k;
                            else
                                message = Strings.TUT_3p;
                            Game1.gammgr.enemies.Add(new Enemies.Solid3(new Vector2(1.1f, 0.5f),
                                background.topColor, background.botColor));
                            break;
                        case 6:
                            messageColor = Color.White;
                            message = Strings.TUT_4;
                            messagePos = new Vector2(0.5f, 0.75f);
                            Game1.gammgr.enemies.Add(new Enemies.Solid3(new Vector2(1.1f, 0.5f),
                                background.topColor, background.botColor));
                            break;
                        case 7:
                            messageColor = Color.White;
                            message = Strings.TUT_5;
                            messagePos = new Vector2(0.5f, 0.35f);
                            for (i = 0; i < 15; i++)
                            {
                                Game1.gammgr.enemies.Add(new Enemies.Solid3(new Vector2(1.1f, 0.1f + (i * 0.05f)),
                                    background.topColor, background.botColor));
                            }
                            break;
                        case 8:
                            for (i = 0; i < 15; i++)
                            {
                                Game1.gammgr.enemies.Add(new Enemies.Solid3(new Vector2(1.1f, 0.9f - (i * 0.05f)),
                                    background.topColor, background.botColor));
                            }
                            messageColor = Color.White;
                            message = Strings.TUT_6;
                            break;
                        case 9:
                            messageColor = Color.White;
                            message = Strings.TUT_7;
                            break;
                        case 10:
                            foreach (Enemies.Enemy tEn in Game1.gammgr.enemies)
                            {
                                tEn.takeDamage(100);
                            }
                            messageColor = Color.White;
                            message = Strings.TUT_8;
                            Game1.gammgr.pickups.Add(new Pickup(new Vector2(0.5f,0.5f), PickupType.missileAdd));
                            break;
                        case 11:
                            messageColor = Color.White;
                            message = Strings.TUT_9;
                            Game1.gammgr.pickups.Add(new Pickup(new Vector2(0.5f, 0.5f), PickupType.shieldAdd));
                            break;
                        case 12:
                            messageColor = Color.White;
                            message = Strings.TUT_10;
                            Game1.gammgr.pickups.Add(new Pickup(new Vector2(0.5f, 0.5f), PickupType.slowMo));

                            for (i = 0; i < 2; i++)
                            {
                                Game1.gammgr.enemies.Add(new Enemies.Solid3(new Vector2(1.1f, 0.4f + (i * 0.2f)),
                                    background.topColor, background.botColor));
                            }
                            break;
                        case 13:
                            message = "";
                            break;
                        case 14:
                            messageColor = Color.White;
                            message = Strings.TUT_11;
                            break;
                        case 15:
                            Game1.gammgr.isLevelOver = true;

                            PickupEffectManager.killSlowmo();

                            Game1.gammgr.saveGameData.totalScore += Game1.gammgr.playerLevelScore;
                            SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                            Game1.gammgr.playerLevelScore = 0;
                            Game1.gammgr.multiplier = 1;

                            LevelManager.GenerateOriginalLevelSequence();
                            Game1.scrmgr.changeScreen(new Screens.ShopScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]), true));
        
                            Game1.gammgr.Clear(false);

                            Game1.gammgr.saveGameData.skipTutorial = true;
                            SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                            break;
                    }
                }

                if (spawnCounter > 3000 && message != "")
                {
                    float f = spawnCounter - 3000;
                    messageColor = new Color(1, 1, 1, 1 - (f / 800));
                    messageShadow = new Color(0, 0, 0, 1 - (f / 800));
                }
            }
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            Game1.scrmgr.drawTexture(spriteBatch, bkgrnd, new Vector2(0.5f, 0.5f), Color.White);
            background.Draw(gametime, spriteBatch);

            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, message, messagePos, messageColor, messageShadow, justification.centre);
        }
    }
}
