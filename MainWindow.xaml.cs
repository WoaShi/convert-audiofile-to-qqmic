using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AudioToMicWPF.Services;
using NAudio;
using NAudio.Wave;


namespace AudioToMicWPF
{

    public partial class MainWindow : Window
    {
        static string? selectedFilePath;
        public MainWindow()
        {
            InitializeComponent();
            FindDevicesServices findDevicesServices = new FindDevicesServices();
            devicesList.ItemsSource = findDevicesServices.outputDevices;
        }

        private void OnPickFile(object sender, RoutedEventArgs e)
        {

            FilePickerServices filePickerServices = new FilePickerServices();

            selectedFilePath = filePickerServices.OpenFilePicker();
            pathShower.Text = "当前音频文件：" +'\n' + selectedFilePath;

        }

        private void devicesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var selectedValue = devicesList.SelectedValue;
            if (selectedValue != null)
            {
               currentDevice.Text = "已选择声卡ID: " + selectedValue.ToString();
            }
        }

        private void OnPlayAudio(object sender, RoutedEventArgs e)
        {
            if(pathShower.Text != "参数错误！" || pathShower.Text != "音频路径...")
            {
                WindowInterop.SetForegroundWindow(ListAllWindows.QQProcess.MainWindowHandle);
                Thread.Sleep(200);
                NAudioServices nAudioServices = new(new NAudio.Wave.AudioFileReader(selectedFilePath), devicesList.SelectedIndex);
            }
            else if(selectedFilePath == null)
            {
                System.Windows.MessageBox.Show("未选择音频。", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("音频参数选择错误！", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GetQQWindow(object sender, RoutedEventArgs e)
        {
            ListAllWindows listAllWindows = new ListAllWindows();
            listAllWindows.ShowDialog();
        }

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
    }
}