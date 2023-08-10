using WaveRecorder.Capture;

namespace WaveRecorder.RecorderStates;

public class StartRecordingState : RecorderState
{
    public override void Execute(CaptureService waveIn, CaptureModel? captureModel) => waveIn.StartRecording();

    public override string Status() => "[bold red]Recording...[/]";

    public override void ToggleRecording() => _context.TransitionTo(new StopRecordingState());
}
