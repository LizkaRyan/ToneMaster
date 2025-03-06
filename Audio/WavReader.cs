using System.Media;
using System.Text;

namespace ToneMaster.Audio;

public class WavReader
{
    private String input;

    private WavData _wavData;
    
    public WavData WavData
    {
        get
        {
            if (_wavData == null)
            {
                _wavData = new WavData(this.input);
            }
            return _wavData;
        }
    }

    public WavReader(String input)
    {
        this.input = input;
    }

    public void ReduceNoise(string outputPath, float threshold)
    {
        // Vérifier le format audio
        short bitsPerSample = BitConverter.ToInt16(this.WavData.FmtChunk, 14);
        if (bitsPerSample != 16) throw new Exception("Uniquement 16-bit PCM supporté");

        // Réduction de bruit
        for (int i = 0; i < WavData.DataChunk.Length; i += 2)
        {
            short sample = BitConverter.ToInt16(WavData.DataChunk, i);

            // Appliquer un seuil : échantillons proches de zéro sont considérés comme du bruit
            if (Math.Abs(sample) < threshold)
            {
                sample = 0; // Suppression du bruit
            }

            // Écrire le nouvel échantillon
            BitConverter.GetBytes(sample).CopyTo(WavData.DataChunk, i);
        }

        // Réécriture du fichier
        RewriteData(outputPath);
    }

    public void AntiDistortion(string outputPath, float threshold = 0.95f)
    {
        // Vérification du format
        short bitsPerSample = BitConverter.ToInt16(WavData.FmtChunk, 14);
        if (bitsPerSample != 16) throw new Exception("Uniquement 16-bit PCM supporté");

        // Traitement anti-distortion
        for (int i = 0; i < WavData.DataChunk.Length; i += 2)
        {
            short original = BitConverter.ToInt16(WavData.DataChunk, i);
            float sample = original / 32768f; // Conversion en float [-1.0, 1.0]

            // Application du soft clipping
            float processed = ApplyAntiDistortion(sample, threshold);

            // Conversion finale
            short result = (short)(processed * 32768f);
            BitConverter.GetBytes(result).CopyTo(WavData.DataChunk, i);
        }

        // Écriture du fichier
        // Réécriture du fichier
        RewriteData(outputPath);
    }

    private static float ApplyAntiDistortion(float sample, float threshold)
    {
        // Seuil de déclenchement (par défaut 95% de l'amplitude max)
        float absSample = Math.Abs(sample);
        if (absSample <= threshold) return sample;

        // Courbure douce au-delà du seuil
        float excess = absSample - threshold;
        float softened = threshold + (excess / (1 + excess * 5f)); // Facteur d'adoucissement ajustable

        return Math.Sign(sample) * softened;
    }

    public void Amplify(string outputPath, float gain)
    {
        // Vérifier le format audio
        short bitsPerSample = BitConverter.ToInt16(WavData.FmtChunk, 14);
        if (bitsPerSample != 16) throw new Exception("Uniquement 16-bit PCM supporté");

        // Amplification
        for (int i = 0; i < WavData.DataChunk.Length; i += 2)
        {
            short sample = BitConverter.ToInt16(WavData.DataChunk, i);
            sample = (short)Math.Clamp(sample * gain, short.MinValue, short.MaxValue);
            BitConverter.GetBytes(sample).CopyTo(WavData.DataChunk, i);
        }

        // Réécriture du fichier
        RewriteData(outputPath);
    }

    private void RewriteData(string outputPath)
    {
        using (FileStream fs = File.Create(outputPath))
        {
            fs.Write(WavData.RiffHeader, 0, 12);
            fs.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
            fs.Write(BitConverter.GetBytes(WavData.FmtChunk.Length), 0, 4);
            fs.Write(WavData.FmtChunk, 0, WavData.FmtChunk.Length);
            if (WavData.FmtChunk.Length % 2 != 0) fs.WriteByte(0);
            fs.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
            fs.Write(BitConverter.GetBytes(WavData.DataChunk.Length), 0, 4);
            fs.Write(WavData.DataChunk, 0, WavData.DataChunk.Length);
            if (WavData.DataChunk.Length % 2 != 0) fs.WriteByte(0);
            foreach (byte[] chunkPart in WavData.OtherChunks)
                fs.Write(chunkPart, 0, chunkPart.Length);
        }
    }

    public void Play()
    {
        try
        {
            SoundPlayer player = new SoundPlayer(this.input);
            player.Play(); // Joue le fichier en mode asynchrone (sans bloquer le programme)

            // Pour attendre la fin du son, utilisez PlaySync au lieu de Play
            // player.PlaySync();

            Console.WriteLine("Lecture du fichier audio...");
            Console.ReadLine(); // Pour garder l'application ouverte
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la lecture du fichier : {ex.Message}");
        }
    }

    public void CloneAudio(string outputPath)
    {
        using (var inputStream = new FileStream(this.input, FileMode.Open, FileAccess.Read))
        using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        using (var reader = new BinaryReader(inputStream))
        using (var writer = new BinaryWriter(outputStream))
        {
            // Lire et écrire l'en-tête de 44 octets
            byte[] header = reader.ReadBytes(44);  // L'en-tête est de 44 octets dans un fichier WAV standard
            writer.Write(header); // Écrire l'en-tête dans le fichier de sortie

            // Copier toutes les données audio du fichier source vers le fichier de sortie
            byte[] audioData = reader.ReadBytes((int)(inputStream.Length - 44)); // Lire les données audio
            writer.Write(audioData);  // Écrire les données audio dans le fichier de sortie
        }

        Console.WriteLine($"Fichier cloné enregistré sous : {outputPath}");
    }
    
    public void DrawWaveform(Graphics graphics, Panel panel)
    {
        // Extraire les échantillons audio
        short[] samples = this.WavData.GetSamples();

        // Normalisation pour adapter la courbe au Panel
        float halfHeight = panel.Height / 2.0f;
        float scaleY = halfHeight / short.MaxValue;
        float scaleX = (float)samples.Length / panel.Width;

        // Styliser la courbe
        Pen wavePen = new Pen(Color.Blue, 1);

        // Dessiner les échantillons
        for (int x = 0; x < panel.Width - 1; x++)
        {
            int sampleIndex1 = (int)(x * scaleX);
            int sampleIndex2 = (int)((x + 1) * scaleX);

            if (sampleIndex1 < samples.Length && sampleIndex2 < samples.Length)
            {
                int y1 = (int)(halfHeight - samples[sampleIndex1] * scaleY);
                int y2 = (int)(halfHeight - samples[sampleIndex2] * scaleY);
                graphics.DrawLine(wavePen, x, y1, x + 1, y2);
            }
        }
    }
}