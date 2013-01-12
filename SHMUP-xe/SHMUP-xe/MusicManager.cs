using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class MusicManager
    {
        string playingSong = "";

        decimal fVolume = 0.5m;

        public decimal volume
        {
            set
            {
                if (value > 1)
                {
                    fVolume = 1;
                    SettingsManager.settings.musVolume = 1;
                }
                else if (value < 0)
                {
                    fVolume = 0;
                    SettingsManager.settings.musVolume = 0;
                }
                else
                {
                    fVolume = value;
                    SettingsManager.settings.musVolume = (float)value;
                }
                
                SettingsManager.SaveSettings();
                MediaPlayer.Volume = SettingsManager.settings.musVolume;
            }
            get
            {
                return (decimal)fVolume;
            }
        }

        public MusicManager()
        {
             MediaPlayer.Play(Game1.content.Load<Song>("Audio/Music/silence"));
            fVolume = (decimal)SettingsManager.settings.musVolume;
            MediaPlayer.Volume = (float)fVolume;
            MediaPlayer.IsRepeating = true;
        }

        public void PlayNewSong(int i)
        {
            if (playingSong != i.ToString())
            {
                MediaPlayer.Play(Game1.content.Load<Song>("Audio/Music/" + i.ToString()));
                playingSong = i.ToString();
                GC.Collect(); ;
            }
        }

        public void PlayNewSong(string s)
        {
            if (playingSong != s)
            {
                MediaPlayer.Play(Game1.content.Load<Song>("Audio/Music/" + s));
                playingSong = s;
                GC.Collect();
            }
        }

        public void stopSong()
        {
            MediaPlayer.Stop();
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }

   
}
