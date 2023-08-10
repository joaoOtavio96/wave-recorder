using CommandLine;

namespace WaveRecorder.Infra;

public class Options
{
    [Option("channels-number", Default = 2)]
    public short ChannelsNumber { get; set; }

    [Option("sample-rate", Default = 48000)]
    public int SampleRate { get; set; }

    [Option("sample-bits", Default = 16)]
    public short SampleBits { get; set; }
}
