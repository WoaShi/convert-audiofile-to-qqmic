using AudioToMicWPF.Services;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace AudioToMicWPF
{
    public class MainViewModel : INotifyPropertyChanged//用于刷新该窗口的数据
    {
        private string? _selectedFilePath;
        public string? SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (_selectedFilePath != value)
                {
                    _selectedFilePath = value;
                    OnPropertyChanged(nameof(SelectedFilePath));
                }
            }
        }

        private string? _programName;
        public string? ProgramName
        {
            get => _programName;
            set
            {
                if (_programName != value)
                {
                    _programName = value;
                    OnPropertyChanged(nameof(ProgramName));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public partial class MainWindow : Window
    {
        static bool hasChosenAudio = false;
        public MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;

            FindDevicesServices findDevicesServices = new FindDevicesServices();
            devicesList.ItemsSource = findDevicesServices.outputDevices;
        }

        private void OnPickFile(object sender, RoutedEventArgs e)
        {
            FilePickerServices filePickerServices = new FilePickerServices();
            viewModel.SelectedFilePath = filePickerServices.OpenFilePicker();
            hasChosenAudio = viewModel.SelectedFilePath != "未选择音频文件！" ? true : false;
        }

        private void devicesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //已选择的设备id，弃用
            var selectedValue = devicesList.SelectedValue;
        }

        private async void OnPlayAudio(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.SelectedFilePath))
            {
                MessageBox.Show("未选择音频！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (devicesList.SelectedIndex < 0)
            {
                MessageBox.Show("未选择输出设备！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 激活聊天窗口
                WindowInterop.SetForegroundWindow(ListAllWindows.chatProcess.MainWindowHandle);
                await Task.Delay(200); 

                // 构建播放器服务并播放音频
                var audioPath = viewModel.SelectedFilePath;
                var audioReader = AudioDecoderFactory.CreateAudioReader(audioPath); // 支持更多格式
                var nAudioServices = new NAudioServices(audioReader, devicesList.SelectedIndex, thresholdSlider.Value);
                await nAudioServices.PlayAudioAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"播放音频时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GetQQWindow(object sender, RoutedEventArgs e)
        {
            ListAllWindows listAllWindows = new ListAllWindows();
            listAllWindows.ShowDialog();
        }

        //手动截图获取麦克风图标
        private void OnCaptureScreen(object sender, RoutedEventArgs e)
        {
            var captureWindow = new CaptureWindow();
            captureWindow.ShowDialog();

            if (captureWindow.CapturedRect.HasValue)
            {
                System.Drawing.Rectangle rect = captureWindow.CapturedRect.Value;

                using (Bitmap screenShot = new Bitmap(rect.Width, rect.Height))
                {
                    using (Graphics g = Graphics.FromImage(screenShot))
                    {
                        g.CopyFromScreen(rect.Left, rect.Top, 0, 0, screenShot.Size);
                    }

                    string savePath = @"Images\MicButton.png";
                    screenShot.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

                    System.Windows.MessageBox.Show($"截图已保存到：{savePath}", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("未选择截图区域。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}