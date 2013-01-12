using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SHMUP
{
    public class AwardsManager
    {
        public static AwardsData currentPlayerAwards;
        public static List<awardsheet> awardSheets = new List<awardsheet>();
        public static List<Texture2D> icons = new List<Texture2D>();

        public enum awards
        {
            Killed1k,
            Killed5k,
            Killed10k,
            Killed100k,
            Killed500k,
            Total10mPoints,
            Total50mPoints,
            Total100mPoints,
            MaxMultiplier,
            NoShipDied,
            NoShipHit,
            ShieldNotHit,
            noMissiles,
            Total1kMissiles,
            Total10kMissiles,
            Total50kMissiles,
            Total100kMissiles,
            Total100Pickups,
            Total1kPickups,
            Total10kPickups,
            OneShipLeft,
            MaxWeapons,
            MaxEverything,
            ZZZLASTAWARDTHATISONLYUSEDFORLISTGENERAIONANDITERATION
        }

        public enum stats
        {
            highScore,
            totalScore,
            totalKilled,
            totalPickups,
            totalMissiles,
            totalBossesKilled,
            ZZZLASTSTATTHATISONLYUSEDFORLISTGENERAIONANDITERATION
        }

        [Serializable]
        public struct AwardsData
        {
            public List<bool> awards;
            public List<int> stats;
            public decimal highestMultiplier;
        }

        public static void SaveAwards(String userName)
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
                    userName + ".awd");
#elif XBOX
            fullpath = Path.Combine(HighScoreManager.container.Path,  userName + ".awd");
#endif
                // Open the file, creating it if necessary
                File.Delete(fullpath);
                FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
                try
                {
                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(AwardsData));
                    serializer.Serialize(stream, currentPlayerAwards);
                }
                finally
                {
                    // Close the file
                    stream.Close();
                }
            }
        }

        public static void LoadAwards(String userName)
        {
            currentPlayerAwards = MakeNewData();

            AwardsData data;
            // Get the path of the saved game
            string fullpath;
#if WINDOWS
            fullpath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"),
                "SHMUP"),
                "SaveGames"),
                userName + ".awd");
#elif XBOX
            fullpath = Path.Combine(HighScoreManager.container.Path,  userName + ".awd");
