namespace ToneMaster.Window
{
    partial class SaveWindow
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
            fileOutputName = new TextBox();
            fileOutputLabel = new Label();
            destination = new TextBox();
            buttonSave = new Button();
            destinationButton = new Button();
            SuspendLayout();
            // 
            // fileOutputName
            // 
            fileOutputName.Location = new Point(151, 51);
            fileOutputName.Name = "fileOutputName";
            fileOutputName.Size = new Size(343, 27);
            fileOutputName.TabIndex = 0;
            fileOutputName.Text = "File_modified";
            // 
            // fileOutputLabel
            // 
            fileOutputLabel.AutoSize = true;
            fileOutputLabel.Location = new Point(66, 54);
            fileOutputLabel.Name = "fileOutputLabel";
            fileOutputLabel.Size = new Size(80, 20);
            fileOutputLabel.TabIndex = 1;
            fileOutputLabel.Text = "File name :";
            // 
            // destination
            // 
            destination.Location = new Point(66, 107);
            destination.Name = "destination";
            destination.Size = new Size(321, 27);
            destination.TabIndex = 3;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(233, 174);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(94, 29);
            buttonSave.TabIndex = 4;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // destinationButton
            // 
            destinationButton.Location = new Point(393, 106);
            destinationButton.Name = "destinationButton";
            destinationButton.Size = new Size(101, 29);
            destinationButton.TabIndex = 5;
            destinationButton.Text = "Destination";
            destinationButton.UseVisualStyleBackColor = true;
            destinationButton.Click += destinationButton_Click;
            // 
            // SaveWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(565, 238);
            Controls.Add(destinationButton);
            Controls.Add(buttonSave);
            Controls.Add(destination);
            Controls.Add(fileOutputLabel);
            Controls.Add(fileOutputName);
            Name = "SaveWindow";
            Text = "SaveWindow";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox fileOutputName;
        private Label fileOutputLabel;
        private TextBox destination;
        private Button buttonSave;
        private Button destinationButton;
    }
}