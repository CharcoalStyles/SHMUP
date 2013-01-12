using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class Bullet
    {
        public static Texture2D playerBulletTex;
        public static Texture2D enemyBulletTex;

        public Vector2 position;
        public Vector2 velocity;

        float maxSpeed = 0.016f;
        Vector2 accel;
        float accelmod = 0.9f;
        Vector2 tV2;

        public bool live = false;

        public bool isPlayerBullet = true;

        public bool isSeeker = false;

        Color c;

        public Vector2 seekPosition;
        public int seekEnemy;
        public Vector2 tempT2;
        public bool nearTarget;
        public float rotate;
        int seektime;

        int cou;
        int soundCounter;
        int soundSwitch;

        public Bullet()
        {
        }

        public void Setup(Vector2 inVelocity, Vector2 initPos,bool pBullet, bool isSeek)
        {
            position = initPos;
            velocity = inVelocity;
            isSeeker = isSeek;
            isPlayerBullet = pBullet;
            live = true;

            c = Color.White;

            if (isSeek)
            {
                if (isPlayerBullet)
                {
                    seekPosition = GetNewEnemyPosition();
                    AwardsManager.FiredNewMisile();
                }
                else
                    seekPosition = GetNewBuffPosition();

                accel = inVelocity;
                nearTarget = false; 
                maxSpeed = 0.015f;
                seektime = 0;
            }
            else
            {
                seekPosition = Vector2.Zero;
            }
            cou = 0;
            soundCounter = 0;
            soundSwitch = Game1.gammgr.r.Next(3, 6);
        }

        public Vector2 GetNewEnemyPosition()
        {
            Vector2 outPos = new Vector2(2f, 0.5f);
            if (Game1.gammgr.enemies.Count > 0)
            {
                seekEnemy = Game1.gammgr.r.Next(Game1.gammgr.enemies.Count);
                outPos = ((Enemies.Enemy)Game1.gammgr.enemies[seekEnemy]).position;
            }
            return outPos;
        }

        public Vector2 GetNewBuffPosition()
        {
            Vector2 outPos = new Vector2(2f, 0.5f);
            if (Game1.gammgr.playerBuffs.Count > 0)
            {
                seekEnemy = Game1.gammgr.r.Next(Game1.gammgr.playerBuffs.Count);
                outPos = ((PlayerBuffs)Game1.gammgr.playerBuffs[seekEnemy]).position;
            }
            return outPos;
        }

        public bool update(double gameTime)
        {

            bool retBool = false;

            if (isSeeker)
            {
                cou++;
                soundCounter++;
                Game1.pclmgr.createEffect(ParticleEffect.seekTrail, position, Color.WhiteSmoke);

                maxSpeed -= 0.00006f;
                if (maxSpeed < 0.003f)
                    maxSpeed = 0.003f;
                    
                if (cou == 10)
                {
                    if (Game1.gammgr.bossIn)
                    {
                        if (isPlayerBullet)
                        {
                            seekPosition = Game1.gammgr.bossPos;
                        }
                        else if (seekEnemy < Game1.gammgr.playerBuffs.Count && !isPlayerBullet)
                        {
                            seekPosition = ((PlayerBuffs)Game1.gammgr.playerBuffs[seekEnemy]).position;
                            seektime++;
                            if (seektime == 5)
                            {
                                nearTarget = true;
                            }
                        }
                    }
                    else
                    {
                        if (seekEnemy < Game1.gammgr.enemies.Count && isPlayerBullet)
                        {
                            seekPosition = ((Enemies.Enemy)Game1.gammgr.enemies[seekEnemy]).position;
                        }
                        else if (seekEnemy < Game1.gammgr.playerBuffs.Count && !isPlayerBullet)
                        {
                            seekPosition = ((PlayerBuffs)Game1.gammgr.playerBuffs[seekEnemy]).position;
                            seektime++;
                            if (seektime == 5)
                            {
                                nearTarget = true;
                            }
                        }
                        else
                        {
                            nearTarget = true;
                        }
                    }
                    cou = 0;
                }
                if (soundCounter == soundSwitch)
                {
                    Game1.sndmgr.playSound(SFX.rocket, position.X * 2 - 1);
                    soundCounter = 0;
                    soundSwitch = Game1.gammgr.r.Next(3, 6);
                }
                //Movement Code!
                accel *= accelmod;
                if (nearTarget)
                {
                    tV2 = tempT2;
                }
                else
                {
                    tV2 = (seekPosition - position);
                }
                tempT2 = tV2;
                tV2.Normalize();
                tV2 *= 0.1f;
                accel += tV2 * maxSpeed;

                if(isPlayerBullet)
                    position += accel;
                else
                    position += (accel / 16) * (float)gameTime;

                if (Vector2.Distance(position, seekPosition) < 0.025f)
                {
                    nearTarget = true;
                    if (Vector2.Distance(position, seekPosition) < 0.01f)
                        retBool = true;
                }
                rotate = (float)Math.Atan2(accel.Y, accel.X);// +3.14150f / 2;

            }
            else
            {
                if (isPlayerBullet)
                {
                    position += velocity;
                }
                else
                {
                    position += (velocity / 16) * (float)gameTime;
                }
            }

            if (isPlayerBullet)
            {
                if (position.X > 1.1f)
                {
                    retBool = true;
                }
                if (isSeeker)
                {
                    if (position.X < -0.1f ||
                        position.Y > 1.4f ||
                        position.Y < -0.4f)
                    {
                        retBool = true;
                    }
                }
            }
            else
            {
                if (position.X < -0.1f ||
                    position.Y > 1.4f ||
                    position.Y < -0.4f)
                {
                    retBool = true;
                }
                if (!isSeeker)
                    rotate = (float)Math.Atan2(velocity.Y, velocity.X);
            }
            return retBool;
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            if (isSeeker)
            {
                    Game1.scrmgr.drawTexture(spriteBatch, playerBulletTex, position, c, new Vector2(2f, 0.5f), rotate);
            }
            else
            {
                if (isPlayerBullet)
                    Game1.scrmgr.drawTexture(spriteBatch, playerBulletTex, position, c, new Vector2(2f, 0.25f), 0);
                else
                    Game1.scrmgr.drawTexture(spriteBatch, playerBulletTex, position, c, new Vector2(1f, 0.5f), rotate);
            }

        }


        public bool CheckHit(Vector2 inPosition, float shipRad)
        {
            bool returnBool = false;

            if (isSeeker)
            {
                if (Vector2.Distance(inPosition, position) < (shipRad + 0.01f) * 0.8f)
                {
                    returnBool = true;
                    Game1.pclmgr.createEffect(ParticleEffect.missileSplosion, position, new Color((float)Game1.gammgr.r.NextDouble() * 0.5f + 0.5f, (float)Game1.gammgr.r.NextDouble() * 0.3f + 0.3f, 0));
                    Game1.sndmgr.playSound(SFX.MissileHit);

                    Explosion e;
                    if (Game1.gammgr.unusedExplosionList.Count == 0)
                    {
                        e = new Explosion();
                    }
                    else
                    {
                        e = Game1.gammgr.unusedExplosionList[0];
                        Game1.gammgr.unusedExplosionList.Remove(e);
                    }
                    e.Reset(position);

                    Game1.gammgr.explosionList.Add(e);
                }
            }
            else
            {
                if (Vector2.Distance(inPosition, position) < shipRad)
                {
                    returnBool = true;
                }
            }
            return returnBool;
        }
    }
}
