using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace LevelDesigner
{
    public partial class Form1 : Form
    {

        SHMUP.Screens.Levels.LevelManager.LevelData ld;
        ColorDialog cd;
        SaveFileDialog sfd;
        OpenFileDialog ofd;

        bool copyEmpty = true;
        int TMPenemyType;
        string TMPnumberOfenemies;
        string TMPgroupSpawnRound;
        string TMPgroupInitalPositionX;
        string TMPgroupInitalPositionY;
        string TMPgroupIncrementalPositionX;
        string TMPgroupIncrementalPositionY;

        int currentGroupCounter;

        public Form1()
        {
            InitializeComponent();
            cd = new ColorDialog();
            ld = new SHMUP.Screens.Levels.LevelManager.LevelData();
            sfd = new SaveFileDialog();
            sfd.Filter = "SHMUP Level File (*.lvl)|*.lvl";
            ofd = new OpenFileDialog();
            ofd.Filter = "SHMUP Level File (*.lvl)|*.lvl";

            //for (int i = 0; i < (int)SHMUP.Screens.Levels.LevelManager.bosses.ZZZEndOfList; i++)
            //{
            //    comBossType.Items.Add((SHMUP.Screens.Levels.LevelManager.bosses)i);
            //}

            string appPath = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf(Path.DirectorySeparatorChar));

            String[] bosses = Directory.GetFiles(Path.Combine(Path.Combine(appPath, "Content"), "Bosses"), "*.bos");

            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i] = bosses[i].Remove(0, Path.Combine(Path.Combine(appPath, "Content"), "Bosses").Length + 1);
                bosses[i] = bosses[i].Remove(bosses[i].LastIndexOf(".bos"));
                comBossType.Items.Add(bosses[i]);
            }

            for (int i = 0; i < (int)SHMUP.Screens.Levels.LevelManager.enemies.ZZZEndOfList; i++)
            {
                comEnemyType.Items.Add((SHMUP.Screens.Levels.LevelManager.enemies)i);
            }

            ClearAll();
        }

        #region LevelWideProperties change
        private void pnlTopColor_MouseClick(object sender, MouseEventArgs e)
        {
            if (cd.ShowDialog() == DialogResult.OK)
            {
                pnlTopColor.BackColor = cd.Color;
            }

            ld.colorHigh = new Microsoft.Xna.Framework.Color(
                pnlTopColor.BackColor.R,
                pnlTopColor.BackColor.G,
                pnlTopColor.BackColor.B,
                32).ToVector4();
        }

        private void pnlBotColor_MouseClick(object sender, MouseEventArgs e)
        {
            if (cd.ShowDialog() == DialogResult.OK)
            {
                pnlBotColor.BackColor = cd.Color;
            }

            ld.colorLow = new Microsoft.Xna.Framework.Color(
                pnlBotColor.BackColor.R,
                pnlBotColor.BackColor.G,
                pnlBotColor.BackColor.B,
                32).ToVector4();
        }

        private void txtSpwnHi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ld.spawnHigh = int.Parse(txtSpwnHi.Text);
            }
            catch { }
        }

        private void txtSpwnLo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ld.spawnLow = int.Parse(txtSpwnLo.Text);
            }
            catch{ }
        }

        private void comBossType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ld.boss = comBossType.SelectedItem.ToString();
        }

        #endregion

        #region EnemyGroupProperties Change
        private void butAddEnemy_Click(object sender, EventArgs e)
        {
            addNewEnemy();
        }

        private void comEnemyNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentGroupCounter = comEnemyNumber.SelectedIndex;
            UpdateEnemyGroup();
        }

        private void comEnemyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ld.enemyType[currentGroupCounter] = comEnemyType.SelectedIndex;
        }

        private void txtSpwnRound_TextChanged(object sender, EventArgs e)
        {
            try
            {
            ld.groupSpawnRound[currentGroupCounter] = int.Parse(txtSpwnRound.Text);
            }
            catch
            {
            }
        }

        private void txtGroupNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ld.numberOfenemies[currentGroupCounter] = int.Parse(txtGroupNumber.Text);
            }
            catch
            {
            }
        }

        private void txtInitPosX_TextChanged(object sender, EventArgs e)
        {
            try
            {
            ld.groupInitalPosition[currentGroupCounter] = new Vector2(
                float.Parse(txtInitPosX.Text, CultureInfo.InvariantCulture),
                ld.groupInitalPosition[currentGroupCounter].Y);
            }
            catch
            {
            }
        }

        private void txtInitPosY_TextChanged(object sender, EventArgs e)
        {
            try
            {
            ld.groupInitalPosition[currentGroupCounter] = new Vector2(
                ld.groupInitalPosition[currentGroupCounter].X,
                float.Parse(txtInitPosY.Text, CultureInfo.InvariantCulture));
            }
            catch
            {
            }
        }

        private void txtIncPosX_TextChanged(object sender, EventArgs e)
        {
            try
            {
            ld.groupIncrementalPosition[currentGroupCounter] = new Vector2(
                float.Parse(txtIncPosX.Text, CultureInfo.InvariantCulture),
                ld.groupIncrementalPosition[currentGroupCounter].Y);
            }
            catch
            {
            }
        }

        private void txtIncPosY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ld.groupIncrementalPosition[currentGroupCounter] = new Vector2(
                    ld.groupIncrementalPosition[currentGroupCounter].X,
                    float.Parse(txtIncPosY.Text, CultureInfo.InvariantCulture));
            }
            catch
            {
            }
        }
        #endregion

        #region utility Functions
        void addNewEnemy()
        {
            ld.enemyType.Add(0);
            ld.numberOfenemies.Add(0);
            ld.groupSpawnRound.Add(0);
            ld.groupInitalPosition.Add(new Vector2(1.1f, 0.5f));
            ld.groupIncrementalPosition.Add(new Vector2(0,0));

            comEnemyNumber.Items.Add(comEnemyNumber.Items.Count);
            currentGroupCounter = comEnemyNumber.Items.Count - 1;
            UpdateEnemyGroup();
            comEnemyNumber.SelectedIndex = currentGroupCounter;
        }

        void UpdateEnemyGroup()
        {
            comEnemyType.SelectedIndex = ld.enemyType[currentGroupCounter];

            txtSpwnRound.Text = ld.groupSpawnRound[currentGroupCounter].ToString();
            txtGroupNumber.Text = ld.numberOfenemies[currentGroupCounter].ToString();

            txtInitPosX.Text = ld.groupInitalPosition[currentGroupCounter].X.ToString();
            txtInitPosY.Text = ld.groupInitalPosition[currentGroupCounter].Y.ToString();

            txtIncPosX.Text = ld.groupIncrementalPosition[currentGroupCounter].X.ToString();
            txtIncPosY.Text = ld.groupIncrementalPosition[currentGroupCounter].Y.ToString();
        }

        void UpdateAll()
        {
            pnlTopColor.BackColor =System.Drawing.Color.FromArgb(
                255,
                (int)(ld.colorHigh.X * 255),
                (int)(ld.colorHigh.Y * 255),
                (int)(ld.colorHigh.Z * 255));

            pnlBotColor.BackColor = System.Drawing.Color.FromArgb(
                255,
                (int)(ld.colorLow.X * 255),
                (int)(ld.colorLow.Y * 255),
                (int)(ld.colorLow.Z * 255));

            txtSpwnHi.Text = ld.spawnHigh.ToString();
            txtSpwnLo.Text = ld.spawnLow.ToString();

            for (int i = 0; i < comBossType.Items.Count; i++)
            {
                if (string.Equals(ld.boss, comBossType.Items[i].ToString()))
                {
                    comBossType.SelectedIndex =  i;
                }
            }

            comEnemyNumber.Items.Clear();
            for (int i = 0; i < ld.groupInitalPosition.Count; i++)
            {
                comEnemyNumber.Items.Add(i.ToString());
            }

            UpdateEnemyGroup();
        }

        void ClearAll()
        {
            currentGroupCounter = 0;
            comEnemyNumber.Items.Clear();
            ld = new SHMUP.Screens.Levels.LevelManager.LevelData();

            ld.spawnLow = 2000;
            ld.spawnHigh = 3000;
            ld.colorLow = new Vector4(0, 0, 0, 0);
            ld.colorHigh = new Vector4(0, 0, 0, 0);
            ld.boss = "";
            ld.enemyType = new List<int>();
            ld.numberOfenemies = new List<int>();
            ld.groupSpawnRound = new List<int>();
            ld.groupInitalPosition = new List<Vector2>();
            ld.groupIncrementalPosition = new List<Vector2>();

            addNewEnemy();

            UpdateAll();
        }
        #endregion

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Open the file, creating it if necessary
                FileStream stream = File.Open(sfd.FileName, FileMode.Create);
                try
                {
                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(SHMUP.Screens.Levels.LevelManager.LevelData));
                    serializer.Serialize(stream, ld);
                }
                finally
                {
                    // Close the file
                    stream.Close();
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                currentGroupCounter = 0;
                // Open the file
                FileStream stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(SHMUP.Screens.Levels.LevelManager.LevelData));
                ld = (SHMUP.Screens.Levels.LevelManager.LevelData)serializer.Deserialize(stream);
                // Close the file
                stream.Close();

                UpdateAll();

                string s = ofd.FileName;
                s = s.Remove(0, s.LastIndexOf("\\"));

                this.Text = "SHMUP LD - " + s;
            }
            else
            {
                MessageBox.Show("Load Failed");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            TMPenemyType = comEnemyType.SelectedIndex;
            TMPnumberOfenemies = txtGroupNumber.Text;
            TMPgroupSpawnRound = txtSpwnRound.Text;
            TMPgroupInitalPositionX = txtInitPosX.Text;
            TMPgroupInitalPositionY = txtInitPosY.Text;
            TMPgroupIncrementalPositionX = txtIncPosX.Text;
            TMPgroupIncrementalPositionY = txtIncPosY.Text;
            copyEmpty = false;
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (!copyEmpty)
            {
                comEnemyType.SelectedIndex = TMPenemyType;
                txtGroupNumber.Text = TMPnumberOfenemies;
                txtSpwnRound.Text = TMPgroupSpawnRound;
                txtInitPosX.Text = TMPgroupInitalPositionX;
                txtInitPosY.Text = TMPgroupInitalPositionY;
                txtIncPosX.Text = TMPgroupIncrementalPositionX;
                txtIncPosY.Text = TMPgroupIncrementalPositionY;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

    }
}