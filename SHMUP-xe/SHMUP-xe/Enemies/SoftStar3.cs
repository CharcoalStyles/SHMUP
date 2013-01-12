using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class SoftStar3 : Enemy
    {
        public SoftStar3(Vector2 inV2, Color topC, Color botC)
        {
            score = 40;
            damageTake = 0.065f;
            drawSize = 0.5f;
            texture = EnemyTextures.SoftStar3;

            base.Setup(inV2, topC, botC);
        }
        
        public override void update(Double gameTime)
        {
            position -= new Vector2((0.0016f / 16) * (float)gameTime, 0);
            base.update(gameTime);
        }

        public override void takeDamage(float mod)
        {
            if (!isDead)
            {
                base.takeDamage(mod);

                if (isDead)
                {
                    for (int o = 0; o < 5; o++)
                    {
                        float f = -0.0006f + (o * 0.00033f);

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
                        b.Setup(new Vector2(-0.003f, f), position, false, false);

                        Game1.gammgr.enemyBulletList.Add(b);
                    }
                }
            }
        }
    }
}
