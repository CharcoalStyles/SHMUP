using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class HardStar3 : Enemy
    {
        public HardStar3(Vector2 inV2, Color topC, Color botC)
        {
            score = 60;
            damageTake = 0.065f;
            drawSize = 0.5f;
            texture = EnemyTextures.HardStar3;

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
                    Bullet b;

                    //Missile 1
                    if (Game1.gammgr.unusedBulletList.Count == 0)
                    {
                        b = new Bullet();
                    }
                    else
                    {
                        b = Game1.gammgr.unusedBulletList[0];
                        Game1.gammgr.unusedBulletList.Remove(b);
                    }
                    b.Setup(new Vector2(0.01f, 0.06f), position, false, true);

                    Game1.gammgr.enemyBulletList.Add(b);

                    //missile 2
                    if (Game1.gammgr.unusedBulletList.Count == 0)
                    {
                        b = new Bullet();
                    }
                    else
                    {
                        b = Game1.gammgr.unusedBulletList[0];
                        Game1.gammgr.unusedBulletList.Remove(b);
                    }
                    b.Setup(new Vector2(0.01f, -0.06f), position, false, true);

                    Game1.gammgr.enemyBulletList.Add(b);
                }
            }
        }
    }
}
