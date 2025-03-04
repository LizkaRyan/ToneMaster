using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToneMaster.Audio;

namespace ToneMaster.Window
{
    public partial class SaveWindow : Form
    {
        WavReader wavReader;

        string outputDirectory;

        public SaveWindow(WavReader wavReader)
        {
            this.wavReader = wavReader;
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            string destination = this.destination.Text;
            string fileName = fileOutputName.Text;

            string outputPath = $"{destination}\\{fileName}.wav";

            wavReader.CloneAudio(outputPath);

            this.Close();
        }

        private void destinationButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = "C:\\"; // R�pertoire initial par d�faut

                // Si un dossier est s�lectionn�, alors...
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    this.destination.Text = folderDialog.SelectedPath;

                }
            }
        }
    }
}
