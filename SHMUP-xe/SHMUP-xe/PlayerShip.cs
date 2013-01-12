using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class PlayerShip
    {
        public static Texture2D mainShip;

        public Vector2 position;
        float maxSpeed = 0.02f;

        float drawScale;
        float drawScaleAlt;

        public bool visible = false;

        public PlayerShip()
        {
            mainShip = Game1.content.Load<Texture2D>("Enemy/SoftStar4");

            reset();
        }

        public void reset()
        {
            position = new Vector2(0.1f, 0.5f);
        }


        float counter = 0;
        public void update(GameTime gameTime)
        {
            counter += 0.0075f;

            drawScale = 0.1f;
            drawScaleAlt = 0.1667f;

            if (Game1.inpmgr.playerOneKeyboard)
            {
                position = Game1.inpmgr.playerOneInput.leftStick;
            }
            else
            {
                position += Game1.inpmgr.playerOneInput.leftStick * maxSpeed * SettingsManager.settings.stickMod;
            }

            if (position.X <  0.1f)
            {
                position.X = 0.1f;
            }
            else if (position.X > 0.9f)
            {
                position.X = 0.9f;
            }

            if (position.Y < 0.1f)
            {
                position.Y = 0.1f;
            }
            else if (position.Y > 0.9f)
            {
                position.Y = 0.9f;
            }            
        }
        public void update(Vector2 inpos, Boolean mouse)
        {
            counter += 0.02f;

            if (mouse)
            {
                position = inpos;
            }
            else
            {
                position += inpos * maxSpeed;
            }

            if (position.X < 0.1f)
            {
                position.X = 0.1f;
            }
            else if (position.X > 0.9f)
            {
                position.X = 0.9f;
            }

            if (position.Y < 0.1f)
            {
                position.Y = 0.1f;
            }
            else if (position.Y > 0.9f)
            {
                position.Y = 0.9f;
            }  
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            if (visible)
            {
                Game1.scrmgr.drawTexture(spriteBatch, mainShip, position, Color.Black, drawScaleAlt, counter);
                Game1.scrmgr.drawTexture(spriteBatch, mainShip, position, Color.Black, drawScaleAlt, -counter);
                Game1.scrmgr.drawTexture(spriteBatch, mainShip, position, Color.White, drawScale, counter);
                Game1.scrmgr.drawTexture(spriteBatch, mainShip, position, Color.White, drawScale, -counter);
            }
        }
    }
}
