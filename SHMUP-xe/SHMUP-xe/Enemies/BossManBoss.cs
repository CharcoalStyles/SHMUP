using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class BossManBoss
    {
        public BossManager.BossData bd;
        List<float> blockHealth = new List<float>();
        List<float> blockShieldColor = new List<float>();
        List<Color> blockCoreColor = new List<Color>();
        List<float> blockShootCounter = new List<float>();

        Vector2 basePosition = new Vector2(1.2f,0.5f);
        Color coreShield = Color.White;
        Color coreCore;
        float coreBulletCounter = 0;
        float coreColorCounter = 0;

        Vector2 aimPosition;
        Vector2 originalPos;

        float coreHealth = 1;
        int coreScore = 0;

        public bool callIn = false;
        bool movedIn = false;
        bool isDead = false;
        bool drawCore = true;

        public Color topColor = Color.RosyBrown; //topColor, passed in form background.
        public Color botColor = Color.RosyBrown; //botColor, passed in form background.


        float blockRadius = Enemies.Enemy.shipTex[0].Width * 0.5f / Game1.graphics.GraphicsDevice.DisplayMode.Width;

        int i;
        int j;
        int counter;
        float f;
        Bullet b;

        public BossManBoss(Color tColor, Color bColor)
        {
            topColor = tColor;
            botColor = bColor;
            
        }

        public void LoadNewBoss(String s)
        {
            bd = BossManager.LoadBoss(s);

            for (i = 0; i < bd.blockOffset.Count; i++)
            {
                blockHealth.Add(1);
                blockShieldColor.Add(1);
                blockCoreColor.Add(Color.Tomato);
                blockShootCounter.Add(0);

                blockCoreColor[i] = new Color(Vector3.Lerp(Vector3.One - topColor.ToVector3(), Vector3.One - botColor.ToVector3(), 1 - (basePosition.X + bd.blockOffset[i].X)));
            }
            coreScore = bd.blockOffset.Count;
        }

        float moveProgression = 0;
        public void Update(Double gameTime)
        {
            coreColorCounter += (float)gameTime;
            for (i = 0; i < bd.blockOffset.Count; i++)
            {
                blockCoreColor[i] = new Color(Vector3.Lerp(Vector3.One - topColor.ToVector3(), Vector3.One - botColor.ToVector3(), basePosition.Y + bd.blockOffset[i].Y));
            }

            if (callIn)
            {
                Game1.gammgr.bossPos = basePosition;

                if (movedIn)
                {
                    if (isDead)
                    {
                        #region isDead
                        counter++;
                        if (counter < 30)
                        {
                            if (counter == 5)
                            {
                                if (blockHealth.Count >= 1)
                                {
                                    counter = 0;
                                    j = Game1.gammgr.r.Next(blockHealth.Count);
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition + bd.blockOffset[j], blockCoreColor[j]);
                                    Game1.sndmgr.playSound(SFX.enemyDead);
                                    Game1.gammgr.AddScore(bd.blockType[j] * 5 + 5, basePosition + bd.blockOffset[j]);

                                    bd.blockOffset.RemoveAt(j);
                                    bd.blockType.RemoveAt(j);
                                    blockCoreColor.RemoveAt(j);
                                    blockHealth.RemoveAt(j);
                                    blockShieldColor.RemoveAt(j);
                                    blockShootCounter.RemoveAt(j);
                                }
                            }
                        }
                        else
                        {
                            switch (counter)
                            {
                                case 30:
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition, coreCore);
                                    Game1.gammgr.AddScore(coreScore * (counter / 30), basePosition);
                                    Game1.sndmgr.playSound(SFX.enemyDead);
                                    break;
                                case 40:
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition, coreCore);
                                    Game1.gammgr.AddScore(coreScore * (counter / 30), basePosition);
                                    Game1.sndmgr.playSound(SFX.enemyDead);
                                    break;
                                case 60:
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition, coreCore);
                                    Game1.gammgr.AddScore(coreScore * (counter / 30), basePosition);
                                    Game1.sndmgr.playSound(SFX.enemyDead);
                                    break;
                                case 90:
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition, coreCore);
                                    Game1.gammgr.AddScore(coreScore * (counter / 30), basePosition);
                                    Game1.sndmgr.playSound(SFX.enemyDead);
                                    break;
                                case 130:
                                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition, coreCore);
                                    Game1.gammgr.AddScore(coreScore * (counter / 30), basePosition);
                                    Game1.sndmgr.playSound(SFX.enemyDead);
                                    drawCore = false;
                                    break;
                                case 300:
                                    Game1.gammgr.bossDead = true;
                                    break;
                            }
                        }
                        #endregion
                    }
                    else
                    {

                        moveProgression += 0.005f + (0.01f / bd.blockOffset.Count);

                        if (moveProgression <= 1)
                        {
                            basePosition = Vector2.Lerp(originalPos, aimPosition, moveProgression);
                        }
                        else if (moveProgression > 1.5f)
                        {
                            aimPosition = new Vector2(((float)Game1.gammgr.r.NextDouble() * 0.2f) + 0.6f, ((float)Game1.gammgr.r.NextDouble() * 0.6f) + 0.2f);
                            moveProgression = 0;
                            originalPos = basePosition;
                        }

                        #region blocks update
                        for (i = 0; i < bd.blockOffset.Count; i++)
                        {
                            #region Shooting
                            blockShootCounter[i] += (float)gameTime / 16;
                            switch ((Enemies.EnemyTextures)bd.blockType[i])
                            {
                                case EnemyTextures.Solid3:
                                    blockShootCounter[i] = 0;
                                    break;
                                case EnemyTextures.Solid4:
                                    blockShootCounter[i] = 0;
                                    break;
                                case EnemyTextures.SoftStar3:
                                    if (blockShootCounter[i] > 140)
                                    {
                                        if (Game1.gammgr.unusedBulletList.Count == 0)
                                        {
                                            b = new Bullet();
                                        }
                                        else
                                        {
                                            b = Game1.gammgr.unusedBulletList[0];
                                            Game1.gammgr.unusedBulletList.Remove(b);
                                        }
                                        b.Setup(new Vector2(-0.003f, 0), basePosition + bd.blockOffset[i], false, false);

                                        Game1.gammgr.enemyBulletList.Add(b);
                                        blockShootCounter[i] = 0;
                                    }
                                    break;
                                case EnemyTextures.SoftStar4:
                                    if (blockShootCounter[i] > 110)
                                    {
                                        for (f = -1; f < 2; f++)
                                        {
                                            if (Game1.gammgr.unusedBulletList.Count == 0)
                                            {
                                                b = new Bullet();
                                            }
                                            else
                                            {
                                                b = Game1.gammgr.unusedBulletList[0];
                                                Game1.gammgr.unusedBulletList.Remove(b);
                                            }
                                            b.Setup(new Vector2(-0.003f, f * 0.001f), basePosition + bd.blockOffset[i], false, false);

                                            Game1.gammgr.enemyBulletList.Add(b);
                                        }
                                        blockShootCounter[i] = 0;
                                    }
                                    break;
                                case EnemyTextures.SoftStar5:
                                    if (blockShootCounter[i] > 110)
                                    {
                                        for (f = -2; f < 3; f++)
                                        {
                                            if (Game1.gammgr.unusedBulletList.Count == 0)
                                            {
                                                b = new Bullet();
                                            }
                                            else
                                            {
                                                b = Game1.gammgr.unusedBulletList[0];
                                                Game1.gammgr.unusedBulletList.Remove(b);
                                            }
                                            b.Setup(new Vector2(-0.003f, f * 0.00075f), basePosition + bd.blockOffset[i], false, false);

                                            Game1.gammgr.enemyBulletList.Add(b);
                                        }
                                        blockShootCounter[i] = 0;
                                    }

                                    break;
                                case EnemyTextures.SoftStar6:
                                    if (blockShootCounter[i] > 70)
                                    {
                                        for (f = -1; f < 2; f++)
                                        {
                                            if (Game1.gammgr.unusedBulletList.Count == 0)
                                            {
                                                b = new Bullet();
                                            }
                                            else
                                            {
                                                b = Game1.gammgr.unusedBulletList[0];
                                                Game1.gammgr.unusedBulletList.Remove(b);
                                            }
                                            b.Setup(new Vector2(-0.003f, f * 0.00075f), basePosition + bd.blockOffset[i], false, false);

                                            Game1.gammgr.enemyBulletList.Add(b);
                                        }
                                        blockShootCounter[i] = 0;
                                    }
                                    break;
                                case EnemyTextures.HardStar3:
                                    if (blockShootCounter[i] > 200)
                                    {
                                        if (Game1.gammgr.unusedBulletList.Count == 0)
                                        {
                                            b = new Bullet();
                                        }
                                        else
                                        {
                                            b = Game1.gammgr.unusedBulletList[0];
                                            Game1.gammgr.unusedBulletList.Remove(b);
                                        }
                                        b.Setup(new Vector2(0.01f, 0.06f), basePosition + bd.blockOffset[i], false, true);

                                        Game1.gammgr.enemyBulletList.Add(b);
                                        blockShootCounter[i] = Game1.gammgr.r.Next(10, 20);
                                    }
                                    break;
                                case EnemyTextures.HardStar4:
                                    if (blockShootCounter[i] > 170)
                                    {
                                        for (f = -1; f < 2; f += 2)
                                        {
                                            if (Game1.gammgr.unusedBulletList.Count == 0)
                                            {
                                                b = new Bullet();
                                            }
                                            else
                                            {
                                                b = Game1.gammgr.unusedBulletList[0];
                                                Game1.gammgr.unusedBulletList.Remove(b);
                                            }
                                            b.Setup(new Vector2(0.01f, 0.06f * f), basePosition + bd.blockOffset[i], false, true);
                                            Game1.gammgr.enemyBulletList.Add(b);
                                        }

                                        blockShootCounter[i] = Game1.gammgr.r.Next(10, 20);
                                    }
                                    break;
                                case EnemyTextures.HardStar5:
                                    if (blockShootCounter[i] > 170)
                                    {
                                        for (f = 1; f < 4; f++)
                                        {
                                            if (Game1.gammgr.unusedBulletList.Count == 0)
                                            {
                                                b = new Bullet();
                                            }
                                            else
                                            {
                                                b = Game1.gammgr.unusedBulletList[0];
                                                Game1.gammgr.unusedBulletList.Remove(b);
                                            }
                                            b.Setup(new Vector2(f * 0.01f, 0.06f), basePosition + bd.blockOffset[i], false, true);
                                            b.Setup(new Vector2(f * 0.01f, -0.06f), basePosition + bd.blockOffset[i], false, true);

                                            Game1.gammgr.enemyBulletList.Add(b);
                                        }
                                        blockShootCounter[i] = Game1.gammgr.r.Next(10, 20);
                                    }
                                    break;
                                case EnemyTextures.HardStar6:
                                    if (blockShootCounter[i] > 130)
                                    {
                                        if (Game1.gammgr.unusedBulletList.Count == 0)
                                        {
                                            b = new Bullet();
                                        }
                                        else
                                        {
                                            b = Game1.gammgr.unusedBulletList[0];
                                            Game1.gammgr.unusedBulletList.Remove(b);
                                        }
                                        b.Setup(new Vector2(0.01f, 0.06f), basePosition + bd.blockOffset[i], false, true);
                                        b.Setup(new Vector2(0.01f, -0.06f), basePosition + bd.blockOffset[i], false, true);

                                        Game1.gammgr.enemyBulletList.Add(b);

                                        blockShootCounter[i] = Game1.gammgr.r.Next(10, 20);
                                    }
                                    break;
                            }
                            #endregion

                            #region shield coloring
                            if (blockShieldColor[i] < 1)
                            {
                                blockShieldColor[i] += 0.1f;
                            }
                            else
                            {
                                blockShieldColor[i] = 1;
                            }
                            #endregion

                            #region Damaging
                            for (j = 0; j < Game1.gammgr.playerBulletList.Count; j++)
                            {
                                if (Game1.gammgr.playerBulletList[j].CheckHit(basePosition + bd.blockOffset[i], blockRadius))
                                {
                                    blockTakeDamage(i);
                                    Game1.sndmgr.playSound(SFX.enemyIsHit);
                                    Game1.gammgr.playerBulletList[j].live = false;
                                    Game1.gammgr.unusedBulletList.Add(Game1.gammgr.playerBulletList[j]);
                                    Game1.gammgr.playerBulletList.RemoveAt(j);
                                }
                            }

                            foreach (PlayerBuffs tpb in Game1.gammgr.playerBuffs)
                            {
                                if (Vector2.Distance(basePosition + bd.blockOffset[i], tpb.position) < blockRadius + tpb.shipRadius)
                                {
                                    blockTakeDamage(i);
                                    tpb.takeDamage(1.25f);
                                }
                            }
                            #endregion

                            #region Christian movement
                            ////Movement Code!
                            //blockAccel[i] *= 0.95f;
                            //tV2 = (basePosition - blockPosition[i]);
                            //blockAccel[i] += tV2 * 0.0033f;

                            //for (j = 0; j < blockPosition.Count; j++)
                            //{
                            //    if (Vector2.Distance(blockPosition[i], blockPosition[j]) < 0.02f)
                            //    {
                            //        if (blockPosition[i] != blockPosition[j])
                            //        {
                            //            blockAccel[i] -= (blockPosition[j] - blockPosition[i]) * (0.1f - Vector2.Distance(blockPosition[i], blockPosition[j]));
                            //        }
                            //    }
                            //}


                            //if (Vector2.Distance(blockPosition[i], basePosition) < 0.02f)
                            //{
                            //    blockAccel[i] -= (basePosition - blockPosition[i]) * (0.1f - Vector2.Distance(blockPosition[i], basePosition));
                            //}

                            //blockPosition[i] += blockAccel[i];
                            #endregion

                            if (blockHealth[i] <= 0)
                            {
                                Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, basePosition + bd.blockOffset[i], blockCoreColor[i]);
                                Game1.sndmgr.playSound(SFX.enemyDead);
                                Game1.gammgr.AddScore(bd.blockType[i] * 10 + 5, basePosition + bd.blockOffset[i]);

                                bd.blockOffset.RemoveAt(i);
                                bd.blockType.RemoveAt(i);
                                blockCoreColor.RemoveAt(i);
                                blockHealth.RemoveAt(i);
                                blockShieldColor.RemoveAt(i);
                                blockShootCounter.RemoveAt(i);
                            }
                        }
                        #endregion

                        #region Core Update
                        for (j = 0; j < Game1.gammgr.playerBulletList.Count; j++)
                        {
                            if (Game1.gammgr.playerBulletList[j].CheckHit(basePosition, blockRadius))
                            {
                                coreHealth -= 0.075f;

                                if (coreHealth <= 0)
                                {
                                    isDead = true;
                                    i = 9;
                                }
                            }
                        }

                        foreach (PlayerBuffs tpb in Game1.gammgr.playerBuffs)
                        {
                            if (Vector2.Distance(basePosition, tpb.position) < tpb.shipRadius)
                            {
                                tpb.takeDamage(1.5f);
                                coreHealth -= 0.1f;
                            }
                        }

                        coreBulletCounter += (float)gameTime / 16;
                        if (coreBulletCounter > 70)
                        {
                            for (f = -1; f < 2; f++)
                            {
                                if (Game1.gammgr.unusedBulletList.Count == 0)
                                {
                                    b = new Bullet();
                                }
                                else
                                {
                                    b = Game1.gammgr.unusedBulletList[0];
                                    Game1.gammgr.unusedBulletList.Remove(b);
                                }
                                b.Setup(new Vector2(-0.003f, f * 0.001f), basePosition, false, false);

                                Game1.gammgr.enemyBulletList.Add(b);
                            }
                            coreBulletCounter = 0;
                        }
                        #endregion
                    }
                }
                else
                {
                    basePosition += new Vector2(-0.002f, 0);
                    if (basePosition.X <= 0.8f)
                    {
                        movedIn = true;
                        aimPosition = new Vector2(((float)Game1.gammgr.r.NextDouble() * 0.2f) + 0.6f,((float)Game1.gammgr.r.NextDouble() * 0.6f) + 0.2f);
                        originalPos = basePosition;
                    }
                }

                coreCore = new Color(((float)Math.Sin(coreColorCounter / 300) + 1) / 2,
                    ((float)Math.Cos(coreColorCounter / 1000) + 1) / 2,
                    ((float)Math.Sin(coreColorCounter / 1000) + 1) / 2);

            }
        }

        void blockTakeDamage(int blockIndex)
        {
            #region which enemy
            switch ((Enemies.EnemyTextures)bd.blockType[blockIndex])
            {
                case EnemyTextures.Solid3:
                    f = 0.1f;
                    break;
                case EnemyTextures.Solid4:
                    f = 0.05f;
                    break;
                case EnemyTextures.SoftStar3:
                    f = 0.1f;
                    break;
                case EnemyTextures.SoftStar4:
                    f = 0.075f;
                    break;
                case EnemyTextures.SoftStar5:
                    f = 0.05f;
                    break;
                case EnemyTextures.SoftStar6:
                    f = 0.0333f;
                    break;
                case EnemyTextures.HardStar3:
                    f = 0.1f;
                    break;
                case EnemyTextures.HardStar4:
                    f = 0.075f;
                    break;
                case EnemyTextures.HardStar5:
                    f = 0.05f;
                    break;
                case EnemyTextures.HardStar6:
                    f = 0.0333f;
                    break;
                case EnemyTextures.Solid5:
                    f = 0.03f;
                    break;
                case EnemyTextures.Solid6:
                    f = 0.015f;
                    break;
            }
            #endregion

            blockHealth[i] -= f * 3;
            blockShieldColor[i] = 0.25f;
        }

        public void Draw(SpriteBatch spriteBatch, drawMode dm, GameTime gameTime)
        {
            if (callIn && drawCore)
            {
                switch (dm)
                {
                    case drawMode.Keyline:
                        Game1.scrmgr.drawTexture(spriteBatch, Enemy.shipTex[1], basePosition, Color.Black, 0.5f - ((1 - coreHealth) * 0.16f), 0);

                        for (i = 0; i < bd.blockOffset.Count; i++)
                        {
                            Game1.scrmgr.drawTexture(spriteBatch, Enemy.shipTex[bd.blockType[i]], basePosition + bd.blockOffset[i], Color.Black, 0.5f - ((1 - blockHealth[i]) * 0.16f), ((float)gameTime.TotalGameTime.TotalMilliseconds / 100) * (2 * bd.blockOffset[i].Y));
                        }
                        break;
                    case drawMode.Bottom:
                        Game1.scrmgr.drawTexture(spriteBatch, Enemy.shipTex[1], basePosition, coreShield, 0.5f - ((1 - coreHealth) * 0.16f) - 0.05f, 0);

                        for (i = 0; i < bd.blockOffset.Count; i++)
                        {
                            Game1.scrmgr.drawTexture(spriteBatch, Enemy.shipTex[bd.blockType[i]], basePosition + bd.blockOffset[i], new Color(blockShieldColor[i], blockShieldColor[i], blockShieldColor[i]), 0.5f - ((1 - blockHealth[i]) * 0.16f) - 0.05f, ((float)gameTime.TotalGameTime.TotalMilliseconds / 100) * (2 * bd.blockOffset[i].Y) );
                        }
                        break;
                    case drawMode.Top:
                        Game1.scrmgr.drawTexture(spriteBatch, Enemy.shipTex[1], basePosition, coreCore, 0.5f - 0.175f, 0);

                        for (i = 0; i < bd.blockOffset.Count; i++)
                        {
                            Game1.scrmgr.drawTexture(spriteBatch, Enemy.shipTex[bd.blockType[i]], basePosition + bd.blockOffset[i], blockCoreColor[i], 0.5f - 0.175f, ((float)gameTime.TotalGameTime.TotalMilliseconds / 100) * (2 * bd.blockOffset[i].Y));
                        }
                        break;
                }

            }
        }
    }
}
