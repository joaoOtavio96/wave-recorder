using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace WaveRecorder.Capture;

public class CaptureService
{
    private readonly WasapiLoopbackCapture _waveIn = null!;

    public CaptureService(CaptureModel captureModel)
    {
        _waveIn = new WasapiLoopbackCapture();
        _waveIn.DataAvailable += (sender, e) => DataAvailableCallback(sender, e, captureModel);
    }

    public int BytesRecorded { get; private set; }
    public bool RecordingBytes => BytesRecorded > 0;

    public void StartRecording() => _waveIn.StartRecording();
    public void StopRecording() => _waveIn.StopRecording();

    private void DataAvailableCallback(object sender, WaveInEventArgs e, CaptureModel captureModel)
    {
        BytesRecorded = e.BytesRecorded;

        if (!RecordingBytes)
            return;

        var bufferPcm = ConvertIeeeTo16BitPCM(e.BytesRecorded, _waveIn.WaveFormat, captureModel.SampleRate, e.Buffer);
        captureModel.AddBufferData(bufferPcm);
    }

    private byte[] ConvertIeeeTo16BitPCM(int bytesRecorded, WaveFormat waveFormat, int sampleRate, byte[] buffer)
    {
        using var ieeeStream = new MemoryStream(buffer, 0, bytesRecorded);
        using var inputIeeeStream = new RawSourceWaveStream(ieeeStream, waveFormat);

        var convertedPCM = new SampleToWaveProvider16(
            new WdlResamplingSampleProvider(
                new WaveToSampleProvider(inputIeeeStream),
                sampleRate)
            );

        byte[] convertedPCMBuffer = new byte[bytesRecorded];
        using var pcmStream = new MemoryStream();
        int read;
        while ((read = convertedPCM.Read(convertedPCMBuffer, 0, bytesRecorded)) > 0)
        {
            pcmStream.Write(convertedPCMBuffer, 0, read);
        }

        return pcmStream.ToArray();
    }
}
