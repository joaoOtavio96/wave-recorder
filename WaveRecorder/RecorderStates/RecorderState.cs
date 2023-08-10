using WaveRecorder.Capture;

namespace WaveRecorder.RecorderStates;

public abstract class RecorderState
{
    protected RecorderStateContext _context = null!;

    public void SetContext(RecorderStateContext context) => _context = context;

    public abstract void ToggleRecording();

    public abstract void Execute(CaptureService waveIn, CaptureModel? captureModel);

    public abstract string Status();
}
