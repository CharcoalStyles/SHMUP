using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace SHMUP
{
    public class SettingsManager
    {

        [Serializable]
        public struct Settings
        {
            public String lastPlayer;
            public int resolution;
            public bool fullScreen;
            public float sfxVolume;
            public float musVolume;
            public double particleMod;
            public float stickMod;
            public int postProcessQuality;
        }

        public static Settings settings;

        public static readonly String localOptionsFileName = "Options.xml";

        public static void LoadSettings()
        {
            // Get the path of the saved game
            string fullpath;

#if WINDOWS
            fullpath = Path.Combine(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"),
                "SHMUP"),
                localOptionsFileName);
#elif XBOX
            fullpath = Path.Combine(HighScoreManager.container.Path, localOptionsFileName);
#endif

            if (File.Exists(fullpath))
            {
                // Open the file
                FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                settings = (Settings)serializer.Deserialize(stream);
                // Close the file
                stream.Close();
            }
            else
            {
                makeNewSettings();
                SaveSettings();
            }
        }

        public static void SaveSettings()
        {
            string fullpath;
#if WINDOWS
            fullpath = Path.Combine(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"),
                "SHMUP"),
                localOptionsFileName);
#elif XBOX
            fullpath = Path.Combine(HighScoreManager.container.Path, localOptionsFileName);
#endif
            FileStream stream = null;
            try
            {
                // Open the file, creating it if necessary
                File.Delete(fullpath);
                stream = File.Open(fullpath, FileMode.OpenOrCreate);
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(stream, settings);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        public static void makeNewSettings()
        {
            settings.lastPlayer = "";
            settings.musVolume = 0.5f;
            settings.resolution = 1;
            settings.fullScreen = false;
            settings.sfxVolume = 0.5f;
            settings.particleMod = 0.8f;
            settings.stickMod = 0.5f;
            settings.postProcessQuality = 2;
        }
    }
}
