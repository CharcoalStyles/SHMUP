﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace SHMUP.Enemies
{
    class SoftStar6 : Enemy
    {
        float f;
        float counterLocal = 0;
        float counter = 0;
        int mode = 0;

        public SoftStar6(Vector2 inV2, Color topC, Color botC)
        {
            score = 75;
            damageTake = 0.045f;
            drawSize = 0.4f;
            texture = EnemyTextures.SoftStar6;

            base.Setup(inV2, topC, botC);
        }

        bool shooting = false;
        public override void update(double gameTime)
        {
            counterLocal += (0.05f / 16) * (float)gameTime;
            counter += 1f / 16f * (float)gameTime;


            if (counter >= 80)
            {
                shooting = true;
                counter = 0;
                mode++;
                if (mode == 4)
                    mode = 0;
            }

            switch (mode)
            {
                case 0:
                    f = 0;
                    break;
                case 1:
                    f = (float)Math.Sin(counterLocal) / 450;
                    break;
                case 2:
                    f = 0;
                    break;
                case 3:
                    f = (float)Math.Cos(counterLocal) / 450;
                    break;
            }

            position -= (new Vector2(0.0013f, f) / 16) * (float)gameTime;

            if (shooting && position.X < 1)
            {
                shooting = false;
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
                    b.Setup(new Vector2(-0.002f, o * 0.0002f), position, false, false);

                    Game1.gammgr.enemyBulletList.Add(b);
                }
            }

            base.update(gameTime);
        }
    }
}