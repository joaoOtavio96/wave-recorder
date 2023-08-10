using WaveRecorder.Capture;

namespace WaveRecorder.RecorderStates;

public class StopRecordingState : RecorderState
{
    public override void Execute(CaptureService waveIn, CaptureModel? captureModel)
    {
        if (!captureModel.Data.Any())
            return;

        waveIn.StopRecording();

        var waveFile = captureModel.CreateFile();
        File.WriteAllBytes(waveFile.Metadata.FileName, waveFile.GetBytes());
        captureModel.ClearData();
    }

    public override string Status() => string.Empty;

    public override void ToggleRecording() => _context.TransitionTo(new StartRecordingState());
}
