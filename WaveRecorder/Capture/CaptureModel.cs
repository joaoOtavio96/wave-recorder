using WaveRecorder.Files;

namespace WaveRecorder.Capture;

public class CaptureModel
{
    public CaptureModel()
    {
        Data = new List<byte[]>();
        SavedFiles = new List<FileMetadata>();
    }

    public List<byte[]> Data { get; set; }
    public List<FileMetadata> SavedFiles { get; set; }
    public short ChannelsNumber { get; set; }
    public int SampleRate { get; set; }
    public short SampleBits { get; set; }

    public WaveFile CreateFile()
    {
        var wavFile = new WaveFile(ChannelsNumber, SampleRate, SampleBits, GetDataBytes());
        SavedFiles.Add(wavFile.Metadata);

        return wavFile;
    }

    public void AddBufferData(byte[] buffer) => Data.Add(buffer);

    public byte[] GetDataBytes() => Data.ToList().SelectMany(x => x).ToArray();

    public void ClearData() => Data.Clear();
}
