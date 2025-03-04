using ToneMaster.Audio;

namespace ToneMaster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WavReader wavReader;

        Dictionary<string,string> properties = ReadProperties();

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
                }
            }
        }

        private void Amplify(object sender, EventArgs e)
        {
            string fileTemp = $"{properties["temp.directory"]}\\temp.wav";
            wavReader.Amplify(fileTemp,2);
            wavReader = new WavReader(fileTemp);
        }

        private static Dictionary<string, string> ReadProperties()
        {
            string filePath = "../../../config.properties"; // Chemin vers le fichier .properties
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                // Dictionnaire pour stocker les paires clé-valeur
                var properties = new Dictionary<string, string>();

                // Lire chaque ligne du fichier
                foreach (var line in File.ReadAllLines(filePath))
                {
                    // Ignorer les lignes vides ou les commentaires (commençant par # ou ;)
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith(";"))
                        continue;

                    // Diviser la ligne en clé et valeur
                    var keyValue = line.Split(new[] { '=' }, 2); // Diviser uniquement au premier '='
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        properties[key] = value;
                    }
                }

                // Afficher les propriétés chargées
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
    }
}
