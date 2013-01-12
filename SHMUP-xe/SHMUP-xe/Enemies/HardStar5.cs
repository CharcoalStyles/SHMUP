using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace SHMUP.Enemies
{
    class HardStar5: Enemy
    {
        float counter = 119;
        int mode = -1;
        float f;

        public HardStar5(Vector2 inV2, Color topC, Color botC)
        {
            score = 75;
            damageTake = 0.055f;
            drawSize = 0.4f;
            texture = EnemyTextures.HardStar5;

            base.Setup(inV2, topC, botC);
        }

        public override void update(double gameTime)
        {
            counter += 1f / 16f * (float)gameTime;

            if (counter >= 120)
            {
                counter = 0;
                mode++;
                switch (mode)
                {
                    case 0:
                        f = 0;
                        break;
                    case 1:
                        f = 0.001f;
                        break;
                    case 2:
                        f = 0;
                        break;
                    case 3:
                        f = -0.002f;
                        mode = -1;
                        break;
                }

                if (position.X < 1)
                {
                    for (int o = 1; o < 3; o++)
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
                        b.Setup(new Vector2(0.01f, 0.03f * o), position, false, true);

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
                        b.Setup(new Vector2(0.01f, -0.03f * o), position, false, true);

                        Game1.gammgr.enemyBulletList.Add(b);
                    }
                }
            }

            position -= (new Vector2(0.0014f, f) / 16) * (float)gameTime;

            base.update(gameTime);
        }
    }
}
