namespace ToneMaster
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            ToneMaster = new Label();
            MainPanel = new Panel();
            panel2 = new Panel();
            WavePanel = new Panel();
            inputNumber = new NumericUpDown();
            playButton = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            menuStrip1 = new MenuStrip();
            fichierToolStripMenuItem = new ToolStripMenuItem();
            ouvrirUnFichierToolStripMenuItem = new ToolStripMenuItem();
            enregistrerToolStripMenuItem = new ToolStripMenuItem();
            fileSelected = new OpenFileDialog();
            panel1.SuspendLayout();
            MainPanel.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)inputNumber).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Highlight;
            panel1.Controls.Add(ToneMaster);
            panel1.Location = new Point(0, 34);
            panel1.Name = "panel1";
            panel1.Size = new Size(981, 64);
            panel1.TabIndex = 0;
            // 
            // ToneMaster
            // 
            ToneMaster.AutoSize = true;
            ToneMaster.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ToneMaster.ForeColor = SystemColors.Control;
            ToneMaster.Location = new Point(12, 9);
            ToneMaster.Name = "ToneMaster";
            ToneMaster.Size = new Size(170, 38);
            ToneMaster.TabIndex = 0;
            ToneMaster.Text = "ToneMaster";
            // 
            // MainPanel
            // 
            MainPanel.Controls.Add(panel2);
            MainPanel.Location = new Point(1, 94);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(980, 353);
            MainPanel.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(WavePanel);
            panel2.Controls.Add(inputNumber);
            panel2.Controls.Add(playButton);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Location = new Point(11, 10);
            panel2.Name = "panel2";
            panel2.Size = new Size(957, 334);
            panel2.TabIndex = 0;
            // 
            // WavePanel
            // 
            WavePanel.BackColor = SystemColors.ActiveBorder;
            WavePanel.Location = new Point(13, 15);
            WavePanel.Name = "WavePanel";
            WavePanel.Size = new Size(927, 182);
            WavePanel.TabIndex = 5;
            WavePanel.Paint += WavePanel_Paint;
            // 
            // inputNumber
            // 
            inputNumber.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            inputNumber.Location = new Point(369, 218);
            inputNumber.Name = "inputNumber";
            inputNumber.Size = new Size(145, 27);
            inputNumber.TabIndex = 4;
            // 
            // playButton
            // 
            playButton.Location = new Point(390, 251);
            playButton.Name = "playButton";
            playButton.Size = new Size(94, 29);
            playButton.TabIndex = 3;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += playButton_Click;
            // 
            // button3
            // 
            button3.Location = new Point(732, 286);
            button3.Name = "button3";
            button3.Size = new Size(136, 29);
            button3.TabIndex = 2;
            button3.Text = "Reduce Noise";
            button3.UseVisualStyleBackColor = true;
            button3.Click += ReduceNoise;
            // 
            // button2
            // 
            button2.Location = new Point(378, 286);
            button2.Name = "button2";
            button2.Size = new Size(124, 29);
            button2.TabIndex = 1;
            button2.Text = "AntiDistortion";
            button2.UseVisualStyleBackColor = true;
            button2.Click += AntiDistortion;
            // 
            // button1
            // 
            button1.Location = new Point(28, 286);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "Amplify";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Amplify;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fichierToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(981, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            fichierToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ouvrirUnFichierToolStripMenuItem, enregistrerToolStripMenuItem });
            fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            fichierToolStripMenuItem.Size = new Size(66, 24);
            fichierToolStripMenuItem.Text = "Fichier";
            // 
            // ouvrirUnFichierToolStripMenuItem
            // 
            ouvrirUnFichierToolStripMenuItem.Name = "ouvrirUnFichierToolStripMenuItem";
            ouvrirUnFichierToolStripMenuItem.Size = new Size(197, 26);
            ouvrirUnFichierToolStripMenuItem.Text = "Ouvrir un fichier";
            ouvrirUnFichierToolStripMenuItem.Click += ouvrirUnFichierToolStripMenuItem_Click;
            // 
            // enregistrerToolStripMenuItem
            // 
            enregistrerToolStripMenuItem.Name = "enregistrerToolStripMenuItem";
            enregistrerToolStripMenuItem.Size = new Size(197, 26);
            enregistrerToolStripMenuItem.Text = "Enregistrer";
            enregistrerToolStripMenuItem.Click += enregistrerToolStripMenuItem_Click;
            // 
            // fileSelected
            // 
            fileSelected.FileName = "fileSelected";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(981, 450);
            Controls.Add(MainPanel);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            MainPanel.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)inputNumber).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label ToneMaster;
        private Panel MainPanel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fichierToolStripMenuItem;
        private ToolStripMenuItem ouvrirUnFichierToolStripMenuItem;
        private ToolStripMenuItem enregistrerToolStripMenuItem;
        private OpenFileDialog fileSelected;
        private Panel panel2;
        private Button button1;
        private Button button3;
        private Button button2;
        private Button playButton;
        private NumericUpDown inputNumber;
        private Panel WavePanel;
    }
}
