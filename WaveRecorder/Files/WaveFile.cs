using System.Text;

namespace WaveRecorder.Files;
public class WaveFile
{
    public WaveFile(short channelsNumber, int sampleRate, short sampleBits, byte[] data)
    {
        int byteRate = sampleRate * channelsNumber * sampleBits / 8;  
        short blockAlign = (short)(channelsNumber * sampleBits / 8);
        int subChunk2Size = data.Length * channelsNumber * sampleBits / 8;
        int fileSize = 4 + (8 + SubChunk1Size) + (8 + subChunk2Size) - 8;

        File = new List<byte[]>
        {
            Riff,
            BitConverter.GetBytes(fileSize),
            Format,
            SubChunk1Id,
            BitConverter.GetBytes(SubChunk1Size),
            BitConverter.GetBytes(AudioFormat),
            BitConverter.GetBytes(channelsNumber),
            BitConverter.GetBytes(sampleRate),
            BitConverter.GetBytes(byteRate),
            BitConverter.GetBytes(blockAlign),
            BitConverter.GetBytes(sampleBits),
            SubChunk2Id,
            BitConverter.GetBytes(fileSize),
            data
        };

        Metadata = new FileMetadata()
        {
            FileName = $"audio{DateTime.Now.ToString("ddMMyyyHHmmss")}.wav",
            Size = GetBytes().Length
        };
    }


    public FileMetadata Metadata { get; private set; }
    public IList<byte[]> File { get; private set; }
    public byte[] Riff => Encoding.ASCII.GetBytes("RIFF");
    public byte[] Format => Encoding.ASCII.GetBytes("WAVE");
    public byte[] SubChunk1Id => Encoding.ASCII.GetBytes("fmt ");
    public byte[] SubChunk2Id => Encoding.ASCII.GetBytes("data");
    public int SubChunk1Size => 16;
    public short AudioFormat => 1;

    public byte[] GetBytes() => File.SelectMany(x => x).ToArray();

}
