using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class LevelHealth
    {
        public List<Vector2> barrierPosition;
        List<Color> barrierColor;
        List<Color> shieldColor;
        public List<float> barrierHealth;
        List<bool> barrierIsDead;
        List<float> barrierSizeMod;

        public static Texture2D wallTex;

        int o;
        public bool shieldFailing = false;

        float sizeMod;

        public LevelHealth()
        {
            barrierPosition = new List<Vector2>();
            barrierColor = new List<Color>();
            shieldColor = new List<Color>();
            barrierHealth = new List<float>();
            barrierIsDead = new List<bool>();
            barrierSizeMod = new List<float>();
        }

        public void reset()
        {
            o = Game1.gammgr.saveGameData.levelHeathOrbs;
            shieldFailing = false;

            barrierPosition.Clear();
            barrierPosition.Clear();
            barrierColor.Clear();
            barrierHealth.Clear();
            barrierIsDead.Clear();
            barrierSizeMod.Clear();

            for (int i = 0; i < o; i++)
            {
                barrierColor.Add(Color.White);
                shieldColor.Add(Color.White);
                barrierHealth.Add(1);
                barrierIsDead.Add(false);
                barrierSizeMod.Add(i);
            }

            sizeMod = 4.5f / (float)barrierColor.Count;
            for (int i = 0; i < o; i++)
            {
                barrierPosition.Add(new Vector2(0.01f, ((0.85f / o) * i) + 0.1f));
            }
        }

        public bool Damage(int inIndex)
        {
            bool ret = false;

            AwardsManager.wallHit = true;

            if (!barrierIsDead[inIndex])
            {
                barrierHealth[inIndex] -= (1 - ((float)Game1.gammgr.saveGameData.levelHeathOrbs / 25)) / 5; 
                ret = true;
                if (barrierHealth[inIndex] < 0.3f)
                {
                    shieldFailing = true;
                }
            }
            return ret;
        }

        public void Heal()
        {
            bool shieldStillFailing = false;
            for (int i = 0; i < barrierHealth.Count; i++)
            {
                if (barrierIsDead[i])
                {
                    barrierIsDead[i] = false;
                    barrierHealth[i] = 0.01f;
                }

                if (barrierHealth[i] < 1)
                {
                    barrierHealth[i] += 0.2f;
                    if (barrierHealth[i] > 1)
                        barrierHealth[i] = 1;
                }

                if (barrierHealth[i] < 0.3f)
                    shieldStillFailing = true;
            }

            if (shieldFailing && !shieldStillFailing)
                shieldFailing = false;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < barrierHealth.Count; i++)
            {

                barrierSizeMod[i] += 0.01f;

                if (shieldFailing)
                {
                    shieldColor[i] = new Color(barrierHealth[i],
                        barrierHealth[i] * (((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 100 + i) * 2 + 1) * 0.5f),
                        barrierHealth[i] * (((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 100 + i) * 2 + 1) * 0.5f), 1);
                }
                else
                {
                    if (barrierHealth[i] < 1)
                    {
                        barrierColor[i] = new Color((barrierHealth[i] * 0.5f) + 0.5f, barrierHealth[i] * 0.9f,
                            barrierHealth[i] * 0.7f, 1);
                    }
                    else
                    {
                        barrierColor[i] = Color.White;
                    }
                    shieldColor[i] = Color.White;
                }
                if (barrierHealth[i] <= 0)
                {
                    barrierIsDead[i] = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, drawMode dm)
        {
            for (int i = 0; i < barrierPosition.Count; i++)
            {
                if (!barrierIsDead[i])
                {
                    switch (dm)
                    {
                        case drawMode.Keyline:
                            Game1.scrmgr.drawTexture(spriteBatch, wallTex, barrierPosition[i], Color.Black, sizeMod + (0.1f * barrierHealth[i]) + (((float)Math.Sin(barrierSizeMod[i]) * 2 - 1) * 0.01f), 0);
                            break;
                        case drawMode.Bottom:
                            Game1.scrmgr.drawTexture(spriteBatch, wallTex, barrierPosition[i], shieldColor[i], sizeMod - 0.05f + (0.1f * barrierHealth[i]) + (((float)Math.Sin(barrierSizeMod[i]) * 2 - 1) * 0.01f), 0);
                            break;
                        case drawMode.Top:
                            Game1.scrmgr.drawTexture(spriteBatch, wallTex, barrierPosition[i], barrierColor[i], sizeMod - 0.15f, 0);
                            break;
                    }
                }
                
            }
        }
    }
}
