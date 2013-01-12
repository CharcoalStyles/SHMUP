using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Enemies
{
    public class Enemy
    {
        public static List<Texture2D> shipTex = new List<Texture2D>(); //All the ship textures

        public Vector2 position; //actual resolution indipendent position, updated in child's update()
        public float shipRadius; //In resolution indipentant scale, for collision detection
        public bool isDead = false; //Is it dead? updated in this.takeDamage() only
        public Color enemyColor = Color.RosyBrown; //Core color, set each update.
        public Color topColor = Color.RosyBrown; //topColor, passed in form background.
        public Color botColor = Color.RosyBrown; //botColor, passed in form background.

        public int score = 0; //score for killing set in child's constructor
        public float damageTake = 1; //damage taken per shot set in child's constructor
        public float drawSize = 1; //Amount to scale sprite, set in child's constructor
        public EnemyTextures texture; //Texture for enemy, set in child's constructor

        public float health = 1; //enemy's health

        public float rotate; //the rotate
        public float rotateSpeed = 0.05f; //rotate speed, can be reset in child's constructor

        public Color shield = Color.White; //shield color, only changes in update()
        public float shieldColor = 1; //more hsild color stuff, only changes in update() and takeDamage()

        public bool isinTop; //is the enemy in the top half or bottom half?

        public bool isBoss = false; //is this a boss?
       
        public void Setup(Vector2 inV2, Color topC, Color botC)
        {
            position = inV2;

            shipRadius = shipTex[(int)texture].Width * drawSize / Game1.graphics.GraphicsDevice.DisplayMode.Width;

            if (inV2.Y < 0.5f)
                isinTop = true;
            else
                isinTop = false;

            //enemyColor = c;
            topColor = topC;
            botColor = botC;
        }

        int damCounter;

        public virtual void takeDamage(float mod)
        {
            if (position.X < 1)
            {
                //Game1.bloom.bloomAmount += 0.02f;

                health -= damageTake * mod;
                shieldColor = 0.25f;

                if (damCounter < 1)
                {
                    Game1.sndmgr.playSound(SFX.enemyIsHit);
                    Game1.pclmgr.createEffect(ParticleEffect.enemyDamage, position, enemyColor);
                }

                if (health < 0.25f)
                {
                    isDead = true;

                    Game1.pclmgr.createEffect(ParticleEffect.enemyExplosion, position, enemyColor);
                    Game1.sndmgr.playSound(SFX.enemyDead);
                }
            }
            damCounter += 2;
        }

        public virtual void update(Double gameTime)
        {
            if (position.Y < 0.1f)
                position.Y = 0.1f;
            else if (position.Y > 0.9f)
                position.Y = 0.9f;

                rotate += (rotateSpeed / 16) * (float)gameTime;

                damCounter--;
                if (damCounter < 0)
                {
                    damCounter = 0;
                }

                if (shieldColor < 1)
                {
                    shieldColor += 0.1f;
                    shield = new Color(shieldColor, shieldColor, shieldColor);
                }
                else
                {
                    shield = Color.White;
                }

                enemyColor = new Color(Vector3.Lerp(Vector3.One - topColor.ToVector3(), Vector3.One - botColor.ToVector3(), 1 - position.Y));

                if (isBoss)
                {
                    Game1.gammgr.bossPos = position;
                }
        }

        public virtual void draw(SpriteBatch spriteBatch, drawMode dm)
        {
            switch (dm)
            {
                case drawMode.Keyline:
                    Game1.scrmgr.drawTexture(spriteBatch, shipTex[(int)texture], position, Color.Black, drawSize - ((1 - health) * 0.16f), rotate);
                    if (isBoss)
                    {
                        Game1.scrmgr.drawTexture(spriteBatch, shipTex[(int)texture], position, Color.Black, drawSize - ((1 - health) * 0.16f), rotate * -1);
                    }
                    break;
                case drawMode.Bottom:
                    Game1.scrmgr.drawTexture(spriteBatch, shipTex[(int)texture], position, shield, drawSize - ((1 - health) * 0.16f) - 0.05f, rotate);
                    if (isBoss)
                    {
                        Game1.scrmgr.drawTexture(spriteBatch, shipTex[(int)texture], position, shield, drawSize - ((1 - health) * 0.16f) - 0.05f, rotate * -1);
                    }
                    break;
                case drawMode.Top:
                    Game1.scrmgr.drawTexture(spriteBatch, shipTex[(int)texture], position, enemyColor, drawSize - 0.175f, rotate);
                    if (isBoss)
                    {
                        Game1.scrmgr.drawTexture(spriteBatch, shipTex[(int)texture], position, enemyColor, drawSize - 0.175f, rotate * -1);
                    }
                    break;
            }
        }
    }

    public enum EnemyTextures
    {
        Solid3,     //0
        Solid4,     //1
        SoftStar3,  //2
        SoftStar4,  //3
        SoftStar5,  //4
        SoftStar6,  //5
        HardStar3,  //6
        HardStar4,  //7
        HardStar5,  //8
        HardStar6,  //9
        Solid5,     //10
        Solid6,     //11
        bulRep,     //12
        misRep      //13,14,15,16
    }
}
