using WaveRecorder.Capture;

namespace WaveRecorder.RecorderStates;

public class RecorderStateContext
{
    private RecorderState _state = null!;

    public RecorderStateContext(RecorderState state) => TransitionTo(state);

    public void TransitionTo(RecorderState state)
    {
        _state = state;
        _state.SetContext(this);
    }

    public void ToggleRecording() => _state.ToggleRecording();

    public void Execute(CaptureService waveIn, CaptureModel? captureModel) => _state.Execute(waveIn, captureModel);

    public string GetStatus() => _state.Status();
}
