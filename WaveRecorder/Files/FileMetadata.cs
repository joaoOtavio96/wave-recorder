namespace WaveRecorder.Files;

public class FileMetadata
{
    public string FileName { get; set; } = null!;
    public int Size { get; set; }

    public override string ToString()
    {
        return $"{FileName} - {Size / 1024} KB";
    }
}
