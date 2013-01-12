using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    class BulRep : Enemy
    {
        private bulletRepeller bulRep;
        int MovementMode;

        float counter= 0;
        int mode = 0;
        float f = 0;
        float counterLocal = 0;

        public BulRep(Vector2 inV2, Color topC, Color botC, int mm)
        {
            score = 60;
            damageTake = 0.065f;
            drawSize = 0.5f;
            texture = EnemyTextures.bulRep;

            bulRep = new bulletRepeller();

            base.Setup(inV2, topC, botC);

            MovementMode = mm;
            if (mm == 4)
            {
                counter = 39;
                mode = -1;
            }
            else if (mm == 5)
            {
                counter = 119;
                mode = -1;
            }
        }
        
        public override void update(Double gameTime)
        {
            switch (MovementMode)
            {
                case 3:
                    position -= new Vector2((0.0016f / 16) * (float)gameTime, 0);
                    break;
                case 4: 
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

                    break;
                case 5:
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
                    }

                    position -= (new Vector2(0.0014f, f) / 16) * (float)gameTime;
                    break;
                case 6:

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
                    break;
            }
            base.update(gameTime);
            bulRep.Update(position, gameTime);
        }

        public override void  draw(SpriteBatch spriteBatch, drawMode dm)
        {
 	         base.draw(spriteBatch, dm);
             bulRep.Draw(spriteBatch);
        }
    }

    class bulletRepeller
    {
        public static Texture2D shieldTex;

        private float drawRadius;
        private Vector2 position;
        private float rotate;
        private Color color;

        public bulletRepeller()
        {
            drawRadius = 0.5f;
            rotate = (float)Game1.gammgr.r.NextDouble() * 100f;
        }

        public void Update(Vector2 inPos, double gameTime)
        {
            position = inPos;
            rotate += (0.02f / 16) * (float)gameTime ;
            if (drawRadius < 0.8f)
                drawRadius += (((float)Game1.gammgr.numBuffs / 3000) / 16) * (float)gameTime ;
            color = new Color(((float)Math.Sin(rotate) + 1) / 3 + 0.6f,
                            ((float)Math.Sin(rotate) + 1) / 3 + 0.6f,
                            ((float)Math.Sin(rotate) + 1) / 3 + 0.6f, 0.75f);

            for (int i = 0; i < Game1.gammgr.playerBulletList.Count; i++)
            {
                if (Vector2.Distance(position, Game1.gammgr.playerBulletList[i].position) <= drawRadius / 5 &&
                    Game1.gammgr.r.Next(12) == 1)
                {
                    if (Game1.gammgr.playerBulletList[i].isSeeker)
                    {
                        Game1.gammgr.playerBulletList[i].CheckHit(position, 0.1f);
                    }
                    Game1.gammgr.playerBulletList[i].live = false;
                    Game1.gammgr.unusedBulletList.Add(Game1.gammgr.playerBulletList[i]);
                    Game1.gammgr.playerBulletList.RemoveAt(i);
                    drawRadius -= (1 - Game1.gammgr.saveGameData.playerShootspeed * Math.Min(4, Game1.gammgr.numBuffs)) / 20;                         
                    //0.012f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Game1.scrmgr.drawTexture(spriteBatch, shieldTex, position, color, drawRadius, rotate);
        }
    }
}
