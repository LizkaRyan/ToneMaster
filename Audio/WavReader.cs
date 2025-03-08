using System.Text;
using NAudio.Wave;

namespace ToneMaster.Audio;

public class WavReader
{
    private readonly String _input;

    private WavData _wavData;

    private WaveOutEvent _waveOut;

    private WaveFileReader _reader;

    private bool _isPlaying;

    public bool IsPlaying
    {
        get { return _isPlaying; }
    }

    public WavData WavData
    {
        get
        {
            if (_wavData == null)
            {
                _wavData = new WavData(this._input);
            }

            return _wavData;
        }
    }

    public WavReader(String input)
    {
        this._input = input;
        this._isPlaying = false;
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

    public void RemoveSimilarities(string input2Path, string outputPath, float threshold = 0.1f)
    {
        // Charger les deux fichiers audio
        WavData audio1 = new WavData(this._input);
        WavData audio2 = new WavData(input2Path);

        // Traiter les données audio
        byte[] processedData = ProcessAudioData(audio1, audio2, threshold);

        // Écrire le fichier de sortie
        WriteOutputFile(audio1, processedData, outputPath);
    }

    private static byte[] ProcessAudioData(WavData audio1, WavData audio2, float threshold)
    {
        short[] samples1 = audio1.GetSamples();
        short[] samples2 = audio2.GetSamples();
        int minLength = Math.Min(samples1.Length, samples2.Length);

        byte[] result = new byte[minLength * 2];

        for (int i = 0; i < minLength; i++)
        {
            float s1 = samples1[i] / 32768f;
            float s2 = samples2[i] / 32768f;
            float diff = Math.Abs(s1 - s2);

            // Suppression des similarités avec atténuation progressive
            short output = (diff < threshold) ? (short)0 : samples1[i];

            BitConverter.GetBytes(output).CopyTo(result, i * 2);
        }

        return result;
    }

    private static void WriteOutputFile(WavData original, byte[] newData, string outputPath)
    {
        using FileStream fs = File.Create(outputPath);

        // Écrire l'en-tête RIFF original
        fs.Write(original.RiffHeader, 0, original.RiffHeader.Length);

        // Réécrire le chunk fmt
        WriteChunk(fs, "fmt ", original.FmtChunk);

        // Écrire le nouveau chunk data
        WriteChunk(fs, "data", newData);

        // Réécrire les autres chunks
        foreach (byte[] chunkPart in original.OtherChunks)
            fs.Write(chunkPart, 0, chunkPart.Length);
    }

    private static void WriteChunk(FileStream fs, string chunkId, byte[] chunkData)
    {
        fs.Write(Encoding.ASCII.GetBytes(chunkId), 0, 4);
        fs.Write(BitConverter.GetBytes(chunkData.Length), 0, 4);
        fs.Write(chunkData, 0, chunkData.Length);
        if (chunkData.Length % 2 != 0) fs.WriteByte(0); // Padding
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
        if (!_isPlaying)
        {
            _reader = new WaveFileReader(this._input);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_reader);
            _waveOut.Play();
            _isPlaying = true;

            Console.WriteLine("Lecture du fichier audio...");
        }
    }

    public void Stop()
    {
        if (_isPlaying)
        {
            _waveOut.Stop();
            _isPlaying = false;
            _waveOut.Dispose();
            _reader.Close();
            _waveOut = null;
            _reader = null;
            Console.WriteLine("Lecture arrêtée.");
        }
    }

    public void Pause()
    {
        if (_isPlaying)
        {
            _waveOut.Pause();
            Console.WriteLine("Lecture en pause.");
        }
    }

    public void Resume()
    {
        if (_isPlaying)
        {
            _waveOut.Play();
            Console.WriteLine("Lecture reprise.");
        }
    }

    public void Skip(int seconds)
    {
        if (_reader != null && _waveOut != null)
        {
            long bytesPerSecond = _reader.WaveFormat.AverageBytesPerSecond;
            long skipBytes = bytesPerSecond * seconds;

            // Calculer la nouvelle position
            long newPosition = _reader.Position + skipBytes;

            // Assurez-vous de ne pas dépasser la longueur du fichier
            if (newPosition > _reader.Length)
                newPosition = _reader.Length;
            if (newPosition < 0)
                newPosition = 0;

            _reader.Position = newPosition;
        }
    }

    public void CloneAudio(string outputPath)
    {
        using (var inputStream = new FileStream(this._input, FileMode.Open, FileAccess.Read))
        using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        using (var reader = new BinaryReader(inputStream))
        using (var writer = new BinaryWriter(outputStream))
        {
            // Lire et écrire l'en-tête de 44 octets
            byte[] header = reader.ReadBytes(44); // L'en-tête est de 44 octets dans un fichier WAV standard
            writer.Write(header); // Écrire l'en-tête dans le fichier de sortie

            // Copier toutes les données audio du fichier source vers le fichier de sortie
            byte[] audioData = reader.ReadBytes((int)(inputStream.Length - 44)); // Lire les données audio
            writer.Write(audioData); // Écrire les données audio dans le fichier de sortie
        }

        Console.WriteLine($"Fichier cloné enregistré sous : {outputPath}");
    }

    public void DrawCursor(Graphics graphics, Panel panel)
    {
        if (_reader != null)
        {
            double percentage = (_reader.CurrentTime.TotalMilliseconds / _reader.TotalTime.TotalMilliseconds) * 100;
            double beginX = panel.Width * percentage / 100;
            double beginY = 0;
            double endX = panel.Width * percentage / 100;
            double endY = panel.Height;
            graphics.DrawLine(new Pen(Color.Red, 1), (int)beginX, (int)beginY, (int)endX, (int)endY);
        }
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