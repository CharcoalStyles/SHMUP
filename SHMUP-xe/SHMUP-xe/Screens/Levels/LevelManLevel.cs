using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens.Levels
{
    class LevelManLevel : Screen
    {
        bool loaded;

        public Texture2D bkgrnd;
        public Background background;

        ArrayList enemies = new ArrayList();
        Color basicEnemyColor;

        bool bossSpawned = false;
        int spawnCounterSwitch = 1000;

        public static bool wallBreached = false;
        int gameovercounter = 199;
        public static float breachedAtY = -1;

        int wavecounter = -1;
        int spawnCounter;

        bool finishedIntro = false;
        int counter = 0;
        string string1 = "";
        int line = 0;
        Color introColor = new Color(1f, 1f, 1f, 1f);
        Color introShadowColor = new Color(0f, 0f, 0f, 1f);

        string level;
        LevelManager.LevelData levelData;
        int lastWave = -1;

        bool moved = false;

        bool shownIntroScreen = false;

        Enemies.BossManBoss bmb;

        public LevelManLevel(string inLevel)
        {
            Game1.gammgr.playerShip.visible = true;
            level = inLevel;
            levelData = new LevelManager.LevelData();
            loaded = false;
        }

        public override void Load()
        {

            levelData = LevelManager.LoadLevel(level);

            if (Game1.IS_DEMO)
                Game1.musman.PlayNewSong(Game1.gammgr.r.Next(1, 5));
            else
                Game1.musman.PlayNewSong(Game1.gammgr.r.Next(1, 13));

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
            background = new Background(levelData.colorHigh, levelData.colorLow, true);
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


            spawnCounterSwitch = Game1.gammgr.r.Next(levelData.spawnLow, levelData.spawnHigh);

            bmb = new SHMUP.Enemies.BossManBoss(background.topColor, background.botColor);
            bmb.LoadNewBoss(levelData.boss);

            Game1.gammgr.lh.reset();
            Game1.gammgr.mb.Reset();
            Game1.gammgr.inTutLevel = false;

            AwardsManager.shipHit = false;
            AwardsManager.shipDead = false;
            AwardsManager.wallHit = false;
            AwardsManager.missileshot = false;
            
            wallBreached = false;
            gameovercounter = 0;
            breachedAtY = -1;

            loaded = true;

            Game1.gammgr.isLevelOver = false;

            GC.Collect();
        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        public override void Update(GameTime gameTime)
        {

            if (Game1.gammgr.isPlaying)
            {
                background.Update();
                Game1.gammgr.playerShip.visible = true;
                spawnCounter += (int)(((float)gameTime.ElapsedGameTime.Milliseconds / 16) * Game1.gammgr.d * (double)Game1.gammgr.enemySpawnMultiplier);

                #region SpawnStuff
                if (spawnCounter > spawnCounterSwitch)
                {
                    wavecounter++;

                    for (int i = 0; i < levelData.groupSpawnRound.Count; i++)
                    {
                        if (levelData.groupSpawnRound[i] > lastWave)
                        {
                            lastWave = levelData.groupSpawnRound[i];
                        }
                        if (levelData.groupSpawnRound[i] == wavecounter)
                        {
                            for (int o = 0; o < levelData.numberOfenemies[i]; o++)
                            {
                                Vector2 enemyPosition = new Vector2(levelData.groupInitalPosition[i].X + (levelData.groupIncrementalPosition[i].X * o),
                                    levelData.groupInitalPosition[i].Y + (levelData.groupIncrementalPosition[i].Y * o));
                                switch ((LevelManager.enemies)levelData.enemyType[i])
                                {
                                    case LevelManager.enemies.Solid3:
                                        Game1.gammgr.enemies.Add(new Enemies.Solid3(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.Solid4:
                                        Game1.gammgr.enemies.Add(new Enemies.Solid4(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.SoftStar3:
                                        Game1.gammgr.enemies.Add(new Enemies.SoftStar3(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.SoftStar4:
                                        Game1.gammgr.enemies.Add(new Enemies.SoftStar4(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.SoftStar5:
                                        Game1.gammgr.enemies.Add(new Enemies.SoftStar5(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.SoftStar6:
                                        Game1.gammgr.enemies.Add(new Enemies.SoftStar6(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.HardStar3:
                                        Game1.gammgr.enemies.Add(new Enemies.HardStar3(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.HardStar4:
                                        Game1.gammgr.enemies.Add(new Enemies.HardStar4(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.HardStar5:
                                        Game1.gammgr.enemies.Add(new Enemies.HardStar5(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.HardStar6:
                                        Game1.gammgr.enemies.Add(new Enemies.HardStar6(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.Solid5:
                                        Game1.gammgr.enemies.Add(new Enemies.Solid5(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.Solid6:
                                        Game1.gammgr.enemies.Add(new Enemies.Solid6(enemyPosition, background.botColor, background.topColor));
                                        break;
                                    case LevelManager.enemies.bulRep3:
                                        Game1.gammgr.enemies.Add(new Enemies.BulRep(enemyPosition, background.botColor, background.topColor, 3));
                                        break;
                                    case LevelManager.enemies.bulRep4:
                                        Game1.gammgr.enemies.Add(new Enemies.BulRep(enemyPosition, background.botColor, background.topColor, 4));
                                        break;
                                    case LevelManager.enemies.bulRep5:
                                        Game1.gammgr.enemies.Add(new Enemies.BulRep(enemyPosition, background.botColor, background.topColor, 5));
                                        break;
                                    case LevelManager.enemies.bulRep6:
                                        Game1.gammgr.enemies.Add(new Enemies.BulRep(enemyPosition, background.botColor, background.topColor, 6));
                                        break;
                                }
                            }
                        }
                    }

                    spawnCounterSwitch = Game1.gammgr.r.Next(levelData.spawnLow, levelData.spawnHigh);
                    spawnCounter = 0;
                }

                if (!bossSpawned && Game1.gammgr.enemies.Count == 0)
                {
                    spawnCounter = spawnCounterSwitch;
                }
                #endregion

                #region Bossness
                if (wavecounter >= lastWave + 1 && Game1.gammgr.enemies.Count == 0)
                {
                    if (!bossSpawned)
                    {
                        bmb.callIn = true;

                        bossSpawned = true;
                        Game1.gammgr.bossIn = true;
                        Game1.musman.PlayNewSong("Triwobular");
                    }
                    else
                    {
                        bmb.Update(Game1.gammgr.d);
                        if (Game1.gammgr.bossDead)
                        {
                            AwardsManager.LevelOver();

                            PickupEffectManager.killSlowmo();

                            AwardsManager.currentPlayerAwards.stats[(int)AwardsManager.stats.totalBossesKilled]++;

                            Game1.gammgr.saveGameData.totalScore += Game1.gammgr.playerLevelScore;
                            SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                            Game1.gammgr.playerLevelScore = 0;

                            Game1.gammgr.isLevelOver = true;
                            Game1.gammgr.currentLevelInSequence++;

                            if (Game1.gammgr.currentLevelInSequence == Game1.gammgr.levelSequence.Count)
                            {
                                Game1.scrmgr.changeScreen(new Screens.GameOverScreen(Game1.gammgr.playerRunScore, true));
                                Game1.gammgr.isLevelOver = true;
                            }
                            else
                            {
                                //Game1.scrmgr.changeScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]));
                                Game1.scrmgr.changeScreen(new Screens.ShopScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]), true));

                                Game1.gammgr.Clear(false);
                            }
                        }
                    }
                }
                #endregion

                #region level over (wallbreach)
                if (wallBreached)
                {
                    gameovercounter++;
                    if (gameovercounter >= 45)
                    {
                        PickupEffectManager.killSlowmo();
                        gameovercounter = 0;
                        Game1.pclmgr.createEffect(ParticleEffect.enemyBreach, new Vector2(-0.01f, breachedAtY), new Color(Vector3.One - Vector3.Lerp(background.topColor.ToVector3(), background.topColor.ToVector3(), breachedAtY)));
                        if (Game1.gammgr.playerBuffs.Count != 0)
                        {
                            PlayerBuffs pb = null;
                            foreach (PlayerBuffs tpb in Game1.gammgr.playerBuffs)
                            {
                                pb = tpb;
                            }

                            pb.takeDamage(500);
                        }
                    }
                }
                #endregion
            }
            else if (!shownIntroScreen)
            {
                if (Game1.inpmgr.playerOneInput.A == expButtonState.Pressed)
                {
                    Game1.gammgr.isPlaying = true;
                    shownIntroScreen = true;
                }

                if (Game1.inpmgr.playerOneKeyboard)
                {
                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                    {
                        if (Game1.gammgr.enemySpawnMultiplier < 5)
                        {
                            Game1.gammgr.enemySpawnMultiplier += 0.25m;
                        }
                    }
                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                    {
                        if (Game1.gammgr.enemySpawnMultiplier > 1)
                        {
                            Game1.gammgr.enemySpawnMultiplier -= 0.25m;
                        }
                    }
                }
                else
                {
                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !moved)
                    {
                        moved = true;
                        if (Game1.gammgr.enemySpawnMultiplier < 5)
                        {
                            Game1.gammgr.enemySpawnMultiplier += 0.25m;
                        }
                    }
                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !moved)
                    {
                        moved = true;
                        if (Game1.gammgr.enemySpawnMultiplier > 1)
                        {
                            Game1.gammgr.enemySpawnMultiplier -= 0.25m;
                        }
                    }
                    else if (Game1.inpmgr.playerOneInput.leftStick.Y > -0.3f &&
                    Game1.inpmgr.playerOneInput.leftStick.Y < 0.3f && moved)
                    {
                        moved = false;
                    }
                }
            }

            #region intro
            if (!finishedIntro)
            {
                counter++;
                switch (line)
                {
                    case 0:
                        try
                        {
                            if (counter % 10 == 0)
                            {
                                string1 = level.Substring(0, counter / 10);
                            }
                        }
                        catch
                        {
                            counter = 0;
                            line++;
                        }
                        break;
                    case 1:
                        if (counter == 150)
                        {
                            line++;
                            spawnCounter = spawnCounterSwitch;
                        }
                        break;
                    case 2:
                        if (Game1.gammgr.isPlaying)
                        {
                            counter -= 3;
                            introColor = new Color(1, 1, 1, ((float)counter) / 150f);
                            introShadowColor = new Color(0, 0, 0, ((float)counter) / 150f);

                            spawnCounter += (int)(((float)gameTime.ElapsedGameTime.Milliseconds / 16) * Game1.gammgr.d);

                            if (counter == 0)
                            {
                                finishedIntro = true;
                            }
                        }
                        break;
                }
            }
            #endregion

        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            Game1.scrmgr.drawTexture(spriteBatch, bkgrnd, new Vector2(0.5f, 0.5f), Color.White);
            background.Draw(gametime, spriteBatch);

            if (!Game1.gammgr.isPlaying && !shownIntroScreen)
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SpawnSpeed, new Vector2(0.5f, 0.46f), justification.centre);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, "x" + Game1.gammgr.enemySpawnMultiplier.ToString(), new Vector2(0.5f, 0.5f), justification.left);
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.48f, 0.5f), Color.White, 0.5f, 0);

                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.StartLevel, new Vector2(0.5f, 0.6f), justification.left);
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.48f, 0.6f), Color.White, 0.5f, 0);
            }

            bmb.Draw(spriteBatch, drawMode.Keyline, gametime);
            bmb.Draw(spriteBatch, drawMode.Bottom, gametime);
            bmb.Draw(spriteBatch, drawMode.Top, gametime);

            if (!finishedIntro)
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, string1, new Vector2(0.5f, 0.35f), introColor, introShadowColor, justification.centre);
            }
        }
    }
}
