using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace SHMUP.Enemies
{
    class SoftStar5 : Enemy
    {
        float counter = 119;
        int mode = -1;
        float f;

        public SoftStar5(Vector2 inV2, Color topC, Color botC)
        {
            score = 65;
            damageTake = 0.045f;
            drawSize = 0.4f;
            texture = EnemyTextures.SoftStar5;

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
                    for (int o = -2; o < 3; o++)
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
                        b.Setup(new Vector2(-0.0022f, o * 0.00012f), position, false, false);

                        Game1.gammgr.enemyBulletList.Add(b);
                    }
                }
            }

            position -= (new Vector2(0.0014f, f) / 16) * (float)gameTime;

            base.update(gameTime);
        }
    }
}
