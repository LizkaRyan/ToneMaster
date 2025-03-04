using System.Text;

namespace ToneMaster.Audio;

public class WavData
{
    private readonly byte[] _riffHeader;
    public byte[] FmtChunk;
    public byte[] DataChunk;
    private readonly List<byte[]> _otherChunks;
    
    public byte[] RiffHeader => _riffHeader;
    
    public List<byte[]> OtherChunks { get => _otherChunks;}

    public WavData(string filename)
    {
        _riffHeader = new byte[12];
        _otherChunks = new List<byte[]>();
        InitData(filename);
    }

    private void InitData(string filename)
    {
        using (FileStream fs = File.OpenRead(filename))
        {
            fs.Read(this._riffHeader, 0, 12);

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
                        FmtChunk = chunkData;
                        break;
                    case "data":
                        DataChunk = chunkData;
                        break;
                    default:
                        _otherChunks.Add(chunkId);
                        _otherChunks.Add(chunkSizeBytes);
                        _otherChunks.Add(chunkData);
                        if (chunkSize % 2 != 0) _otherChunks.Add(new byte[] { 0 });
                        break;
                }
            }
        }
    }
}