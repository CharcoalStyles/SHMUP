using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SHMUP
{
    public class GameManager
    {
        public Random r;
        public PlayerShip playerShip;

        public string currentPlayer = "";
        public SaveGameManager.SaveGameData saveGameData = new SaveGameManager.SaveGameData();
        public bool loggedIn = false;

        public ArrayList enemies = new ArrayList();
        public ArrayList playerBuffs = new ArrayList();
        public ArrayList pickups = new ArrayList();

        public int numBuffs;

        public List<Bullet> playerBulletList = new List<Bullet>();
        public List<Bullet> enemyBulletList = new List<Bullet>();
        public List<Bullet> unusedBulletList = new List<Bullet>();

        public List<Explosion> explosionList = new List<Explosion>();
        public List<Explosion> unusedExplosionList = new List<Explosion>();

        public bool bossIn = false;
        public bool bossDead = false;
        public Vector2 bossPos = Vector2.Zero;

        public bool isPlaying = false;
        public bool isPaused = false;

        public bool isLevelOver = false;

        public bool inTutLevel = false;

        public List<String> levelSequence = new List<string>();
        public int currentLevelInSequence;

        public int playerRunScore = 0;
        public int playerLevelScore = 0;

        public decimal multiplier = 1;
        int toScore = 0;
        float playerScoreSize = 0.75f;
        List<int> scoreNotifiers = new List<int>();
        List<Vector3> scorePositionAlpha = new List<Vector3>();

        public LevelHealth lh;

        public MissileManager mb;

        public float shootingTimer;

        public decimal enemySpawnMultiplier = 1;

        int bulletHitMod = 0;

        public String cursorSpeed = "";

        //utility variables;
        int i;
        List<Vector2> pbPos = new List<Vector2>();
        ArrayList toDelete = new ArrayList();

        //menu
        enum pauseMenuSelect
        {
            resume,
            mainmenu,
            musicVolume,
            sfxVolume,
            cursorSpeed,
            none
        }

        //menu vars
        pauseMenuSelect currentSelection = pauseMenuSelect.resume;
        List<float> menuSizes = new List<float>();
        List<Color> menuColors = new List<Color>();
        bool moved = false;
        bool lrMoved = false;
        string musVol;
        string sfxVol;

        double need = 0.05;

        public double divisor = 1;

        public GameManager()
        {
            r = new Random();

            for (int i = 0; i < (int)pauseMenuSelect.none + 1; i++)
            {
                menuSizes.Add(1);
                menuColors.Add(Color.White);
            }

            //musVol = Strings.MusicVolume + " = " + (Game1.musman.volume * 10);
            //sfxVol = Strings.EffectsVolume + " = " + (Game1.sndmgr.masterFXVolume * 10);
            saveGameData = SaveGameManager.MakeNewData();
            AwardsManager.currentPlayerAwards = AwardsManager.MakeNewData();

            lh = new LevelHealth();
            mb = new MissileManager();

            for (i = 0; i < 20; i++)
            {
                unusedBulletList.Add(new Bullet());
            }
        }

        public void Clear(bool fullClear)
        {
            enemies.Clear();
            playerBuffs.Clear();
            for (i = 0; i < playerBulletList.Count; i++)
            {
                playerBulletList[i].live = false;
                unusedBulletList.Add(playerBulletList[i]);
                playerBulletList.RemoveAt(i);
            }
            playerBulletList.Clear();

            for (i = 0; i < enemyBulletList.Count; i++)
            {
                enemyBulletList[i].live = false;
                unusedBulletList.Add(enemyBulletList[i]);
                enemyBulletList.RemoveAt(i);
            }
            enemyBulletList.Clear();

            pickups.Clear();
            
            bossIn = false;
            bossDead = false;

            multiplier = 1;

            if (fullClear)
            {
                playerRunScore = 0;
            }

            toScore = 0;
            scoreNotifiers.Clear();
            scorePositionAlpha.Clear();
            shootingTimer = 0;

            playerShip.reset();
        }
            public double d;
            bool lastBuffslow = false;

        public void Update(GameTime gameTime)
        {
            Game1.gammgr.playerShip.update(gameTime);
            #region Yes, we are playing
            if (isPlaying)
            {
                lh.Update(gameTime);
                mb.Update();
                toDelete.Clear();
                pbPos.Clear();

                foreach (PlayerBuffs tPb in playerBuffs)
                {
                    pbPos.Add(tPb.position);
                }

                foreach (PlayerBuffs tPb in playerBuffs)
                {
                    tPb.update(playerShip.position, pbPos);

                    if (tPb.isDead)
                    {
                        Game1.pclmgr.createEffect(ParticleEffect.pbExplosion, tPb.position, tPb.c);
                        toDelete.Add(tPb);
                        numBuffs--;
                    }
                }

                foreach (PlayerBuffs tPb in toDelete)
                {
                    playerBuffs.Remove(tPb);
                }

                toDelete.Clear();

                
                foreach (Pickup tPu in pickups)
                {
                    tPu.Update(gameTime);

                    if (tPu.position.X < 0)
                        toDelete.Add(tPu);

                    foreach (PlayerBuffs tPb in playerBuffs)
                    {
                        if (tPu.checkCollision(tPb.position, tPb.shipRadius))
                        {
                            toDelete.Add(tPu);
                        }
                    }
                }


                foreach (Pickup tPu in toDelete)
                {
                    pickups.Remove(tPu);
                }

                toDelete.Clear();

                d = gameTime.ElapsedGameTime.TotalMilliseconds / divisor;

                foreach (Enemies.Enemy tEnemy in enemies)
                {
                    tEnemy.update(d);

                    if (tEnemy.position.X < 0.1f)
                    {
                        for (int i = 0; i < lh.barrierPosition.Count; i++)
                        {
                            if (Vector2.Distance(tEnemy.position, lh.barrierPosition[i]) < 0.05f)
                            {
                                if (lh.Damage(i))
                                {
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyTeleport, tEnemy.position, Color.White);
                                    tEnemy.position = new Vector2(0.9f, tEnemy.position.Y);
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyTeleport, tEnemy.position, Color.White);

                                    if (multiplier > 1)
                                    {
                                        multiplier -= 0.1m;
                                    }
                                }
                            }
                        }

                        if (tEnemy.position.X < -0.01f)
                        {
                            Screens.Levels.LevelManLevel.wallBreached = true;
                            Screens.Levels.LevelManLevel.breachedAtY = tEnemy.position.Y;
                                
                        }
                    }

                    if (tEnemy.isDead)
                    {

                        if (tEnemy.isBoss)
                        {
                            bossDead = true;
                        }
                        else
                        {
                            if (pickups.Count == 0 && !inTutLevel)
                            {
                                int shldneed = 2;
                                int misneed = 2;
                                need = saveGameData.basePickUpRate;
                                need += (1 - ((double)mb.missilesLeft / (double)saveGameData.maxMissiles)) * 0.05;
                                misneed += (int)(1 - ((double)mb.missilesLeft / (double)saveGameData.maxMissiles)) * 2;
                                if (lh.shieldFailing)
                                {
                                    need += 0.05;
                                    shldneed += 2;
                                }
                                if (numBuffs < saveGameData.totalPlayerBuffs)
                                    need += 0.05;
                                if (lastBuffslow)
                                {
                                    shldneed += 2;
                                    misneed += 2;
                                }

                                switch (tEnemy.texture)
                                {
                                    case SHMUP.Enemies.EnemyTextures.Solid3:
                                        need -= 0.05;
                                        break;
                                    case SHMUP.Enemies.EnemyTextures.Solid4:
                                        need -= 0.03;
                                        break;
                                    case SHMUP.Enemies.EnemyTextures.Solid5:
                                        need -= 0.01;
                                        break;
                                }

                                if (r.NextDouble() < need)
                                {
                                    i = r.Next(10);
                                    if (i < shldneed)
                                    {
                                        lastBuffslow = false;
                                        pickups.Add(new Pickup(tEnemy.position, PickupType.shieldAdd));
                                    }
                                    else if (i < shldneed + misneed)
                                    {
                                        lastBuffslow = false;
                                        pickups.Add(new Pickup(tEnemy.position, PickupType.missileAdd));
                                    }
                                    else
                                    {
                                        lastBuffslow = true;
                                        pickups.Add(new Pickup(tEnemy.position, PickupType.slowMo));
                                    }
                                }
                            }
                        }
                        AddScore(tEnemy.score, tEnemy.position);
                        
                        AwardsManager.enemyKilled(tEnemy);
                        if (multiplier < 10m)
                        {
                            multiplier += 0.1m;
                        }
                        playerScoreSize += 0.1f;
                        toDelete.Add(tEnemy);
                    }
                }

                foreach (Enemies.Enemy tEnemy in toDelete)
                {
                    enemies.Remove(tEnemy);
                }

                toDelete.Clear();

                for (i = 0; i < playerBulletList.Count; i++)
                {
                    if (playerBulletList[i].update(d))
                    {
                        playerBulletList[i].live = false;
                        unusedBulletList.Add(playerBulletList[i]);
                        playerBulletList.RemoveAt(i);
                    }
                }

                for (i = 0; i < enemyBulletList.Count; i++)
                {
                    if (enemyBulletList[i].update(d))
                    {
                        enemyBulletList[i].live = false;
                        unusedBulletList.Add(enemyBulletList[i]);
                        enemyBulletList.RemoveAt(i);
                    }
                }

                for (i = 0; i < explosionList.Count; i++)
                {
                    explosionList[i].Update();

                    if (explosionList[i].finished)
                    {
                        unusedExplosionList.Add(explosionList[i]);
                        explosionList.RemoveAt(i);
                    }
                }

                foreach (Enemies.Enemy tEnemy in enemies)
                {
                    if (gameTime.IsRunningSlowly)
                    {
                        for (i = 0; i < playerBulletList.Count; i++)
                        {
                            if ((i + bulletHitMod) % 2 == 0)
                            {
                                if (playerBulletList[i].CheckHit(tEnemy.position, tEnemy.shipRadius))
                                {
                                    tEnemy.takeDamage(3);
                                    playerBulletList[i].live = false;
                                    unusedBulletList.Add(playerBulletList[i]);
                                    playerBulletList.RemoveAt(i);

                                }
                            }
                        }


                        bulletHitMod++;
                        if (bulletHitMod == 2)
                            bulletHitMod = 0;
                    }
                    else
                    {
                        for (i = 0; i < playerBulletList.Count; i++)
                        {
                            if (playerBulletList[i].CheckHit(tEnemy.position, tEnemy.shipRadius))
                            {
                                tEnemy.takeDamage(3);
                                playerBulletList[i].live = false;
                                unusedBulletList.Add(playerBulletList[i]);
                                playerBulletList.RemoveAt(i);

                            }
                        }
                    }

                    for (i = 0; i < explosionList.Count; i++)
                    {
                        explosionList[i].checkHit(tEnemy);
                    }
                }


                foreach (Enemies.Enemy tEnemy in enemies)
                {
                    foreach (PlayerBuffs tPb in playerBuffs)
                    {
                        if (Vector2.Distance(tEnemy.position, tPb.position) < (tEnemy.shipRadius + tPb.shipRadius) * 0.8f)
                        {
                            if (!inTutLevel)
                            {
                                tEnemy.takeDamage(1);
                                tPb.takeDamage(1);
                            }
                            else
                            {
                                tEnemy.takeDamage(1);
                                tPb.takeDamage(0);
                            }
                        }
                    }
                }

                foreach (PlayerBuffs tPb in playerBuffs)
                {
                    for (i = 0; i < enemyBulletList.Count; i++)
                    {
                        if (Vector2.Distance(enemyBulletList[i].position, tPb.position) < tPb.shipRadius * 0.8f)
                        {
                            tPb.takeDamage(3);

                            enemyBulletList[i].live = false;
                            unusedBulletList.Add(enemyBulletList[i]);
                            enemyBulletList.RemoveAt(i);
                        }
                    }
                }

                if (playerBuffs.Count == 0 && !isLevelOver)
                {
                    PickupEffectManager.killSlowmo();
                    Game1.scrmgr.changeScreen(new Screens.GameOverScreen(playerRunScore, false));

                    Game1.gammgr.saveGameData.totalScore += playerLevelScore;
                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                    Game1.gammgr.playerRunScore = 0;

                    Clear(true);
                }

                if (bossIn)
                {
                    if (bossDead)
                    {
                        playerRunScore += toScore;
                        playerLevelScore += toScore;
                        toScore = 0;
                    }
                }

                GC.Collect();

                //pause Menu
                if (Game1.inpmgr.playerOneInput.Back == expButtonState.Pressed ||
                    Game1.inpmgr.playerOneInput.Start == expButtonState.Pressed)
                {
                    isPlaying = false;
                    isPaused = true;
                    currentSelection = pauseMenuSelect.resume;
                    Game1.inpmgr.playerOneInput.Back = expButtonState.Held;
                    Game1.inpmgr.playerOneInput.Start = expButtonState.Held;
                }

                //score Notifiers
                for (int i = 0; i < scoreNotifiers.Count; i++)
                {
                    scorePositionAlpha[i] = new Vector3(scorePositionAlpha[i].X, scorePositionAlpha[i].Y - 0.001f, scorePositionAlpha[i].Z - 0.02f);
                    if (scorePositionAlpha[i].Z <= 0)
                    {
                        scoreNotifiers.RemoveAt(i);
                        scorePositionAlpha.RemoveAt(i);
                    }
                }

            }
            #endregion
            if (isPaused)
            {
                if (Game1.inpmgr.playerOneKeyboard == false)
                {
                    #region JoypadSelection
                    if (Game1.inpmgr.playerOneInput.leftStick.Y > 0.3f && !moved)
                    {
                        moved = true;
                        menuSizes[(int)currentSelection] = 1;

                        Game1.sndmgr.playSound(SFX.menuMove);
                        if (currentSelection == pauseMenuSelect.cursorSpeed)
                        {
                            currentSelection = pauseMenuSelect.resume;
                        }
                        else
                        {
                            currentSelection++;
                        }
                    }
                    else if (Game1.inpmgr.playerOneInput.leftStick.Y < -0.3f && !moved)
                    {
                        moved = true;
                        menuSizes[(int)currentSelection] = 1;

                        Game1.sndmgr.playSound(SFX.menuMove);
                        if (currentSelection == pauseMenuSelect.resume)
                        {
                            currentSelection = pauseMenuSelect.cursorSpeed;
                        }
                        else
                        {
                            currentSelection--;
                        }
                    }
                    else if (Game1.inpmgr.playerOneInput.leftStick.Y > -0.3f &&
                        Game1.inpmgr.playerOneInput.leftStick.Y < 0.3f && moved)
                    {
                        moved = false;
                    }
                    #endregion
                }
                else
                {
                    #region mouseSelection
                    if (Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition - 0.025f)
                    {
                        currentSelection = pauseMenuSelect.none;
                    }
                    else
                    {
                        if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition - 0.025f &&
                            Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.025f)
                        {
                            currentSelection =  pauseMenuSelect.resume;
                        }
                        else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.025f &&
                                Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.075f)
                        {
                            currentSelection = pauseMenuSelect.mainmenu;
                        }
                        else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.075f &&
                                Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.125f)
                        {
                            currentSelection = pauseMenuSelect.musicVolume;
                        }
                        else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.125f &&
                                Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.175f)
                        {
                            currentSelection = pauseMenuSelect.sfxVolume;
                        }
                        else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.175f &&
                                Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.225f)
                        {
                            currentSelection = pauseMenuSelect.cursorSpeed;
                        }
                        else
                        {
                            currentSelection = pauseMenuSelect.none;
                        }
                    }
                    #endregion
                }


                if (Game1.inpmgr.playerOneKeyboard == false)
                {
                    #region JoypadModification
                    if (currentSelection == pauseMenuSelect.musicVolume)
                    {
                        musVol = "< " + Strings.MusicVolume + " = " + ((int)(Game1.musman.volume * 64) * 4) + " >";
                        if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            lrMoved = true;
                            if (Game1.musman.volume < 1)
                                Game1.musman.volume += 0.05m;
                        }
                        else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            lrMoved = true;
                            if (Game1.musman.volume > 0)
                                Game1.musman.volume -= 0.05m;
                        }
                        else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                            Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                        {
                            lrMoved = false;
                        }
                    }
                    else
                    {
                        musVol = Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100);
                    }

                    if (currentSelection == pauseMenuSelect.sfxVolume)
                    {
                        sfxVol = "< " + Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100) + " >";
                        if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            lrMoved = true;
                            if (Game1.sndmgr.masterFXVolume < 1)
                                Game1.sndmgr.masterFXVolume += 0.05m;
                        }
                        else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            lrMoved = true;
                            if (Game1.sndmgr.masterFXVolume > 0)
                                Game1.sndmgr.masterFXVolume -= 0.05m;
                        }
                        else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                            Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                        {
                            lrMoved = false;
                        }
                    }
                    else
                    {
                        sfxVol = Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100);
                    }
