using System.Diagnostics;
using ToneMaster.Audio;
using ToneMaster.Window;

namespace ToneMaster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WavReader wavReader;

        Dictionary<string, string> properties = ReadProperties();

        private void ouvrirUnFichierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Tous les fichiers (*.wav)|*.wav";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    wavReader = new WavReader(selectedFilePath);
                    this.WavePanel.Invalidate();
                }
            }
        }

        private void Amplify(object sender, EventArgs e)
        {
            string fileTemp = $"{properties["temp.directory"]}\\temp.wav";
            wavReader.Amplify(fileTemp, (float)this.inputNumber.Value);
            wavReader = new WavReader(fileTemp);
            this.WavePanel.Invalidate();
        }

        private static Dictionary<string, string> ReadProperties()
        {
            string filePath = "../../../config.properties"; // Chemin vers le fichier .properties
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                // Dictionnaire pour stocker les paires cl�-valeur
                var properties = new Dictionary<string, string>();

                // Lire chaque ligne du fichier
                foreach (var line in File.ReadAllLines(filePath))
                {
                    // Ignorer les lignes vides ou les commentaires (commen�ant par # ou ;)
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith(";"))
                        continue;

                    // Diviser la ligne en cl� et valeur
                    var keyValue = line.Split(new[] { '=' }, 2); // Diviser uniquement au premier '='
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        properties[key] = value;
                    }
                }

                // Afficher les propri�t�s charg�es
                foreach (var entry in properties)
                {
                    result.Add(entry.Key, entry.Value);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($@"Put the config.properties file at the root of the project: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($@"Erreur lors de la lecture du fichier : {ex.Message}");
            }

            return result;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (wavReader.IsPlaying)
            {
                wavReader.Resume();
            }
            wavReader.Play();
        }

        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveWindow saveWindow = new SaveWindow(wavReader);
            // Affiche la fenêtre
            saveWindow.ShowDialog(); // Utilisez ShowDialog() pour une fenêtre modale
        }

        private void AntiDistortion(object sender, EventArgs e)
        {
            string fileTemp = $"{properties["temp.directory"]}\\temp.wav";
            wavReader.AntiDistortion(fileTemp, (float)this.inputNumber.Value);
            wavReader = new WavReader(fileTemp);
            this.WavePanel.Invalidate();
        }

        private void ReduceNoise(object sender, EventArgs e)
        {
            string fileTemp = $"{properties["temp.directory"]}\\temp.wav";
            wavReader.ReduceNoise(fileTemp, 0.8f);
            wavReader = new WavReader(fileTemp);
            this.WavePanel.Invalidate();
        }

        private void WavePanel_Paint(object sender, PaintEventArgs e)
        {
            if (this.wavReader != null)
            {
                this.wavReader.DrawWaveform(e.Graphics, this.WavePanel);
                return;
            }
            this.DrawStraightLine(e.Graphics, this.WavePanel);
        }

        private void DrawStraightLine(Graphics g, Panel panel)
        {
            float beginY = panel.Height / 2;
            float beginX = 0;
            float endY = panel.Height / 2;
            float endX = panel.Width;

            g.DrawLine(new Pen(Color.Blue, 1), beginX, beginY, endX, endY);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            this.wavReader.Stop();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            this.wavReader.Pause();
        }

        private void next10Button_Click(object sender, EventArgs e)
        {
            this.wavReader.Skip(10);
        }

        private void prev10Button_Click(object sender, EventArgs e)
        {
            this.wavReader.Skip(-10);
        }
    }
}
