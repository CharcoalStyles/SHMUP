using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace SHMUP
{
    public enum SFX
    {
        menuMove,
        menuSelect,
        playerShot,
        enemyIsHit,
        enemyDead,
        rocket,
        playerHit,
        MissileHit,
        SlowMoEnd,
        SlowMoStart,
        ZZZDONOTPLAYLASTLINE
    }
    public class SoundManager
    {
        List<List<SoundEffectInstance>> soundEffects = new List<List<SoundEffectInstance>>();
        List<float> soundEffectVolume = new List<float>();
        List<float> soundEffectPan = new List<float>();

        int i;

        decimal fmasterFXVolume = 0.5m;

        public decimal masterFXVolume
        {
            set
            {
                if (value > 1)
                {
                    fmasterFXVolume = 1;
                    SettingsManager.settings.sfxVolume = 1;
                }
                else if (value < 0)
                {
                    fmasterFXVolume = value;
                    SettingsManager.settings.sfxVolume = 0;
                }
                else
                {
                    fmasterFXVolume = value;
                    SettingsManager.settings.sfxVolume = (float)value;
                }

                SettingsManager.SaveSettings();

            }
            get
            {
                return fmasterFXVolume;
            }
        }

        public SoundManager()
        {
            fmasterFXVolume = (decimal)SettingsManager.settings.sfxVolume;
        }

        public void load()
        {
            for (i = 0; i < (int)SFX.ZZZDONOTPLAYLASTLINE; i++)
            {
                bool b = false;
                List<SoundEffectInstance> l = new List<SoundEffectInstance>();
                int counter = 0;
                while (!b)
                {
                    counter++;
                    try
                    {
                        SoundEffect t = Game1.content.Load<SoundEffect>("Audio/Effects/" + ((SFX)i).ToString() + counter.ToString());
                        t.Play(0.001f, 0, 0);
                        SoundEffectInstance s = t.CreateInstance();
                        s.Volume = 0.001f;
                        s.Play();
                        l.Add(s);
                    }
                    catch
                    {
                        b = true;
                    }  
                }
                soundEffects.Add(l);
                soundEffectVolume.Add(0);
                soundEffectPan.Add(0);
            }
        }

        public void playSound(SFX inSFX)
        {
            switch (inSFX)
            {
                case SFX.menuMove:
                    soundEffectVolume[(int)inSFX] = 1f;
                    break;
                case SFX.menuSelect:
                    soundEffectVolume[(int)inSFX] = 1f;
                    break;
                case SFX.playerShot:
                    soundEffectVolume[(int)inSFX] = 0.35f;
                    break;
                case SFX.enemyIsHit:
                    if (soundEffectVolume[(int)inSFX] == 0)
                        soundEffectVolume[(int)inSFX] = 0.5f;
                    else
                        soundEffectVolume[(int)inSFX] += 0.05f;
                    break;
                case SFX.enemyDead:
                    soundEffectVolume[(int)inSFX] = 0.75f;
                    break;
                case SFX.rocket:
                    soundEffectVolume[(int)inSFX] = 0.15f;
                    break;
                case SFX.playerHit:
                    soundEffectVolume[(int)inSFX] = 0.25f;
                    break;
                case SFX.MissileHit:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
                case SFX.SlowMoEnd:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
                case SFX.SlowMoStart:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
            }
        }

        public void playSound(SFX inSFX, float inPan)
        {
            switch (inSFX)
            {
                case SFX.menuMove:
                    soundEffectVolume[(int)inSFX] = 1f;
                    break;
                case SFX.menuSelect:
                    soundEffectVolume[(int)inSFX] = 1f;
                    break;
                case SFX.playerShot:
                    soundEffectVolume[(int)inSFX] = 0.25f;
                    break;
                case SFX.enemyIsHit:
                    if (soundEffectVolume[(int)inSFX] == 0)
                        soundEffectVolume[(int)inSFX] = 0.4f;
                    else
                        soundEffectVolume[(int)inSFX] += 0.1f;
                    break;
                case SFX.enemyDead:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
                case SFX.rocket:
                    soundEffectVolume[(int)inSFX] = 0.15f;
                    break;
                case SFX.playerHit:
                    soundEffectVolume[(int)inSFX] = 0.25f;
                    break;
                case SFX.MissileHit:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
                case SFX.SlowMoEnd:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
                case SFX.SlowMoStart:
                    soundEffectVolume[(int)inSFX] = 0.4f;
                    break;
            }

            soundEffectPan[(int)inSFX] = inPan;
        }

        //public void LoadNPlay(String s)
        //{
        //    Game1.content.Load<SoundEffect>("Audio/Effects/" + s).Play();
        //}

        public void update()
        {
            for (i = 0; i < (int)SFX.ZZZDONOTPLAYLASTLINE; i++)
            {
                if (soundEffectVolume[i] > 0)
                {
                    try
                    {
                        soundEffects[i][Game1.gammgr.r.Next(soundEffects[i].Count)].Volume = soundEffectVolume[i] * (float)masterFXVolume;
                        soundEffects[i][Game1.gammgr.r.Next(soundEffects[i].Count)].Pan = soundEffectPan[i];
                        soundEffects[i][Game1.gammgr.r.Next(soundEffects[i].Count)].Play();
                    }
                    catch{}
                   soundEffectVolume[i] = 0;
                }
            }
        }
    }
}
