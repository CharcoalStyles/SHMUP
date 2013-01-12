using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class Solid3 : Enemy
    {
        public Solid3(Vector2 inV2, Color topC, Color botC)
        {
            score = 20;
            damageTake = 0.1f;
            drawSize = 0.4f;

            texture = EnemyTextures.Solid3;
            base.Setup(inV2, topC, botC);
        }

        public override void update(double gameTime)
        {
            position -= new Vector2((0.0016f / 16) * (float)gameTime, 0);
            base.update(gameTime);
        }

        public override void draw(SpriteBatch spriteBatch, drawMode dm)
        {
            base.draw(spriteBatch, dm);
        }
    }
}
