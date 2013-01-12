using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public enum PickupType
    {
        missileAdd,
        shieldAdd,
        slowMo
    }

    public class Pickup
    {
        public Vector2 position;
        PickupType pickupType;

        Color coreColor;

        Vector2 scaleOne;
        Vector2 scaleTwo;

        float collisionRadius;

        public Pickup(Vector2 inPos, PickupType p)
        {
            position = inPos;
            pickupType = p;

            collisionRadius = PlayerBuffs.tex.Width * 0.1f / Game1.graphics.GraphicsDevice.DisplayMode.Width;

            switch (pickupType)
            {
                case PickupType.missileAdd:
                    coreColor = Color.Green;
                    break;
                case PickupType.slowMo:
                    coreColor = Color.Red;
                    break;
                case PickupType.shieldAdd:
                    coreColor = Color.Blue;
                    break;
            }
        }

        public bool checkCollision(Vector2 inPosition, float inRadius)
        {
            bool retBool = false;
            
            if (Vector2.Distance(position, inPosition) <= (inRadius + collisionRadius) * 0.8f)
            {
                retBool = true;
                AwardsManager.GotNewPickup();
                
                List<Vector2> l = new List<Vector2>();
                switch (pickupType)
                {
                    case PickupType.missileAdd:
                        Game1.gammgr.mb.AddMissiles();
                        l.Add(new Vector2(0.5f,0.9f));
                        Game1.pclmgr.createEffect(ParticleEffect.pickUp, position, Color.Green);
                        break;
                    case PickupType.slowMo:
                        Game1.pclmgr.createEffect(ParticleEffect.pickUp, position, Color.Red);
                        PickupEffectManager.activateSlowmo();
                        break;
                    case PickupType.shieldAdd:
                        Game1.gammgr.lh.Heal();
                        Game1.pclmgr.createEffect(ParticleEffect.pickUp, position, Color.Blue);
                        break;
                }
            }

            return retBool;
        }

        public void Update(GameTime gameTime)
        {
            scaleOne = new Vector2((((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 150) + 2) / 3) * 0.05f,
                (((float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 150) + 2) / 3) * 0.05f);
            scaleTwo = new Vector2((((float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 150) + 2) / 3) * 0.05f,
                            (((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 150) + 2) / 3) * 0.05f);

            position -= new Vector2(0.001f, 0);
            
        }

        public void Draw(SpriteBatch spriteBatch, drawMode dm)
        {
            switch (dm)
            {
                case drawMode.Keyline:
                    Game1.scrmgr.drawTexture(spriteBatch, PlayerBuffs.tex, position, Color.White, scaleOne + new Vector2(0.075f,0.075f), 0);
                    break;
                case drawMode.Bottom:
                    Game1.scrmgr.drawTexture(spriteBatch, PlayerBuffs.tex, position, Color.Black, scaleTwo + new Vector2(0.06f,0.06f), 0);
                    break;
                case drawMode.Top:
                    Game1.scrmgr.drawTexture(spriteBatch, PlayerBuffs.tex, position, coreColor, scaleOne + new Vector2(0.035f,0.035f), 0);
                    break;
            }
        }
    }
}