#endif
            if (!File.Exists(fullpath))
            {
                data = MakeNewData();
                MessageManager.newMessage(Strings.NewAwards);
            }
            else
            {
                // Get the path of the saved game
                // Open the file
                FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(AwardsData));
                data = (AwardsData)serializer.Deserialize(stream);
                // Close the file
                stream.Close();
            }

            GamePadState gps = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            KeyboardState kbs = Keyboard.GetState();

            if (gps.IsButtonDown(Buttons.LeftShoulder) &&
                gps.IsButtonDown(Buttons.RightShoulder) &&
                gps.Triggers.Left > 0.5f &&
                gps.Triggers.Right > 0.5f)
            {
                data = MakeNewData();
            }
            else if (kbs.IsKeyDown(Keys.F1) &&
                    kbs.IsKeyDown(Keys.F2))
            {
                data = MakeNewData();
            }
            currentPlayerAwards = data;

            lastScore = data.stats[(int)stats.totalScore];

            SaveAwards(userName);
        }

        public static AwardsData MakeNewData()
        {
            AwardsData data = new AwardsData();

            data.awards = new List<bool>();
            int numFalseAwards = 0;
            for (int i = 0; i < (int)awards.ZZZLASTAWARDTHATISONLYUSEDFORLISTGENERAIONANDITERATION; i++)
            {
                data.awards.Add(false);
                numFalseAwards++;
            }
            data.stats = new List<int>();
            for (int i = 0; i < (int)stats.ZZZLASTSTATTHATISONLYUSEDFORLISTGENERAIONANDITERATION; i++)
            {
                data.stats.Add(0);
            }
            data.highestMultiplier = 0;

            return data;
        }

        public static void LoadAwardIcons()
        {
            for (int i = 0; i < (int)awards.ZZZLASTAWARDTHATISONLYUSEDFORLISTGENERAIONANDITERATION; i++)
            {
                switch ((AwardsManager.awards)(i))
                {
                    case AwardsManager.awards.Killed1k:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Lieutenant"));
                        break;
                    case AwardsManager.awards.Killed5k:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Commander"));
                        break;
                    case AwardsManager.awards.Killed10k:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Captain"));
                        break;
                    case AwardsManager.awards.Killed100k:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Commodore"));
                        break;
                    case AwardsManager.awards.Killed500k:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Admiral"));
                        break;
                    case AwardsManager.awards.Total10mPoints:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Points10k"));
                        break;
                    case AwardsManager.awards.Total50mPoints:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Points50k"));
                        break;
                    case AwardsManager.awards.Total100mPoints:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Points100k"));
                        break;
                    case AwardsManager.awards.MaxMultiplier:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Defender"));
                        break;
                    case AwardsManager.awards.NoShipDied:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Ninja"));
                        break;
                    case AwardsManager.awards.NoShipHit:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/SuperNinja"));
                        break;
                    case AwardsManager.awards.ShieldNotHit:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/VigilantDefender"));
                        break;
                    case AwardsManager.awards.noMissiles:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Ballistaphobic"));
                        break;
                    case AwardsManager.awards.Total1kMissiles:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Missile1k"));
                        break;
                    case AwardsManager.awards.Total10kMissiles:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Missile10k"));
                        break;
                    case AwardsManager.awards.Total50kMissiles:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Missile50k"));
                        break;
                    case AwardsManager.awards.Total100kMissiles:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/MissileMaster"));
                        break;
                    case AwardsManager.awards.Total100Pickups:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Pickup100"));
                        break;
                    case AwardsManager.awards.Total1kPickups:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Pickup1k"));
                        break;
                    case AwardsManager.awards.Total10kPickups:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Pickup10k"));
                        break;
                    case AwardsManager.awards.OneShipLeft:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/Snowball"));
                        break;
                    case AwardsManager.awards.MaxWeapons:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/FullyLoaded"));
                        break;
                    case AwardsManager.awards.MaxEverything:
                        icons.Add(Game1.content.Load<Texture2D>("AwdIcons/BigSpender"));
                        break;
                }
            }
        }
        ////////////////////////////////////////////////////////
        //Awards Updates

        public static bool checkEnemyKilledAwards = false;
        public static bool checkPlayerScoreAwards = false;
        static List<String> enemiesToProcess = new List<string>();
        static List<int> scoreToProcess = new List<int>();
        static int lastScore = 0;

        public static void scoreGained(int score)
        {
            scoreToProcess.Add(score);
            checkPlayerScoreAwards = true;
        }

        public static void enemyKilled(Enemies.Enemy enemy)
        {
            enemiesToProcess.Add(enemy.GetType().Name);
            //Get thread 1 to check the awards
            checkEnemyKilledAwards = true;
        }

        public static void CheckScoreAwards()
        {
            for (int i = 0; i < scoreToProcess.Count; i++)
            {
                currentPlayerAwards.stats[(int)stats.totalScore] += scoreToProcess[i];

                //checks
                //10k
                if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 1000000, 0.1f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total10mPoints, Strings.TenPercent, "1,000,000", "10,000,000", (int)awards.Total10mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 10000000, 0.25f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total10mPoints, Strings.TwentyFivePercent, "2,500,000", "10,000,000", (int)awards.Total10mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 10000000, 0.5f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total10mPoints, Strings.FiftyPercent, "5,000,000", "10,000,000", (int)awards.Total10mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 10000000, 0.75f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total10mPoints, Strings.TwentyFivePercent, "7,500,000", "10,000,000", (int)awards.Total10mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 10000000, 1) && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1779);
                    awardSheets.Add(new awardsheet(Strings.Total10mPoints, Strings.AwardUnlocked, "10,000,000", "10,000,000", (int)awards.Total10mPoints));
                    currentPlayerAwards.awards[(int)awards.Total10mPoints] = true;
                }
                //50k
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 5000000, 0.1f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total50mPoints, Strings.TenPercent, "5,000,000", "50,000,000", (int)awards.Total50mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 50000000, 0.25f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total50mPoints, Strings.TwentyFivePercent, "12,500,000", "50,000,000", (int)awards.Total50mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 50000000, 0.5f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total50mPoints, Strings.FiftyPercent, "25,000,000", "50,000,000", (int)awards.Total50mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 50000000, 0.75f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total50mPoints, Strings.TwentyFivePercent, "37,500,000", "50,000,000", (int)awards.Total50mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 50000000, 1) && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1780);
                    awardSheets.Add(new awardsheet(Strings.Total50mPoints, Strings.AwardUnlocked, "50,000,000", "50,000,000", (int)awards.Total50mPoints));
                    currentPlayerAwards.awards[(int)awards.Total50mPoints] = true;
                }
                //100k
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 10000000, 0.1f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total100mPoints, Strings.TenPercent, "10,000,000", "100,000,000", (int)awards.Total100mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 100000000, 0.25f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total100mPoints, Strings.TwentyFivePercent, "25,000,000", "100,000,000", (int)awards.Total100mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 100000000, 0.5f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total100mPoints, Strings.FiftyPercent, "50,000,000", "100,000,000", (int)awards.Total100mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 100000000, 0.75f))
                {
                    awardSheets.Add(new awardsheet(Strings.Total100mPoints, Strings.SeventyFivePercent, "75,000,000", "100,000,000", (int)awards.Total100mPoints));
                }
                else if (checkScore(lastScore, currentPlayerAwards.stats[(int)stats.totalScore], 100000000, 1) && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1781);
                    awardSheets.Add(new awardsheet(Strings.Total100mPoints, Strings.AwardUnlocked, "100,000,000", "100,000,000", (int)awards.Total100mPoints));
                    currentPlayerAwards.awards[(int)awards.Total100mPoints] = true;
                }

                lastScore = currentPlayerAwards.stats[(int)stats.totalScore];
            }

            scoreToProcess.Clear();
            checkPlayerScoreAwards = false;
        }

        static bool checkScore(int lowScore, int highScore, int awardScore, float percentage)
        {
            bool ret = false;
            int checkscore = (int)(awardScore * percentage);

            if (lowScore < checkscore && highScore > checkscore)
                ret = true;
            return ret;
        }

        public static void CheckEnemyKilledAwards()
        {
            for (int i = 0; i < enemiesToProcess.Count; i++)
            {
                //add one enemy killed to the total enemies killed
                currentPlayerAwards.stats[(int)stats.totalKilled]++;

                //check to see if any (partial) awards have been triggered for total number of Enemies Killed
                #region huge list of awardtriggers
                switch (currentPlayerAwards.stats[(int)stats.totalKilled])
                {
                    case 100:
                        awardSheets.Add(new awardsheet(Strings.Killed1k, Strings.TenPercent, "100", "1000", (int)awards.Killed1k));
                        break;
                    case 250:
                        awardSheets.Add(new awardsheet(Strings.Killed1k, Strings.TwentyFivePercent, "250", "1000", (int)awards.Killed1k));
                        break;
                    case 500:
                        awardSheets.Add(new awardsheet(Strings.Killed1k, Strings.FiftyPercent, "500", "1000", (int)awards.Killed1k));
                        awardSheets.Add(new awardsheet(Strings.Killed5k, Strings.TenPercent, "500", "5000", (int)awards.Killed5k));
                        break;
                    case 750:
                        awardSheets.Add(new awardsheet(Strings.Killed1k, Strings.SeventyFivePercent, "750", "1000", (int)awards.Killed1k));
                        break;
                    case 1000:
                       // Game1.icc.UnlockAchievement(1770);
                        awardSheets.Add(new awardsheet(Strings.Killed1k, Strings.AwardUnlocked, "1000", "1000", (int)awards.Killed1k));
                        currentPlayerAwards.awards[(int)awards.Killed1k] = true;
                        awardSheets.Add(new awardsheet(Strings.Killed10k, Strings.TenPercent, "1000", "10000", (int)awards.Killed10k));
                        break;
                    case 1250:
                        awardSheets.Add(new awardsheet(Strings.Killed5k, Strings.TwentyFivePercent, "1250", "5000", (int)awards.Killed5k));
                        break;
                    case 2500:
                        awardSheets.Add(new awardsheet(Strings.Killed5k, Strings.FiftyPercent, "2500", "5000", (int)awards.Killed5k));
                        awardSheets.Add(new awardsheet(Strings.Killed10k, Strings.TwentyFivePercent, "2500", "10000", (int)awards.Killed10k));
                        break;
                    case 3750:
                        awardSheets.Add(new awardsheet(Strings.Killed5k, Strings.SeventyFivePercent, "3750", "5000", (int)awards.Killed5k));
                        break;
                    case 5000:
                       // Game1.icc.UnlockAchievement(1771);
                        awardSheets.Add(new awardsheet(Strings.Killed5k, Strings.AwardUnlocked, "5000", "5000", (int)awards.Killed5k));
                        currentPlayerAwards.awards[(int)awards.Killed5k] = true;
                        awardSheets.Add(new awardsheet(Strings.Killed10k, Strings.FiftyPercent, "5000", "10000", (int)awards.Killed10k));
                        break;
                    case 7500:
                        awardSheets.Add(new awardsheet(Strings.Killed10k, Strings.SeventyFivePercent, "7500", "10000", (int)awards.Killed10k));
                        break;
                    case 10000:
                       // Game1.icc.UnlockAchievement(1772);
                        awardSheets.Add(new awardsheet(Strings.Killed10k, Strings.AwardUnlocked, "10000", "10000", (int)awards.Killed10k));
                        currentPlayerAwards.awards[(int)awards.Killed10k] = true;
                        awardSheets.Add(new awardsheet(Strings.Killed100k, Strings.TenPercent, "10000", "100000", (int)awards.Killed100k));
                        break;
                    case 25000:
                        awardSheets.Add(new awardsheet(Strings.Killed100k, Strings.TwentyFivePercent, "25000", "100000", (int)awards.Killed100k));
                        break;
                    case 50000:
                        awardSheets.Add(new awardsheet(Strings.Killed100k, Strings.FiftyPercent, "50000", "100000", (int)awards.Killed100k));
                        awardSheets.Add(new awardsheet(Strings.Killed500k, Strings.TenPercent, "50000", "500000", (int)awards.Killed500k));
                        break;
                    case 75000:
                        awardSheets.Add(new awardsheet(Strings.Killed100k, Strings.SeventyFivePercent, "75000", "100000", (int)awards.Killed100k));
                        break;
                    case 100000:
                        //Game1.icc.UnlockAchievement(1773);
                        awardSheets.Add(new awardsheet(Strings.Killed100k, Strings.AwardUnlocked, "100000", "100000", (int)awards.Killed100k));
                        currentPlayerAwards.awards[(int)awards.Killed100k] = true;
                        break;
                    case 125000:
                        awardSheets.Add(new awardsheet(Strings.Killed500k, Strings.TwentyFivePercent, "125000", "500000", (int)awards.Killed500k));
                        break;
                    case 250000:
                        awardSheets.Add(new awardsheet(Strings.Killed500k, Strings.FiftyPercent, "250000", "500000", (int)awards.Killed500k));
                        break;
                    case 375000:
                        awardSheets.Add(new awardsheet(Strings.Killed500k, Strings.SeventyFivePercent, "375000", "500000", (int)awards.Killed500k));
                        break;
                    case 500000:
                        //Game1.icc.UnlockAchievement(1774);
                        awardSheets.Add(new awardsheet(Strings.Killed500k, Strings.AwardUnlocked, "500000", "500000", (int)awards.Killed500k));
                        currentPlayerAwards.awards[(int)awards.Killed500k] = true;
                        break;
                }
                #endregion
                //get the type of enemy killed
                switch (enemiesToProcess[i])
                {
                    case "Solid3":
                        break;
                    case "Solid4":
                        break;
                    case "Solid5":
                        break;
                }
            }

            //Check the multiplier
            if (Game1.gammgr.multiplier > currentPlayerAwards.highestMultiplier)
            {
                if (currentPlayerAwards.highestMultiplier < 5m && Game1.gammgr.multiplier == 5m)
                {
                    awardSheets.Add(new awardsheet(Strings.AwdDefender, Strings.FiftyPercent, "5.0x", "10.0x", (int)awards.MaxMultiplier));
                }
                else if (currentPlayerAwards.highestMultiplier < 10m && Game1.gammgr.multiplier == 10m)
                {
                    //Game1.icc.UnlockAchievement(1758);
                    awardSheets.Add(new awardsheet(Strings.AwdDefender, Strings.AwardUnlocked, "10.0x", "10.0x", (int)awards.MaxMultiplier));
                    currentPlayerAwards.awards[(int)awards.MaxMultiplier] = true;
                }
                currentPlayerAwards.highestMultiplier = Game1.gammgr.multiplier;

            }

            //clear the list
            enemiesToProcess.Clear();
            //tell thread one not to check again.
            checkEnemyKilledAwards = false;

            SaveAwards(Game1.gammgr.currentPlayer);
        }

        public static void FiredNewMisile()
        {
            currentPlayerAwards.stats[(int)stats.totalMissiles]++;
            #region huge list of awardtriggers
            if (currentPlayerAwards.stats[(int)stats.totalMissiles] % 250 == 0)
            {
                switch (currentPlayerAwards.stats[(int)stats.totalMissiles])
                {
                    case 250:
                        awardSheets.Add(new awardsheet(Strings.Shot1kMissiles, Strings.TwentyFivePercent, "250", "1000", (int)awards.Total1kMissiles));
                        break;
                    case 500:
                        awardSheets.Add(new awardsheet(Strings.Shot1kMissiles, Strings.FiftyPercent, "500", "1000", (int)awards.Total1kMissiles));
                        break;
                    case 750:
                        awardSheets.Add(new awardsheet(Strings.Shot1kMissiles, Strings.SeventyFivePercent, "750", "1000", (int)awards.Total1kMissiles));
                        break;
                    case 1000:
                        if (!Game1.IS_DEMO)
                        {
                           //Game1.icc.UnlockAchievement(1775);
                            awardSheets.Add(new awardsheet(Strings.Shot1kMissiles, Strings.AwardUnlocked, "1000", "1000", (int)awards.Total1kMissiles));
                            currentPlayerAwards.awards[(int)awards.Total1kMissiles] = true;
                        }
                        break;
                    case 2500:
                        awardSheets.Add(new awardsheet(Strings.Shot10kMissiles, Strings.TwentyFivePercent, "2500", "10000", (int)awards.Total10kMissiles));
                        break;
                    case 5000:
                        awardSheets.Add(new awardsheet(Strings.Shot10kMissiles, Strings.FiftyPercent, "5000", "10000", (int)awards.Total10kMissiles));
                        break;
                    case 7500:
                        awardSheets.Add(new awardsheet(Strings.Shot10kMissiles, Strings.SeventyFivePercent, "7500", "10000", (int)awards.Total10kMissiles));
                        break;
                    case 10000:
                        if (!Game1.IS_DEMO)
                        {
                            //Game1.icc.UnlockAchievement(1776);
                            awardSheets.Add(new awardsheet(Strings.Shot10kMissiles, Strings.AwardUnlocked, "10000", "10000", (int)awards.Total10kMissiles));
                            currentPlayerAwards.awards[(int)awards.Total10kMissiles] = true;
                        }
                        break;
                    case 12500:
                        awardSheets.Add(new awardsheet(Strings.Shot50kMissiles, Strings.TwentyFivePercent, "12500", "50000", (int)awards.Total50kMissiles));
                        break;
                    case 25000:
                        awardSheets.Add(new awardsheet(Strings.Shot50kMissiles, Strings.FiftyPercent, "25000", "50000", (int)awards.Total50kMissiles));
                        break;
                    case 37500:
                        awardSheets.Add(new awardsheet(Strings.Shot50kMissiles, Strings.SeventyFivePercent, "37500", "50000", (int)awards.Total50kMissiles));
                        break;
                    case 50000:
                        if (!Game1.IS_DEMO)
                        {
                            //Game1.icc.UnlockAchievement(1777);
                            awardSheets.Add(new awardsheet(Strings.Shot50kMissiles, Strings.AwardUnlocked, "50000", "50000", (int)awards.Total50kMissiles));
                            currentPlayerAwards.awards[(int)awards.Total50kMissiles] = true;
                            awardSheets.Add(new awardsheet(Strings.Shot100kMissiles, Strings.FiftyPercent, "5000", "10000", (int)awards.Total100kMissiles));
                        }
                        break;
                    case 75000:
                        awardSheets.Add(new awardsheet(Strings.Shot100kMissiles, Strings.SeventyFivePercent, "75000", "100000", (int)awards.Total100kMissiles));
                        break;
                    case 100000:
                        if (!Game1.IS_DEMO)
                        {
                            //Game1.icc.UnlockAchievement(1778);
                            awardSheets.Add(new awardsheet(Strings.Shot100kMissiles, Strings.AwardUnlocked, "100000", "100000", (int)awards.Total100kMissiles));
                            currentPlayerAwards.awards[(int)awards.Total100kMissiles] = true;
                        }
                        break;
                }
            }
            #endregion
        }

        public static void GotNewPickup()
        {
            currentPlayerAwards.stats[(int)stats.totalPickups]++;
            #region huge list of awardtriggers
            if (currentPlayerAwards.stats[(int)stats.totalPickups] % 25 == 0)
            {
                switch (currentPlayerAwards.stats[(int)stats.totalPickups])
                {
                    case 25:
                        awardSheets.Add(new awardsheet(Strings.Got100Pickups, Strings.TwentyFivePercent, "25", "100", (int)awards.Total100Pickups));
                        break;
                    case 50:
                        awardSheets.Add(new awardsheet(Strings.Got100Pickups, Strings.FiftyPercent, "50", "100", (int)awards.Total100Pickups));
                        break;
                    case 75:
                        awardSheets.Add(new awardsheet(Strings.Got100Pickups, Strings.SeventyFivePercent, "75", "100", (int)awards.Total100Pickups));
                        break;
                    case 100:
                        if (!Game1.IS_DEMO)
                        {
                            //Game1.icc.UnlockAchievement(1767);
                            awardSheets.Add(new awardsheet(Strings.Got100Pickups, Strings.AwardUnlocked, "100", "100", (int)awards.Total100Pickups));
                            currentPlayerAwards.awards[(int)awards.Total100Pickups] = true;
                        }
                        break;
                    case 250:
                        awardSheets.Add(new awardsheet(Strings.Got1kPickups, Strings.TwentyFivePercent, "250", "1000", (int)awards.Total1kPickups));
                        break;
                    case 500:
                        awardSheets.Add(new awardsheet(Strings.Got1kPickups, Strings.FiftyPercent, "500", "1000", (int)awards.Total1kPickups));
                        break;
                    case 750:
                        awardSheets.Add(new awardsheet(Strings.Got1kPickups, Strings.SeventyFivePercent, "750", "1000", (int)awards.Total1kPickups));
                        break;
                    case 1000:
                        if (!Game1.IS_DEMO)
                        {
                            //Game1.icc.UnlockAchievement(1768);
                            awardSheets.Add(new awardsheet(Strings.Got1kPickups, Strings.AwardUnlocked, "1000", "1000", (int)awards.Total1kPickups));
                            currentPlayerAwards.awards[(int)awards.Total1kPickups] = true;
                        }
                        break;
                    case 2500:
                        awardSheets.Add(new awardsheet(Strings.Got10kPickups, Strings.TwentyFivePercent, "2500", "10000", (int)awards.Total10kPickups));
                        break;
                    case 5000:
                        awardSheets.Add(new awardsheet(Strings.Got10kPickups, Strings.FiftyPercent, "5000", "10000", (int)awards.Total10kPickups));
                        break;
                    case 7500:
                        awardSheets.Add(new awardsheet(Strings.Got10kPickups, Strings.SeventyFivePercent, "7500", "10000", (int)awards.Total10kPickups));
                        break;
                    case 10000:
                        if (!Game1.IS_DEMO)
                        {
                            //Game1.icc.UnlockAchievement(1769);
                            awardSheets.Add(new awardsheet(Strings.Got10kPickups, Strings.AwardUnlocked, "10000", "10000", (int)awards.Total10kPickups));
                            currentPlayerAwards.awards[(int)awards.Total10kPickups] = true;
                        }
                        break;
                }
            }
            #endregion
        }

        //level bools
        public static bool shipHit;
        public static bool shipDead;
        public static bool wallHit;
        public static bool missileshot;

        public static void LevelOver()
        {
            if (!shipHit)
            {
                if (!currentPlayerAwards.awards[(int)awards.NoShipHit] && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1765);
                    awardSheets.Add(new awardsheet(Strings.AwdShipNotHit, Strings.AwardUnlocked, (int)awards.NoShipHit));
                    currentPlayerAwards.awards[(int)awards.NoShipHit] = true;
                }
            }

            if (!shipDead)
            {
                if (!currentPlayerAwards.awards[(int)awards.NoShipDied] && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1764);
                    awardSheets.Add(new awardsheet(Strings.AwdShipNotDied, Strings.AwardUnlocked, (int)awards.NoShipDied));
                    currentPlayerAwards.awards[(int)awards.NoShipDied] = true;
                }
            }

            if (!wallHit)
            {
                if (!currentPlayerAwards.awards[(int)awards.ShieldNotHit] && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1766);
                    awardSheets.Add(new awardsheet(Strings.AwdWallNotHIt, Strings.AwardUnlocked, (int)awards.ShieldNotHit));
                    currentPlayerAwards.awards[(int)awards.ShieldNotHit] = true;
                }
            }

            if (!missileshot)
            {
                if (!currentPlayerAwards.awards[(int)awards.noMissiles] && !Game1.IS_DEMO)
                {
                    //Game1.icc.UnlockAchievement(1762);
                    awardSheets.Add(new awardsheet(Strings.AwdNoMiss, Strings.AwardUnlocked, (int)awards.noMissiles));
                    currentPlayerAwards.awards[(int)awards.noMissiles] = true;
                }
            }

            if (!currentPlayerAwards.awards[(int)awards.OneShipLeft] && Game1.gammgr.playerBuffs.Count == 1 && !Game1.IS_DEMO)
            {
                //Game1.icc.UnlockAchievement(1763);
                awardSheets.Add(new awardsheet(Strings.AwdOneShip, Strings.AwardUnlocked, (int)awards.OneShipLeft));
                currentPlayerAwards.awards[(int)awards.OneShipLeft] = true;
            }

            SaveAwards(Game1.gammgr.currentPlayer);
        }

        public static void checkMaxWeaponsAward()
        {
            if (!currentPlayerAwards.awards[(int)awards.MaxWeapons] && !Game1.IS_DEMO)
            {
                //Game1.icc.UnlockAchievement(1761);
                awardSheets.Add(new awardsheet(Strings.AwdMaxWeapons, Strings.AwardUnlocked, (int)awards.MaxWeapons));
                currentPlayerAwards.awards[(int)awards.MaxWeapons] = true;
            }
        }
        public static void checkMaxEverythingAward()
        {
            if (!currentPlayerAwards.awards[(int)awards.MaxEverything] && !Game1.IS_DEMO)
            {
                //Game1.icc.UnlockAchievement(1759);
                awardSheets.Add(new awardsheet(Strings.AwdMaxEverything, Strings.AwardUnlocked, (int)awards.MaxEverything));
                currentPlayerAwards.awards[(int)awards.MaxEverything] = true;
            }
        }

        public static void Update()
        {

            for (int i = 0; i < awardSheets.Count; i++)
            {
                awardSheets[i].Update();
                if (awardSheets[i].isFinished)
                {
                    awardSheets.RemoveAt(i);
                }
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < awardSheets.Count; i++)
            {
                awardSheets[i].Draw(spriteBatch, 0.045f + (0.08f * (float)i));
            }
        }
    }

    public class awardsheet
    {
        public static SpriteFont font;
        public static Texture2D back;
        public Texture2D icon;
        public bool isFinished = false;
        string awardTypeText;
        string awardText;
        string part;
        string total;
        float xPosition;

        int mode = 0;
        int counter = 0;

        bool showPartialness;

        public awardsheet(string inAwardText, string inAwardTypetext, string inPart, string inTotal, int awardIcon)
        {
            awardText = inAwardText;
            awardTypeText = inAwardTypetext;
            part = inPart;
            total = inTotal;
            xPosition = -0.15f;
            showPartialness = true;

            icon = AwardsManager.icons[awardIcon];
        }

        public awardsheet(string inAwardText, string inAwardTypetext, int awardIcon)
        {
            awardText = inAwardText;
            awardTypeText = inAwardTypetext;
            xPosition = -0.15f;
            showPartialness = false;

            icon = AwardsManager.icons[awardIcon];
        }

        public void Update()
        {
            switch (mode)
            {
                case 0:
                    xPosition += 0.006f;
                    if (xPosition >= 0.12f)
                        mode++;
                    break;
                case 1:
                    counter++;
                    if (counter >= 600)
                        mode++;
                    break;
                case 2:
                    xPosition -= 0.008f;
                    if (xPosition <= -0.15f)
                        isFinished = true;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float yPos)
        {
            Game1.scrmgr.drawTexture(spriteBatch, back, new Vector2(xPosition, yPos),Color.White, 0.5f, 0);
            Game1.scrmgr.drawTexture(spriteBatch, icon, new Vector2(xPosition - 0.1f, yPos), Color.White, 0.7f, 0);
            Game1.scrmgr.drawString(spriteBatch, font, awardTypeText, new Vector2(xPosition - 0.07f, yPos - 0.0275f), 0.55f, justification.left);
            Game1.scrmgr.drawString(spriteBatch, font, awardText, new Vector2(xPosition - 0.07f, yPos), 0.8f, justification.left);
            if(showPartialness)
                Game1.scrmgr.drawString(spriteBatch, font, part + "/" + total, new Vector2(xPosition - 0.07f, yPos + 0.0275f), 0.55f, justification.left);
        }
    }
}