#if XBOX
                    if (currentSelection == pauseMenuSelect.cursorSpeed)
                    {
                        cursorSpeed = "<" + Strings.CursorSpeed + " = " + (int)(SettingsManager.settings.stickMod * 100) + ">";

                        if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            lrMoved = true;
                            if (SettingsManager.settings.stickMod < 1)
                                SettingsManager.settings.stickMod += 0.05f;
                        }
                        else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            lrMoved = true;
                            if (SettingsManager.settings.stickMod > 0.1)
                                SettingsManager.settings.stickMod -= 0.05f;
                        }
                        else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                            Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                        {
                            lrMoved = false;
                        }
                    }
                    else
                    {
                        cursorSpeed = Strings.CursorSpeed + " = " + (int)(SettingsManager.settings.stickMod * 100);
                    }
#endif
                    #endregion
                }
                else
                {
                    #region MouseModification
                    if (currentSelection == pauseMenuSelect.musicVolume)
                    {
                        musVol = "< " + Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100) + " >";
                        if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            if (Game1.musman.volume < 1)
                                Game1.musman.volume += 0.05m;
                        }
                        else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            if (Game1.musman.volume > 0)
                                Game1.musman.volume -= 0.05m;
                        }
                    }
                    else
                    {
                        musVol = Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100);
                    }

                    if (currentSelection == pauseMenuSelect.sfxVolume)
                    {
                        sfxVol = "< " + Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100) + " >";
                        if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            if (Game1.sndmgr.masterFXVolume < 1)
                                Game1.sndmgr.masterFXVolume += 0.05m;
                        }
                        else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                        {
                            Game1.sndmgr.playSound(SFX.menuMove);
                            if (Game1.sndmgr.masterFXVolume > 0)
                                Game1.sndmgr.masterFXVolume -= 0.05m;
                        }
                    }
                    else
                    {
                        sfxVol = Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100);
                    }
                    #endregion
                }

                //Selection
                if (Game1.inpmgr.playerOneInput.Start == expButtonState.Pressed ||
                    Game1.inpmgr.playerOneInput.A == expButtonState.Pressed)
                {
                    Game1.sndmgr.playSound(SFX.menuSelect);
                    switch (currentSelection)
                    {
                        case pauseMenuSelect.resume:
                            isPaused = false;
                            isPlaying = true;
                            Game1.inpmgr.playerOneInput.Back = expButtonState.Held;
                            Game1.inpmgr.playerOneInput.Start = expButtonState.Held;
                            break;
                        case pauseMenuSelect.mainmenu:
                            isPaused = false;
                            isPlaying = false;

                            Game1.gammgr.saveGameData.totalScore += Game1.gammgr.playerLevelScore;
                            SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);

                            Game1.scrmgr.changeScreen(new Screens.GameOverScreen(playerRunScore, false));
                            Game1.gammgr.playerRunScore = 0;
                            break;
                    }
                }
                else if (Game1.inpmgr.playerOneInput.B == expButtonState.Pressed)
                {
                    isPaused = false;
                    isPlaying = true;
                }

                for (int i = 0; i < menuColors.Count; i++)
                {
                    if (i == (int)currentSelection)
                    {
                        menuSizes[i] = 1 + (float)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 150) / 10);
                        menuColors[i] = Color.White;
                    }
                    else
                    {
                        menuSizes[i] = 1;
                        menuColors[i] = Color.SlateGray;
                    }
                }
            }


            if (playerScoreSize > 0.75f)
            {
                if (playerScoreSize > 2f)
                    playerScoreSize = 2;
                playerScoreSize *= 0.99f;
                for (i = 0; i < Game1.gammgr.r.Next(3, 8); i++)
                {
                    if (toScore > 0)
                    {
                        playerRunScore++;
                        playerLevelScore++;
                        toScore--;
                    }
                }
            }
            else
            {
                playerRunScore += toScore;
                playerLevelScore += toScore;
                toScore = 0;
            }

        }

        public void AddScore(int scoreIn, Vector2 posIn)
        {
            int tempScore = (int)(scoreIn * multiplier * (0.5m + (Game1.gammgr.enemySpawnMultiplier / 2)));
            toScore += tempScore;
            scoreNotifiers.Add(tempScore);
            AwardsManager.scoreGained(tempScore);
            scorePositionAlpha.Add(new Vector3(posIn, 1));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isPlaying || isPaused)
            {
                //draw enemies
                //Draw Keyline

                foreach (Enemies.Enemy tEnemy in enemies)
                {
                    tEnemy.draw(spriteBatch, drawMode.Keyline);
                }
                //draw shield/health
                foreach (Enemies.Enemy tEnemy in enemies)
                {
                    tEnemy.draw(spriteBatch, drawMode.Bottom);
                }
                //draw inner
                foreach (Enemies.Enemy tEnemy in enemies)
                {
                    tEnemy.draw(spriteBatch, drawMode.Top);
                }

                for (i = 0; i < playerBulletList.Count; i++)
                {
                    playerBulletList[i].Draw(gameTime, spriteBatch);
                }
                for (i = 0; i < enemyBulletList.Count; i++)
                {
                    enemyBulletList[i].Draw(gameTime, spriteBatch);
                }
                for (i = 0; i < explosionList.Count; i++)
                {
                    explosionList[i].Draw(spriteBatch);
                }

                //Draw "buffs"
                //Draw Keyline
                foreach (PlayerBuffs tPb in playerBuffs)
                {
                    tPb.Draw(gameTime, spriteBatch, drawMode.Keyline);
                }
                //draw shield/health
                foreach (PlayerBuffs tPb in playerBuffs)
                {
                    tPb.Draw(gameTime, spriteBatch, drawMode.Bottom);
                }
                //draw core
                foreach (PlayerBuffs tPb in playerBuffs)
                {
                    tPb.Draw(gameTime, spriteBatch, drawMode.Top);
                }

                //Draw pickups
                //Draw Keyline
                foreach (Pickup tPu in pickups)
                {
                    tPu.Draw(spriteBatch, drawMode.Keyline);
                }
                //draw shield/health
                foreach (Pickup tPu in pickups)
                {
                    tPu.Draw(spriteBatch, drawMode.Bottom);
                }
                //draw core
                foreach (Pickup tPu in pickups)
                {
                    tPu.Draw(spriteBatch, drawMode.Top);
                }

                //draw Score notifiers
                for (int i = 0; i < scoreNotifiers.Count; i++)
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, scoreNotifiers[i].ToString(), new Vector2(scorePositionAlpha[i].X, scorePositionAlpha[i].Y), new Color(1, 1, 1, scorePositionAlpha[i].Z), new Color(0, 0, 0, scorePositionAlpha[i].Z), 0.5f + ((1- scorePositionAlpha[i].Z) * 0.5f), justification.centre);
                }

                //draw Score
                //Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, playerScore.ToString().PadLeft(6, char.Parse("0")), new Vector2(0.5f, 0.1f), playerScoreSize);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, playerRunScore.ToString().PadLeft(6, "0".ToCharArray()[0]), new Vector2(0.5f, 0.1f), playerScoreSize, justification.centre);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, multiplier.ToString() + "x", new Vector2(0.5f, 0.14f), 0.5f + playerScoreSize * 0.5f, justification.centre);

                lh.Draw(spriteBatch, drawMode.Keyline);
                lh.Draw(spriteBatch, drawMode.Bottom);
                lh.Draw(spriteBatch, drawMode.Top);
                mb.Draw(spriteBatch);
            }
            if (isPaused)
            {
                Game1.scrmgr.drawTexture(spriteBatch, Shape.CircleFade, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.175f), Color.Black, 4, 0);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Resume, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Quit, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, musVol, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, sfxVol, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.15f), menuColors[3], menuSizes[3], justification.left);
#if XBOX
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, cursorSpeed, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.2f), menuColors[4], menuSizes[4], justification.left);
#endif
                //input guide
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Select, new Vector2(0.12f, 0.86f), justification.left);
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Resume, new Vector2(0.12f, 0.9f), justification.left);
                switch (currentSelection)
                {
                    case  pauseMenuSelect.musicVolume:
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.1f), Color.White, 0.5f, 0);
                        break;
                    case pauseMenuSelect.sfxVolume:
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.15f), Color.White, 0.5f, 0);
                        break;
                    case pauseMenuSelect.cursorSpeed:
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.2f), Color.White, 0.5f, 0);
                        break;
                }
            }

            Game1.gammgr.playerShip.Draw(gameTime, spriteBatch);
        }
        
    }

    public enum drawMode
    {
        Top,
        Bottom,
        Keyline
    }
}
