using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class PickupEffectManager
    {
        static bool slowMoActive = false;
        static int slowMoTimer = 0;
        static int slowMoLength = 600; //600 frame, 10 seconds
        static double slowMoAmount = 0;

        public static void activateSlowmo()
        {
            slowMoActive = true;
            slowMoTimer = 0;
            Game1.sndmgr.playSound(SFX.SlowMoStart);
        }

        public static void killSlowmo()
        {
            slowMoTimer = 600;
        }

        public static void Update()
        {
            #region Slowmo
            if (slowMoActive && !Game1.gammgr.isPaused)
            {
                if (slowMoAmount >= 2.5)
                {
                    slowMoTimer++;
                    if (slowMoTimer >= slowMoLength)
                    {
                        slowMoActive = false;
                        Game1.sndmgr.playSound(SFX.SlowMoEnd);
                    }
                }
                else
                {
                    slowMoAmount += 0.02;
                    float f = (float)(slowMoAmount - 1) / 1.5f;
                    //Game1.bloom.fadeEffect(BloomSettings.PresetSettings[1], BloomSettings.PresetSettings[2], f);
                }
            }
            else
            {
                if (slowMoAmount > 1)
                {
                    slowMoAmount -= 0.02;
                    float f = (float)(slowMoAmount - 1) / 1.5f;
                    //Game1.bloom.fadeEffect(BloomSettings.PresetSettings[1], BloomSettings.PresetSettings[2], f);
                }
                else
                {
                    slowMoAmount = 1;
                }
            }

            Game1.gammgr.divisor = slowMoAmount;
            #endregion
        }
    }
}
