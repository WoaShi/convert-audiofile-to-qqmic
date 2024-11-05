using AudioToMicWPF.Services;
using NAudio.Wave;
using System.Threading;
using System.Threading.Tasks;

public class NAudioServices
{
    //private readonly HoldSpaceSimulator _holdSpaceSimulator;
    private readonly ClickSimulator clickSimulator;
    private readonly LocateMicButton locateMicButton;

    public NAudioServices(AudioFileReader audioFile, int selectedDevice)
    {
        clickSimulator = new ClickSimulator();
        locateMicButton = new LocateMicButton();
        PlayAudioAsync(audioFile, selectedDevice).ConfigureAwait(false);
    }

    private async Task PlayAudioAsync(AudioFileReader audioFile, int selectedDevice)
    {
        using (var outputDevice = new WaveOutEvent())
        {
            outputDevice.DeviceNumber = selectedDevice;
            outputDevice.Init(audioFile);

            //_holdSpaceSimulator.PressSpace();

            locateMicButton.MatchAndMoveMouse("Images\\MicButton.png");
            ClickSimulator.HoldMouseLeftButton();
            Thread.Sleep(100);
            outputDevice.Play();

            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                await Task.Delay(200); 
            }
            //_holdSpaceSimulator.ReleaseSpace();
            ClickSimulator.ReleaseMouseLeftButton();

        }
    }
}