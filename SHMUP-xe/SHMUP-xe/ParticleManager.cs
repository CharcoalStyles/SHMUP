using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class ParticleManager
    {
        List<Particles> particleList = new List<Particles>();
        List<Particles> unusedParticleList = new List<Particles>();

        public Texture2D particleTexture;//This is the texture our particle will use.

        Random random = new Random();//Our random number generator.

        int i;
        byte alpha;

        //int particlesRecentlyMade;

        public ParticleManager()
        {
            particleTexture = Game1.content.Load<Texture2D>("Background/Circle");

            for (i = 0; i < 20; i++)
            {
                unusedParticleList.Add(new Particles());
            }
        }

        public void createEffect(ParticleEffect pf, Vector2 position, Color color)
        {
            Particles p = new Particles();
            float pMod = (float)SettingsManager.settings.particleMod * Math.Max(0.01f, Math.Min(1, (float)500 / (float)particleList.Count));

            switch (pf)
            {
                case ParticleEffect.enemyExplosion:
                    #region ParticleEffect.enemyExplosion
                    for (i= 0; i< Math.Max(30 * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(1000,1500);
                        p.halfLife = random.Next(600, 1200);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.0009f, ((float)random.NextDouble() * 2 - 1) * 0.0009f);

                        p.rot = 0;
                        p.rotspeed = (float)random.NextDouble();

                        p.size = new Vector2(1,1);
                        p.growSpeed = (float)random.Next(100, 500) / 20000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.pbExplosion:
                    #region ParticleEffect.pbExplosion
                    for (i = 0; i < Math.Max(50 * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(1700, 2500);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.0005f, ((float)random.NextDouble() * 2 - 1) * 0.0005f);

                        p.rot = 0;
                        p.rotspeed = (float)random.NextDouble();

                        p.size = new Vector2(1,1);
                        p.growSpeed = (float)random.Next(50, 400) / 17000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.enemyDamage:
                    #region ParticleEffect.enemyDamage
                    for (i = 0; i < Math.Max(Game1.gammgr.r.Next(5, 10) * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(500, 1000);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() / 2 + 0.5f) * 0.003f, ((float)random.NextDouble() * 2 - 1) * 0.003f);

                        p.rot = (float)Math.Atan2(p.velocity.Y, p.velocity.X);
                        p.rotspeed = 0;//(float)random.NextDouble();

                        p.size = new Vector2(0.75f, 0.25f);
                        p.growSpeed = (float)random.Next(200, 500) / 15000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.pbDamage:
                    #region ParticleEffect.pbDamage
                    for (i = 0; i < Math.Max(Game1.gammgr.r.Next(5, 10) * pMod,1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(500, 1000);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() / 2 + 0.5f) * -0.003f, ((float)random.NextDouble() * 2 - 1) * 0.002f);

                        p.rot = 0;
                        p.rotspeed = (float)random.NextDouble();

                        p.size = new Vector2(0.5f, 0.5f);
                        p.growSpeed = (float)random.Next(100, 1000) / 15000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.seekTrail:
                    #region ParticleEffect.seekTrail
                    for (i = 0; i < Math.Max(4 * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(100, 300);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() / 2 + 0.5f) * -0.001f, ((float)random.NextDouble() * 2 - 1) * 0.001f);

                        p.rot = 0;
                        p.rotspeed = (float)random.NextDouble();

                        p.size = new Vector2(0.2f, 0.2f);
                        p.growSpeed = (float)random.Next(1000, 3000) / 25000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.enemyTeleport:
                    #region ParticleEffect.enemyTeleport
                    for (i = 0; i < Math.Max(Game1.gammgr.r.Next(15, 20) * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = Color.White;

                        p.lifespan = random.Next(500, 1000);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.0002f, ((float)random.NextDouble() * 2 - 1) * 0.004f);

                        p.rot = (float)Math.Atan2(p.velocity.Y, p.velocity.X);
                        p.rotspeed = 0;//(float)random.NextDouble();

                        p.size = new Vector2(0.75f, 0.25f);
                        p.growSpeed = (float)random.Next(200, 500) / 15000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.missileBar:
                    #region ParticleEffect.missileBar
                    for (i = 0; i < Math.Max(2 * pMod,1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = Color.White;

                        p.lifespan = random.Next(500, 1000);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.01f, ((float)random.NextDouble() * 2 - 1) * 0.0002f);

                        p.rot = (float)Math.Atan2(p.velocity.Y, p.velocity.X);
                        p.rotspeed = 0;//(float)random.NextDouble();

                        p.size = new Vector2(0.6f, 0.3f);
                        p.growSpeed = (float)random.Next(100, 200) / 20000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.enemyBreach:
                    #region ParticleEffect.enemyBreach
                    for (i = 0; i < Math.Max(Game1.gammgr.r.Next(100, 200) * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(2000, 3000);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.0333f, ((float)random.NextDouble() * 2 - 1) * 0.002f);

                        p.rot = (float)Math.Atan2(p.velocity.Y, p.velocity.X);
                        p.rotspeed = 0;//(float)random.NextDouble();

                        p.size = new Vector2(1.5f, 0.75f);
                        p.growSpeed = (float)random.Next(100, 200) / 30000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.missileSplosion:
                    #region missileSplosion
                    for (i = 0; i < Math.Max(Game1.gammgr.r.Next(15, 20) * pMod, 1); i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(500, 750);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.007f, ((float)random.NextDouble() * 2 - 1) * 0.007f);

                        p.rot = (float)Math.Atan2(p.velocity.Y, p.velocity.X);
                        p.rotspeed = 0;//(float)random.NextDouble();

                        p.size = new Vector2(1.25f, 0.5f);
                        p.growSpeed = (float)random.Next(100, 200) / 30000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
                case ParticleEffect.pickUp:
                    #region pickUp
                    for (i = 0; i < Math.Max(Game1.gammgr.r.Next(15, 20) * pMod, 1) ; i++)
                    {
                        if (unusedParticleList.Count == 0)
                        {
                            p = new Particles();
                        }
                        else
                        {
                            p = unusedParticleList[0];
                            unusedParticleList.Remove(p);
                        }

                        p.color = color;

                        p.lifespan = random.Next(1500, 2500);

                        p.particleEffectType = pf;

                        p.position = position;
                        p.initPos = position;
                        p.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 0.007f, ((float)random.NextDouble() * 2 - 1) * 0.007f);

                        p.rot = (float)Math.Atan2(p.velocity.Y, p.velocity.X);
                        p.rotspeed = 0;//(float)random.NextDouble();

                        p.size = new Vector2(1.25f, 0.5f);
                        p.growSpeed = (float)random.Next(100, 200) / 30000;

                        p.timeAlive = 0;

                        p.used = true;

                        particleList.Add(p);
                    }
                    #endregion
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Game1.gammgr.isPaused)
            {

                for (i = 0; i < particleList.Count; i++)
                {
                    if (particleList[i].used)
                    {
                        particleList[i].timeAlive += gameTime.ElapsedGameTime.Milliseconds;

                        if (particleList[i].timeAlive >= particleList[i].lifespan)
                        {
                            particleList[i].used = false;
                            unusedParticleList.Add(particleList[i]);
                            particleList.RemoveAt(i);
                        }
                        else
                        {
                            particleList[i].position += particleList[i].velocity;
                            particleList[i].rot += particleList[i].rotspeed;
                            particleList[i].size += particleList[i].size * particleList[i].growSpeed;

                            if (particleList[i].particleEffectType == ParticleEffect.pickUp)
                                particleList[i].aimFrom = particleList[i].position;

                            alpha = (byte)((1 - (((double)particleList[i].timeAlive / (double)particleList[i].lifespan)) * byte.MaxValue) + 2);
                            particleList[i].color.A = alpha;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (i = 0; i < particleList.Count; i++)
            {
                if (particleList[i].used)
                {
                    Game1.scrmgr.drawTexture(spriteBatch, particleTexture, particleList[i].position, particleList[i].color, 0.02f * particleList[i].size, particleList[i].rot);
                }
            }
        }
    }
}
