using Spectre.Console;
using WaveRecorder.Capture;
using WaveRecorder.Infra;
using WaveRecorder.RecorderStates;

namespace WaveRecorder;

public class WaveRecorderApp : App<Options>
{
    private CaptureService _captureService = null!;
    private CaptureModel _captureModel = null!;
    private RecorderStateContext _recorderStateContext = null!;

    public override void Init(Options args)
    {
        _captureModel = new CaptureModel()
        {
            ChannelsNumber = args.ChannelsNumber,
            SampleRate = args.SampleRate,
            SampleBits = args.SampleBits
        };

        _captureService = new CaptureService(_captureModel);
        _recorderStateContext = new RecorderStateContext(new StopRecordingState());
    }

    public override void Update()
    {
        if (!Console.KeyAvailable)
            return;

        var keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.Spacebar)
        {
            _recorderStateContext.ToggleRecording();
            _recorderStateContext.Execute(_captureService, _captureModel);                  
        }   
    }

    public override void Draw()
    {
        AnsiConsole.Write(new FigletText("Wave Recorder").LeftJustified().Color(Color.LightSlateBlue));
        AnsiConsole.WriteLine($"Number of channel: {_captureModel.ChannelsNumber} - Sample rate: {_captureModel.SampleRate} - Sample bits: {_captureModel.SampleBits}");
        AnsiConsole.WriteLine("Press space to start recording");
        AnsiConsole.MarkupLine($"{_recorderStateContext.GetStatus()}");
        AnsiConsole.WriteLine(" ");
        AnsiConsole.WriteLine("Saved files:");
        _captureModel.SavedFiles.ForEach(x => AnsiConsole.WriteLine(x.ToString()));
    }
}
