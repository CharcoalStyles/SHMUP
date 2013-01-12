using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class PlayerBuffs
    {
        public static Texture2D tex;

        public Vector2 position;
        float maxSpeed;

        Vector2 accel;
        float accelmod;
        Vector2 tV2;

        public float drawScale = 0.15f;

        float rotspeed;
        float rotation = 0;

        public float shipRadius;//In resolution indipentant scale, for collision detection

        public bool growing = true;

        public bool isDead = false;

        public float health = 1;

        public static Color topColor;
        public static Color botColor;

        Color painColor = Color.Transparent;

        public Color c;
        public Color shield = Color.White; //shield color, only changes in update()
        public float shieldColor = 1; //more hsild color stuff, only changes in update() and takeDamage()

        public float shootingTimer = 0;
        float rocketTimer = 0;

        int pauseTime = 0;

        int healTime = 0;
        int healSwit = 180;

        public PlayerBuffs()
        {
            shipRadius = tex.Width * drawScale / Game1.graphics.GraphicsDevice.DisplayMode.Width;

            maxSpeed = ((float)Game1.gammgr.r.NextDouble() * 0.00075f) + 0.0015f + ((float) Game1.gammgr.saveGameData.buffSpeed * 0.0001f);
            accelmod = 1 - (((float)Game1.gammgr.r.NextDouble() * 0.05f) + 0.05f);
            //accelmod = 0.95f;
            rotspeed = (float)Game1.gammgr.r.NextDouble() / 2 + 0.2f;

            shootingTimer = (float)Game1.gammgr.r.NextDouble() * 3;

            health = (float)Game1.gammgr.saveGameData.buffHealth;
        }

        public void resetPosition(Vector2 inPosition)
        {
            position = new Vector2((float)Game1.gammgr.r.NextDouble() / 5 - 0.5f, (float)Game1.gammgr.r.NextDouble() / 2 + 0.25f);
        }

        public void takeDamage(float mod)
        {
            health -= 0.075f * mod;
            shieldColor = 0.25f;
            painColor = new Color(0, 0, 0, 1 - shieldColor);
            Game1.pclmgr.createEffect(ParticleEffect.pbDamage, position, c);
            Game1.sndmgr.playSound(SFX.playerHit);
            Game1.inpmgr.playerOneInput.rightRumble += 0.05f;

            AwardsManager.shipHit = true;

            pauseTime = 15;
            healTime = 0;
        }

        public void update(Vector2 inPosition, List<Vector2> pbPos)
        {

            if (health< 0.01f)
            {
                isDead = true; 
                AwardsManager.shipDead = true;
            }

            if (healTime < healSwit)
            {
                healTime++;
            }
            else
            {
                if (health < Game1.gammgr.saveGameData.buffHealth)
                {
                    health += 0.1f;
                }
                else if (health > Game1.gammgr.saveGameData.buffHealth)
                {
                    health = (float)Game1.gammgr.saveGameData.buffHealth;
                }
            }

            c = new Color(Vector3.Lerp(topColor.ToVector3(), botColor.ToVector3(), position.Y));
            if (shieldColor < 1)
            {
                shieldColor += 0.1f;
                shield = new Color(shieldColor, shieldColor, shieldColor);
                painColor = new Color(0, 0, 0, 1 - shieldColor);
            }
            else
            {
                shield = Color.White;
                painColor = Color.Transparent;
            }

                //Movement Code!
                accel *= accelmod;
                tV2 = (inPosition - position);
                tV2.Normalize();
                tV2 *= 0.75f;
                accel += tV2 * maxSpeed;
                System.Console.Write("hi");

                for (int i = 0; i < pbPos.Count; i++)
                {
                    if (Vector2.Distance(pbPos[i], position) < 0.025f - ((float)Game1.gammgr.saveGameData.totalPlayerBuffs * 0.001f))
                    {
                        if (pbPos[i] != position)
                        {
                            accel -= (pbPos[i] - position) * (0.1f - Vector2.Distance(pbPos[i], position));
                        }
                    }
                }

                position += accel;
                //new new new Bullet Shotting

                //if (Game1.inpmgr.playerOneInput.A == expButtonState.Pressed || Game1.inpmgr.playerOneInput.A == expButtonState.Held ||
                //    Game1.inpmgr.playerOneInput.rightTrigger > 0.25f)
                //{
                    shootingTimer += Game1.gammgr.saveGameData.playerShootspeed;

                    if (shootingTimer >= 3)
                    {
                        shootingTimer = 0;
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
                        b.Setup(new Vector2(0.006f + (Game1.gammgr.saveGameData.playerShootspeed / 50), 0), position, true, false);

                        Game1.gammgr.playerBulletList.Add(b);
                        Game1.sndmgr.playSound(SFX.playerShot);
                    }
                //}

                rocketTimer += (float)(Game1.gammgr.r.NextDouble() * 0.05);

                if (Game1.inpmgr.playerOneInput.B == expButtonState.Pressed)
                    rocketTimer += 0.005f;

                if (rocketTimer >= 2)
                {
                    if (Game1.inpmgr.playerOneInput.A == expButtonState.Pressed ||
                        Game1.inpmgr.playerOneInput.A == expButtonState.Held ||
                        Game1.inpmgr.playerOneInput.B == expButtonState.Pressed ||
                        Game1.inpmgr.playerOneInput.B == expButtonState.Held ||
                        Game1.inpmgr.playerOneInput.leftTrigger > 0.25f ||
                        Game1.inpmgr.playerOneInput.rightTrigger > 0.25f)
                    {
                        rocketTimer = 0;
                        Game1.gammgr.mb.FireMissiles(position);
                        AwardsManager.missileshot = true;
                    }
                } 
            
            if (pauseTime != 0)
            {
                pauseTime--;
                shieldColor = 0.25f;
            }
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch, drawMode dm)
        {

            switch (dm)
            {
                case drawMode.Keyline:
                    Game1.scrmgr.drawTexture(spriteBatch, tex, position, Color.Black, drawScale + (health * 0.005f) + 0.02f, rotation);
                    break;
                case drawMode.Bottom:
                    Game1.scrmgr.drawTexture(spriteBatch, tex, position, shield, drawScale + (health * 0.005f), rotation);
                    break;
                case drawMode.Top:
                    Game1.scrmgr.drawTexture(spriteBatch, tex, position, c, drawScale - (health * 0.005f) - 0.005f, rotation);
                    break;
            }

            Game1.scrmgr.drawTexture(spriteBatch,Shape.CircleFade, position, painColor, drawScale - (health * 0.005f) - 0.005f, rotation);
        }
    }
}
