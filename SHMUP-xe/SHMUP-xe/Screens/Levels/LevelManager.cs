using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace SHMUP.Screens.Levels
{
    public class LevelManager
    {
        [Serializable]
        public struct LevelData
        {
            public int spawnLow;
            public int spawnHigh;
            public Vector4 colorLow;
            public Vector4 colorHigh;
            public string boss;
            public List<int> enemyType;
            public List<int> numberOfenemies;
            public List<int> groupSpawnRound;
            public List<Vector2> groupInitalPosition;
            public List<Vector2> groupIncrementalPosition;
        }

        [Serializable]
        public struct LevelList
        {
            public List<string> levels;
        }

        public enum enemies
        {
            Solid3,     // tutoral
            Solid4,     // level 1
            SoftStar3,  // level 2
            SoftStar4,  // level 4
            SoftStar5,  // level 7
            SoftStar6,  // level 10
            HardStar3,  // level 3
            HardStar4,  // level 5
            HardStar5,  // level 8
            HardStar6,  // level 11
            Solid5,     // level 6  
            Solid6,     // level 9
            bulRep3,    // +
            bulRep4,    // |
            bulRep5,    // |level 6
            bulRep6,    // +
            ZZZEndOfList
        }

        public static LevelData LoadLevel(String levelName)
        {
            LevelData data;
            // Get the path of the saved game
            string fullpath;
            fullpath = Path.Combine(Path.Combine(Game1.content.RootDirectory, "Levels"), levelName + ".lvl");
            // Get the path of the saved game
            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            // Read the data from the file
            XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
            data = (LevelData)serializer.Deserialize(stream);
            // Close the file
            stream.Close();

            String[] bosses = Directory.GetFiles(Path.Combine(Game1.content.RootDirectory, "Bosses"), "*.bos");

            bool hasBoss = false;
            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i] = bosses[i].Remove(0, Path.Combine(Game1.content.RootDirectory, "Bosses").Length + 1);
                bosses[i] = bosses[i].Remove(bosses[i].LastIndexOf(".bos"), 4);
                if (bosses[i] == data.boss)
                    hasBoss = true;
            }

            if (!hasBoss)
            {
                data.boss = "BossOne";
            }

            return (data);
        }

        public static bool LoadLevelLists(String listName)
        {
            bool retbool = true;
            LevelList data;
            // Get the path of the saved game
            string fullpath;
            fullpath = Path.Combine(Path.Combine(Game1.content.RootDirectory, "Levels"), listName + ".seq");
            // Get the path of the saved game
            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            // Read the data from the file
            XmlSerializer serializer = new XmlSerializer(typeof(LevelList));
            data = (LevelList)serializer.Deserialize(stream);
            // Close the file
            stream.Close();

            Game1.gammgr.levelSequence.Clear();
            int fails = 0;
            for (int i = 0; i < data.levels.Count; i++)
            {
                if (File.Exists(Path.Combine(Path.Combine(Game1.content.RootDirectory, "Levels"), data.levels[i] + ".lvl")))
                {
                    Game1.gammgr.levelSequence.Add(data.levels[i]);
                }
                else
                    fails++;
            }
            if (fails > 0)
                MessageManager.newMessage(fails + " " + Strings.LevelsNoExist);
            if (fails == data.levels.Count - 1)
                retbool = false;

            return retbool;

        }

        public static void GanerateRandomLevelSet(int number)
        {
            Game1.gammgr.levelSequence.Clear();
            String[] levels = Directory.GetFiles(Path.Combine(Game1.content.RootDirectory, "Levels"), "*.lvl");

            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = levels[i].Remove(0, Path.Combine(Game1.content.RootDirectory, "Levels").Length + 1);
                levels[i] = levels[i].Remove(levels[i].LastIndexOf(".lvl"), 4);
            }

            for (int i = 0; i < number; i++)
            {
                Game1.gammgr.levelSequence.Add(levels[Game1.gammgr.r.Next(levels.Length)]);
            }                  
        }

        public static void GenerateOriginalLevelSequence()
        {
            Game1.gammgr.levelSequence.Clear();
            Game1.gammgr.levelSequence.Add("Level 01");
            Game1.gammgr.levelSequence.Add("Level 02");
            Game1.gammgr.levelSequence.Add("Level 03");
            Game1.gammgr.levelSequence.Add("Level 04");
            if (!Game1.IS_DEMO)
            {
                Game1.gammgr.levelSequence.Add("Level 05");
                Game1.gammgr.levelSequence.Add("Level 06");
                Game1.gammgr.levelSequence.Add("Level 07");
                Game1.gammgr.levelSequence.Add("Level 08");
                Game1.gammgr.levelSequence.Add("Level 09");
                Game1.gammgr.levelSequence.Add("Level 10");
                Game1.gammgr.levelSequence.Add("Level 11");
                Game1.gammgr.levelSequence.Add("Level 12");
                Game1.gammgr.levelSequence.Add("Level 13");
                Game1.gammgr.levelSequence.Add("Level 14");
                Game1.gammgr.levelSequence.Add("Level 15");
            }
            Game1.gammgr.currentLevelInSequence = 0;
        }
    }
}
