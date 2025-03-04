using System.Text;

namespace ToneMaster.Audio;

public class WavReader
{

    private String input;
    public int SampleRate { get; private set; }
    public int BitsPerSample { get; private set; }
    public int Channels { get; private set; }
    public byte[] AudioData { get; private set; }

    public WavReader(String input)
    {
        this.input = input;
    }
    
    public void Amplify(string outputPath, float gain)
    {
        byte[] riffHeader = new byte[12];
        byte[] fmtChunk = null;
        byte[] dataChunk = null;
        List<byte[]> otherChunks = new List<byte[]>();

        // Lire structure complète
        using (FileStream fs = File.OpenRead(this.input))
        {
            fs.Read(riffHeader, 0, 12);
            
            while (fs.Position < fs.Length)
            {
                byte[] chunkId = new byte[4];
                byte[] chunkSizeBytes = new byte[4];
                fs.Read(chunkId, 0, 4);
                fs.Read(chunkSizeBytes, 0, 4);
                
                int chunkSize = BitConverter.ToInt32(chunkSizeBytes, 0);
                byte[] chunkData = new byte[chunkSize];
                fs.Read(chunkData, 0, chunkSize);

                // Gestion du padding
                if (chunkSize % 2 != 0) fs.ReadByte();

                string chunkName = Encoding.ASCII.GetString(chunkId);
                switch (chunkName)
                {
                    case "fmt ":
                        fmtChunk = chunkData;
                        break;
                    case "data":
                        dataChunk = chunkData;
                        break;
                    default:
                        otherChunks.Add(chunkId);
                        otherChunks.Add(chunkSizeBytes);
                        otherChunks.Add(chunkData);
                        if (chunkSize % 2 != 0) otherChunks.Add(new byte[] { 0 });
                        break;
                }
            }
        }

        // Vérifier le format audio
        short bitsPerSample = BitConverter.ToInt16(fmtChunk, 14);
        if (bitsPerSample != 16) throw new Exception("Uniquement 16-bit PCM supporté");

        // Amplification
        for (int i = 0; i < dataChunk.Length; i += 2)
        {
            short sample = BitConverter.ToInt16(dataChunk, i);
            sample = (short)Math.Clamp(sample * gain, short.MinValue, short.MaxValue);
            BitConverter.GetBytes(sample).CopyTo(dataChunk, i);
        }

        // Réécriture du fichier
        using (FileStream fs = File.Create(outputPath))
        {
            fs.Write(riffHeader, 0, 12);
            
            // Réécrire le chunk fmt
            fs.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
            fs.Write(BitConverter.GetBytes(fmtChunk.Length), 0, 4);
            fs.Write(fmtChunk, 0, fmtChunk.Length);
            if (fmtChunk.Length % 2 != 0) fs.WriteByte(0);

            // Réécrire le chunk data
            fs.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
            fs.Write(BitConverter.GetBytes(dataChunk.Length), 0, 4);
            fs.Write(dataChunk, 0, dataChunk.Length);
            if (dataChunk.Length % 2 != 0) fs.WriteByte(0);

            // Réécrire les autres chunks
            foreach (byte[] chunkPart in otherChunks)
                fs.Write(chunkPart, 0, chunkPart.Length);
        }
    }
}