using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class Solid6 : Enemy
    {
        float f;
        float counterLocal = 0;
        float counter = 0;
        int mode = 0;

        public Solid6(Vector2 inV2, Color topC, Color botC)
        {
            score = 50;
            damageTake = 0.01f;
            drawSize = 0.4f;

            texture = EnemyTextures.Solid6;
            base.Setup(inV2, topC, botC);
        }

        public override void update(double gameTime)
        {
            counterLocal += (0.05f / 16) * (float)gameTime;
            counter += 1f / 16f * (float)gameTime;

            if (counter >= 80)
            {
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

            base.update(gameTime);
        }
    }
}
