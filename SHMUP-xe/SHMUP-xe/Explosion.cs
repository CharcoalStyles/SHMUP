using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class Explosion
    {
        Vector2 position;
        float radius;
        float collisionRadius;
        Color c;

        public static float maxRadius = 0.25f;
        public static float radiusGrow = 0.01f;

        public bool finished;

        public Explosion()
        {
        }

        public void Reset(Vector2 inV2)
        {
            position = inV2;
            radius = 0;
            finished = false;
            c = Color.White;
            maxRadius = (float)Game1.gammgr.saveGameData.missileExpolsion;
            radiusGrow = maxRadius / 30;

            Game1.inpmgr.playerOneInput.leftRumble = 0.9f;
        }

        public void Update()
        {
            radius += radiusGrow + (float)(Game1.gammgr.r.NextDouble() / 50);
            collisionRadius = PlayerBuffs.tex.Width * radius / Game1.graphics.GraphicsDevice.DisplayMode.Width;

            c = new Color(1 - ((radius / maxRadius) * 0.4f), 0.75f - ((radius / maxRadius) * 0.75f), 0, 1 - (radius / maxRadius));

            if (radius >= maxRadius)
            {
                finished = true;
            }
        }

        public void checkHit(Enemies.Enemy tEnemy)
        {
            if (Vector2.Distance(position, tEnemy.position) <= (tEnemy.shipRadius + collisionRadius) * 0.8f)
            {
                tEnemy.takeDamage(0.1f * (1 - collisionRadius));
            }
        }

        public void Draw(SpriteBatch spiteBatch)
        {
            Game1.scrmgr.drawTexture(spiteBatch, PlayerBuffs.tex, position, c, radius, 0);
        }
    }
}
