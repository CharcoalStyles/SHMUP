using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class MissileManager
    {
        Texture2D barGFX;

        public int missilesLeft;

        Vector2 drawScale;

        float origScale;
        float aimScale;
        float amount = 1;

        public static Effect wave;

        public MissileManager()
        {
            barGFX = new Texture2D(Game1.graphics.GraphicsDevice, 16, 16, false, SurfaceFormat.Color);

            Color[] pixels = new Color[barGFX.Width * barGFX.Height];

            for (int y = 0; y < barGFX.Height; y++)
            {
                for (int x = 0; x < barGFX.Width; x++)
                {
                    if (y > 5 && y < 12)
                        pixels[y * barGFX.Width + x] = Color.White;
                    else
                        pixels[y * barGFX.Width + x] = Color.Transparent;
                }
            }

            barGFX.SetData<Color>(pixels);
        }

        public void Reset()
        {
            missilesLeft = Game1.gammgr.saveGameData.maxMissiles;
            aimScale = ((float)missilesLeft / (float)Game1.gammgr.saveGameData.maxMissiles) * (Game1.gammgr.saveGameData.maxMissiles / 5);
            origScale = 1;
            amount = 0.9667f;
        }

        public void AddMissiles()
        {
            missilesLeft += Game1.gammgr.saveGameData.maxMissiles / 10;
            if (missilesLeft > Game1.gammgr.saveGameData.maxMissiles)
                missilesLeft = Game1.gammgr.saveGameData.maxMissiles;

            origScale = origScale + (aimScale - origScale) * amount;
            aimScale = ((float)missilesLeft / (float)Game1.gammgr.saveGameData.maxMissiles) * (Game1.gammgr.saveGameData.maxMissiles / 5);
            amount = 0;
        }

        public void FireMissiles(Vector2 inPosition)
        {
            if (missilesLeft > 0)
            {
                missilesLeft--;
                origScale = origScale + (aimScale - origScale) * amount;
                aimScale = ((float)missilesLeft / (float)Game1.gammgr.saveGameData.maxMissiles) * (Game1.gammgr.saveGameData.maxMissiles / 5);
                amount = 0;
                for (int i = 0; i < 2; i++)
                {
                    Bullet b;
                    if (Game1.gammgr.unusedBulletList.Count == 0)
                    {
                        b = new Bullet();
                    }
                    else
                    {
                        b = Game1.gammgr.unusedBulletList[0];
                        Game1.gammgr.unusedBulletList.Remove(b);
                    }
                    b.Setup(new Vector2(0.001f, -0.03f + (i * 0.06f)), inPosition, true, true);

                    Game1.gammgr.playerBulletList.Add(b);
                }

                Game1.pclmgr.createEffect(ParticleEffect.missileBar, new Vector2(0.5f - (((float)missilesLeft / (float)Game1.gammgr.saveGameData.maxMissiles) * 0.01f), 0.9f), Color.White);
                Game1.pclmgr.createEffect(ParticleEffect.missileBar, new Vector2(0.5f + (((float)missilesLeft / (float)Game1.gammgr.saveGameData.maxMissiles) * 0.01f), 0.9f), Color.White);
            }
        }

        public void Update()
        {
            if (amount < 1)
            {
                amount += 0.0333f;
                drawScale = new Vector2(origScale + (aimScale - origScale) * amount, 1);
                
            }
            wave.Parameters["InFloat"].SetValue((1 - amount) * Game1.gammgr.saveGameData.maxMissiles);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                //SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            wave.CurrentTechnique.Passes[0].Apply();
            Game1.scrmgr.drawTexture(spriteBatch, barGFX, new Vector2(0.5f, 0.9f), Color.White, drawScale, 0);
            spriteBatch.End();
            spriteBatch.Begin();

        }
    }
}