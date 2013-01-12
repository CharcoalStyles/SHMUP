using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class Solid4 : Enemy
    {
        float counter = 39;
        int mode = -1;
        float f;

        public Solid4(Vector2 inV2, Color topC, Color botC) 
        {
            score = 30;
            damageTake = 0.07f;
            drawSize = 0.4f;

            //inV2.Y = inV2.Y - (40 * 0.0015f);

            texture = EnemyTextures.Solid4;
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

            base.update(gameTime);
        }
    }
}
