using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

using System.IO.Compression;
using Microsoft.Xna.Framework.Storage;

using System.Net;
using System.Text;


namespace SHMUP
{
    public class HighScoreManager
    {
        public static readonly String localHighScoreFileName = "all.hs";

        public static StorageContainer container;

        public static string finalResponse = "";

        [Serializable]
        public struct HighScoreData
        {
            public string[] PlayerName;
            public int[] Score;
            public int Count;
            public HighScoreData(int count)
            {
                PlayerName = new string[count];
                Score = new int[count];
                Count = count;
            }
        }

        public static void SaveHighScores(HighScoreData data)
        {
            // Get the path of the saved game
            string fullpath;
#if WINDOWS
            fullpath = Path.Combine(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"),
                "SHMUP"),
                localHighScoreFileName);
#elif XBOX
            fullpath = Path.Combine(container.Path, localHighScoreFileName);
#endif

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.Create);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

        }

        public static HighScoreData LoadHighScores()
        {
            HighScoreData data;
            // Get the path of the saved game
            string fullpath;
#if WINDOWS
            fullpath = Path.Combine(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games"),
                "SHMUP"),
                localHighScoreFileName);
#elif XBOX
            fullpath = Path.Combine(container.Path, localHighScoreFileName);
#endif
            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            catch
            {
                MessageManager.newMessage(Strings.InvalidHighScore);
                data = MakeNewData();
            }
            finally
            {
                // Close the file
                stream.Close();
            }
            return (data);
        }

        public static bool CheckHighScore(int inScore, string inName)
        {
            bool retBool = false;

            if (Game1.gammgr.loggedIn)
            {
                // Create the data to save
                HighScoreData data = LoadHighScores();
                int scoreIndex = -1;
                for (int i = 0; i < data.Count; i++)
                {
                    if (inScore > data.Score[i])
                    {
                        scoreIndex = i;
                        break;
                    }
                }
                if (scoreIndex > -1)
                {
                    retBool = true;
                    //New high score found ... do swaps
                    for (int i = data.Count - 1; i > scoreIndex; i--)
                    {
                        data.PlayerName[i] = data.PlayerName[i - 1];
                        data.Score[i] = data.Score[i - 1];
                    }
                    data.PlayerName[scoreIndex] = inName;
                    data.Score[scoreIndex] = inScore;
                    SaveHighScores(data);
                    finalResponse = sendScore(inName, inScore);
                }
            }
            return retBool;
        }

        private static string hashString(string _value)
        {
#if WINDOWS
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(_value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++) ret += data[i].ToString("x2").ToLower();
            return ret;
#elif XBOX
            return "X";
#endif
        }

        public static string sendScore(string name, int score)
        {
            string response = "";

           // Game1.icc.PostLeaderboardScore(439, score);
#if WINDOWS
            /*PostSubmitter post = new PostSubmitter();
            post.Url = "http://charcoalstyles.com/postScore.asp";
            if (!Game1.IS_DEMO)
                post.PostItems.Add("Name", name);
            else
                post.PostItems.Add("Name", name + "(Demo)");
            post.PostItems.Add("Score", score.ToString());
            post.PostItems.Add("Hash", hash);
            post.Type = PostSubmitter.PostTypeEnum.Post;
            try
            {
                string result = post.Post();
                return response.Trim();
            }
            catch
            {
                return "fail";
            }*/

#endif
            //return "fail";

            return response;
        }

        public static HighScoreData MakeNewData()
        {
            HighScoreManager.HighScoreData data = new HighScoreManager.HighScoreData(5);

            data.PlayerName[0] = "A";
            data.Score[0] = 10000;
            data.PlayerName[1] = "B";
            data.Score[1] = 5000;
            data.PlayerName[2] = "C";
            data.Score[2] = 2000;
            data.PlayerName[3] = "D";
            data.Score[3] = 1000;
            data.PlayerName[4] = "E";
            data.Score[4] = 500;

            return data;
        }
        
        private static ICELandaLib.CoLeaderboardPage page;
        private static HighScoreManager.HighScoreData data;

        public static HighScoreData GetGlobalHighScores()
        {
            data = new HighScoreManager.HighScoreData(5);

#if WINDOWS
            //page = Game1.icc.RequestOpenLeaderboard(437, 5, hsDelegate);

            /*PostSubmitter post = new PostSubmitter();
            post.Url = "http://charcoalstyles.com/SHMUPgetHS.asp";
            post.Type = PostSubmitter.PostTypeEnum.Get;
            try
            {
                string result = post.Post();

                string[] arr = new string[10];

                arr = result.Split(",".ToCharArray());

                if (arr.Length > 5)
                {
                    data.PlayerName[0] = arr[0];
                    data.Score[0] = int.Parse(arr[1]);
                    data.PlayerName[1] = arr[2];
                    data.Score[1] = int.Parse(arr[3]);
                    data.PlayerName[2] = arr[4];
                    data.Score[2] = int.Parse(arr[5]);
                    data.PlayerName[3] = arr[6];
                    data.Score[3] = int.Parse(arr[7]);
                    data.PlayerName[4] = arr[8];
                    data.Score[4] = int.Parse(arr[9]);
                }
            }
            catch
            {
            }*/

#endif
            return data;

        }

        private static void hsDelegate(bool success)
        {
            if (success)
            {
                //Game1.icc.RequestFirstPage(page, null);

                if (null != page)
                {
                    /*ICELandaLib.CoLeaderboardRow row;
                    for (int i = 0; i < page.rows.Length; i++)
                    {
                        row = page.GetRow((uint)(i + 1));
                        data.PlayerName[i] = row.UserName;
                        data.Score[i] = (int)row.Score;
                    }*/
                }
            }
        }
    }
}
