using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class SoftStar4 : Enemy
    {
        float counter = 39;
        int mode = -1;
        float f;
        float shootCounter;

        public SoftStar4(Vector2 inV2, Color topC, Color botC)
        {
            score = 55;
            damageTake = 0.05f;
            drawSize = 0.5f;
            texture = EnemyTextures.SoftStar4;

            //inV2.Y = inV2.Y - (40 * 0.0015f);

            base.Setup(inV2, topC, botC);
        }

        public override void update(double gameTime)
        {
            counter += 1f / 16f * (float)gameTime;

            if (counter >= 40)
            {
                counter = 0;
                mode++;
                switch (mode)
                {
                    case 0:
                        f = -0.0008f;
                        break;
                    case 1:
                        f = 0.0008f;
                        break;
                    case 2:
                        f = 0.0008f;
                        break;
                    case 3:
                        f = -0.0008f;
                        mode = -1;
                        break;
                }
            }

            position -= (new Vector2(0.0015f, f) / 16) * (float)gameTime;

            shootCounter += (0.0065f / 16) * (float)gameTime;

            if (shootCounter >= 1 && position.X < 1)
            {
                shootCounter = 0;

                for (int o = -1; o < 2; o++)
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
                    b.Setup(new Vector2(-0.0025f, o * 0.0012f), position, false, false);

                    Game1.gammgr.enemyBulletList.Add(b);
                }
            }

            base.update(gameTime);
        }

        public override void takeDamage(float mod)
        {
            base.takeDamage(mod);
        }
    }
}
