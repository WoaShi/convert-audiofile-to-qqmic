using AudioToMicWPF.Services;
using NAudio.Wave;

public class NAudioServices
{
    private readonly WaveStream audioReader;
    private readonly int deviceNumber;
    private readonly double threshold;

    private readonly ClickSimulator clickSimulator;
    private readonly LocateMicButton locateMicButton;

    public NAudioServices(WaveStream audioReader, int deviceNumber, double threshold)
    {
        this.audioReader = audioReader;
        this.deviceNumber = deviceNumber;
        this.threshold = threshold;

        clickSimulator = new ClickSimulator();
        locateMicButton = new LocateMicButton(threshold);
    }

    public async Task PlayAudioAsync()
    {
        using var outputDevice = new WaveOutEvent { DeviceNumber = deviceNumber };
        outputDevice.Init(audioReader);

        locateMicButton.MatchAndMoveMouse("Images\\MicButton.png");

        ClickSimulator.HoldMouseLeftButton();
        await Task.Delay(100);

        outputDevice.Play();

        while (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            await Task.Delay(200);
        }

        ClickSimulator.ReleaseMouseLeftButton();
    }
}
