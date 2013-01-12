using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace SHMUP
{
    public class SaveGameManager
    {
        [Serializable]
        public struct SaveGameData
        {
            public bool skipTutorial;
            public string twitterUserName;
            public string twitterPassword;
            public bool verifiedTwitter;
            public int totalScore; //the total score a player has gained, minus how much they're spent at the store.
            public float playerShootspeed; //shoot speed from 0.05 to 0.5 in 0.05 increments;
            public int totalPlayerBuffs; //number of buffs the player has at their command
            public int buffHealth; //health of buffs
            public int buffSpeed; //Max speed of hte buffs
            public int levelHeathOrbs;
            public int maxMissiles;
            public double missileExpolsion;
            public double basePickUpRate;
            public int key1; //
            public int key2; //
            public int key3; //
        }
        static Random r;
        public static void SaveGame(String userName, SaveGameData data)
        {
            if (Game1.gammgr.loggedIn)
            {
                // Get the path of the saved game
                string fullpath;
#if WINDOWS
                fullpath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "My Games"),
                    "SHMUP"),
                    "SaveGames"),
                    userName + ".sav");
#elif XBOX
            fullpath = Path.Combine(HighScoreManager.container.Path,  userName + ".sav");
#endif
                //Generate Keys

                r = new Random((data.totalPlayerBuffs + data.buffHealth + data.buffSpeed + data.levelHeathOrbs) + data.totalScore);
                data.key1 = r.Next();
                r = new Random(((int)(data.playerShootspeed * 150) + (int)data.missileExpolsion + data.maxMissiles + (int)data.basePickUpRate) + data.totalScore);
                data.key2 = r.Next();
                r = new Random(data.key1 * data.key2);
                data.key3 = r.Next();

                // Open the file, creating it if necessary
                File.Delete(fullpath);
                FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
                try
                {
                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                    serializer.Serialize(stream, data);
                }
                finally
                {
                    // Close the file
                    stream.Close();
                }
            }
            else
            {
                MessageManager.newMessage(Strings.NotLoggedInSaveFail);
            }
        }

        public static SaveGameData LoadGame(String userName)
        {
            SaveGameData data;
            // Get the path of the saved game
            string fullpath;
#if WINDOWS
            fullpath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"),
                "SHMUP"),
                "SaveGames"),
                userName + ".sav");
#elif XBOX
            fullpath = Path.Combine(HighScoreManager.container.Path,  userName + ".sav");
#endif
            if (!File.Exists(fullpath))
            {
                MessageManager.newMessage(Strings.NewSave);
                data = MakeNewData();
            }
            else
            {
                // Get the path of the saved game
                // Open the file
                FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                data = (SaveGameData)serializer.Deserialize(stream);
                // Close the file
                stream.Close();

                if (!isValidSaveGameFile(data))
                {
                    MessageManager.newMessage(Strings.InvalidSaveGame);
                    data = MakeNewData();
                }
                
                GamePadState gps = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
                KeyboardState kbs = Keyboard.GetState();

                if (gps.IsButtonDown(Buttons.LeftShoulder) &&
                    gps.IsButtonDown(Buttons.RightShoulder) &&
                    gps.Triggers.Left > 0.5f &&
                    gps.Triggers.Right > 0.5f)
                {
                    MessageManager.newMessage("Deleted " + userName + "'s Save Game");
                    data = MakeNewData();
                    SettingsManager.makeNewSettings();
                    SettingsManager.SaveSettings();
                }
                else if (kbs.IsKeyDown(Keys.F1) &&
                        kbs.IsKeyDown(Keys.F2))
                {
                    MessageManager.newMessage("Deleted " + userName + "'s Save Game");
                    data = MakeNewData();
                }

            }

            AwardsManager.LoadAwards(userName);

            return (data);
        }

        public static SaveGameData MakeNewData()
        {
            SaveGameData data = new SaveGameData();

            data = new SaveGameData();
            data.totalScore = 0;
            data.playerShootspeed = 0.05f;
            data.totalPlayerBuffs = 3;
            data.buffHealth = 1;
            data.buffSpeed = 1;
            data.levelHeathOrbs = 10;
            data.maxMissiles = 30;
            data.missileExpolsion = 0.25;
            data.basePickUpRate = 0.00;
            data.twitterUserName = "";
            data.twitterPassword = "";
            data.verifiedTwitter = false;
            data.skipTutorial = false;
            r = new Random((data.totalPlayerBuffs + data.buffHealth + data.buffSpeed + data.levelHeathOrbs) + data.totalScore);
            data.key1 = r.Next();
            r = new Random(((int)(data.playerShootspeed * 150) + (int)data.missileExpolsion + data.maxMissiles + (int)data.basePickUpRate) + data.totalScore);
            data.key2 = r.Next();
            r = new Random(data.key1 * data.key2);
            data.key3 = r.Next();

            return data;
        }

        static bool isValidSaveGameFile(SaveGameData data)
        {
            bool returnBool = true;

            //r = new Random((data.totalPlayerBuffs + data.buffHealth + data.buffSpeed + data.levelHeathOrbs) + data.totalScore);
            //if (data.key1 != r.Next())
            //    returnBool = false;
            //r = new Random(((int)(data.playerShootspeed * 150) + (int)data.missileExpolsion + data.maxMissiles + (int)data.basePickUpRate) + data.totalScore);
            //if (data.key2 != r.Next())
            //    returnBool = false;
            //r = new Random(data.key1 * data.key2);
            //if (data.key3 != r.Next())
            //    returnBool = false;                

            return returnBool;
        }
    }
}
