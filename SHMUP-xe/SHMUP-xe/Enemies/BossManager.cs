using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace SHMUP.Enemies
{
    public class BossManager
    {
        [Serializable]
        public struct BossData
        {
            public List<int> blockType;
            public List<Vector2> blockOffset;
            public int coreBulletVolley;
            public int coreMissileVolley;
            public int difficulty;
        }

        public static BossData LoadBoss(String bossName)
        {
            BossData data;
            // Get the path of the saved game
            string fullpath;
            fullpath = Path.Combine(Path.Combine(Game1.content.RootDirectory, "Bosses"), bossName + ".bos");
            // Get the path of the saved game
            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            // Read the data from the file
            XmlSerializer serializer = new XmlSerializer(typeof(BossData));
            data = (BossData)serializer.Deserialize(stream);
            // Close the file
            stream.Close();

            return (data);
        }
    }
}
