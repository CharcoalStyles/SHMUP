namespace LevelDesigner
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comBossType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSpwnLo = new System.Windows.Forms.TextBox();
            this.txtSpwnHi = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlBotColor = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlTopColor = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtGroupNumber = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIncPosY = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtIncPosX = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSpwnRound = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtInitPosY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInitPosX = new System.Windows.Forms.TextBox();
            this.comEnemyType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.butAddEnemy = new System.Windows.Forms.Button();
            this.comEnemyNumber = new System.Windows.Forms.ComboBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(312, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(106, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comBossType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSpwnLo);
            this.groupBox1.Controls.Add(this.txtSpwnHi);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pnlBotColor);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.pnlTopColor);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 128);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Wide Properties";
            // 
            // comBossType
            // 
            this.comBossType.FormattingEnabled = true;
            this.comBossType.Location = new System.Drawing.Point(156, 44);
            this.comBossType.Name = "comBossType";
            this.comBossType.Size = new System.Drawing.Size(121, 21);
            this.comBossType.TabIndex = 10;
            this.comBossType.SelectedIndexChanged += new System.EventHandler(this.comBossType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Boss Type";
            // 
            // txtSpwnLo
            // 
            this.txtSpwnLo.Location = new System.Drawing.Point(78, 89);
            this.txtSpwnLo.Name = "txtSpwnLo";
            this.txtSpwnLo.Size = new System.Drawing.Size(61, 20);
            this.txtSpwnLo.TabIndex = 8;
            this.txtSpwnLo.Text = "2000";
            this.txtSpwnLo.TextChanged += new System.EventHandler(this.txtSpwnLo_TextChanged);
            // 
            // txtSpwnHi
            // 
            this.txtSpwnHi.Location = new System.Drawing.Point(78, 45);
            this.txtSpwnHi.Name = "txtSpwnHi";
            this.txtSpwnHi.Size = new System.Drawing.Size(61, 20);
            this.txtSpwnHi.TabIndex = 7;
            this.txtSpwnHi.Text = "3000";
            this.txtSpwnHi.TextChanged += new System.EventHandler(this.txtSpwnHi_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Spawn Low";
            // 
            // pnlBotColor
            // 
            this.pnlBotColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBotColor.Location = new System.Drawing.Point(9, 89);
            this.pnlBotColor.Name = "pnlBotColor";
            this.pnlBotColor.Size = new System.Drawing.Size(50, 20);
            this.pnlBotColor.TabIndex = 3;
            this.pnlBotColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlBotColor_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bot Color";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Spawn High";
            // 
            // pnlTopColor
            // 
            this.pnlTopColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopColor.Location = new System.Drawing.Point(9, 44);
            this.pnlTopColor.Name = "pnlTopColor";
            this.pnlTopColor.Size = new System.Drawing.Size(50, 21);
            this.pnlTopColor.TabIndex = 1;
            this.pnlTopColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlTopColor_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Top Color";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtGroupNumber);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txtIncPosY);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtIncPosX);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtSpwnRound);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtInitPosY);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtInitPosX);
            this.groupBox2.Controls.Add(this.comEnemyType);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(12, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 155);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Group";
            // 
            // txtGroupNumber
            // 
            this.txtGroupNumber.Location = new System.Drawing.Point(216, 44);
            this.txtGroupNumber.Name = "txtGroupNumber";
            this.txtGroupNumber.Size = new System.Drawing.Size(61, 20);
            this.txtGroupNumber.TabIndex = 24;
            this.txtGroupNumber.Text = "0";
            this.txtGroupNumber.TextChanged += new System.EventHandler(this.txtGroupNumber_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(213, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 13);
            this.label14.TabIndex = 23;
            this.label14.Text = "No. enemies";
            // 
            // txtIncPosY
            // 
            this.txtIncPosY.Location = new System.Drawing.Point(128, 126);
            this.txtIncPosY.Name = "txtIncPosY";
            this.txtIncPosY.Size = new System.Drawing.Size(61, 20);
            this.txtIncPosY.TabIndex = 22;
            this.txtIncPosY.Text = "0";
            this.txtIncPosY.TextChanged += new System.EventHandler(this.txtIncPosY_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(105, 129);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Y:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 107);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Inc. Group Pos";
            // 
            // txtIncPosX
            // 
            this.txtIncPosX.Location = new System.Drawing.Point(29, 126);
            this.txtIncPosX.Name = "txtIncPosX";
            this.txtIncPosX.Size = new System.Drawing.Size(61, 20);
            this.txtIncPosX.TabIndex = 19;
            this.txtIncPosX.Text = "0";
            this.txtIncPosX.TextChanged += new System.EventHandler(this.txtIncPosX_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 129);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "X:";
            // 
            // txtSpwnRound
            // 
            this.txtSpwnRound.Location = new System.Drawing.Point(139, 44);
            this.txtSpwnRound.Name = "txtSpwnRound";
            this.txtSpwnRound.Size = new System.Drawing.Size(61, 20);
            this.txtSpwnRound.TabIndex = 17;
            this.txtSpwnRound.Text = "0";
            this.txtSpwnRound.TextChanged += new System.EventHandler(this.txtSpwnRound_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(136, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Spawn Round";
            // 
            // txtInitPosY
            // 
            this.txtInitPosY.Location = new System.Drawing.Point(128, 86);
            this.txtInitPosY.Name = "txtInitPosY";
            this.txtInitPosY.Size = new System.Drawing.Size(61, 20);
            this.txtInitPosY.TabIndex = 15;
            this.txtInitPosY.Text = "0.5";
            this.txtInitPosY.TextChanged += new System.EventHandler(this.txtInitPosY_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(105, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Inital Group Pos";
            // 
            // txtInitPosX
            // 
            this.txtInitPosX.Location = new System.Drawing.Point(29, 86);
            this.txtInitPosX.Name = "txtInitPosX";
            this.txtInitPosX.Size = new System.Drawing.Size(61, 20);
            this.txtInitPosX.TabIndex = 13;
            this.txtInitPosX.Text = "1.1";
            this.txtInitPosX.TextChanged += new System.EventHandler(this.txtInitPosX_TextChanged);
            // 
            // comEnemyType
            // 
            this.comEnemyType.FormattingEnabled = true;
            this.comEnemyType.Location = new System.Drawing.Point(9, 43);
            this.comEnemyType.Name = "comEnemyType";
            this.comEnemyType.Size = new System.Drawing.Size(121, 21);
            this.comEnemyType.TabIndex = 12;
            this.comEnemyType.SelectedIndexChanged += new System.EventHandler(this.comEnemyType_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "X:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Enemy Type";
            // 
            // butAddEnemy
            // 
            this.butAddEnemy.Location = new System.Drawing.Point(109, 161);
            this.butAddEnemy.Name = "butAddEnemy";
            this.butAddEnemy.Size = new System.Drawing.Size(40, 20);
            this.butAddEnemy.TabIndex = 4;
            this.butAddEnemy.Text = "add";
            this.butAddEnemy.UseVisualStyleBackColor = true;
            this.butAddEnemy.Click += new System.EventHandler(this.butAddEnemy_Click);
            // 
            // comEnemyNumber
            // 
            this.comEnemyNumber.FormattingEnabled = true;
            this.comEnemyNumber.Location = new System.Drawing.Point(12, 161);
            this.comEnemyNumber.Name = "comEnemyNumber";
            this.comEnemyNumber.Size = new System.Drawing.Size(91, 21);
            this.comEnemyNumber.TabIndex = 6;
            this.comEnemyNumber.SelectedIndexChanged += new System.EventHandler(this.comEnemyNumber_SelectedIndexChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(166, 161);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(56, 20);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Text = "copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(228, 160);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(56, 20);
            this.btnPaste.TabIndex = 8;
            this.btnPaste.Text = "paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 351);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.comEnemyNumber);
            this.Controls.Add(this.butAddEnemy);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SHMUP Level Designer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlBotColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlTopColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSpwnLo;
        private System.Windows.Forms.TextBox txtSpwnHi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comBossType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comEnemyType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button butAddEnemy;
        private System.Windows.Forms.ComboBox comEnemyNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtInitPosX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtInitPosY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSpwnRound;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGroupNumber;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtIncPosY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtIncPosX;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnPaste;
    }
}

