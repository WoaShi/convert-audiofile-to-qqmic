using AudioToMicWPF.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AudioToMicWPF
{
    /// <summary>
    /// ListAlllWindows.xaml 的交互逻辑
    /// </summary>
    public partial class ListAllWindows : Window
    {
        public static Process? QQProcess;
        public ListAllWindows()
        {
            InitializeComponent();
            LoadRunningProcesses();
        }

        private void LoadRunningProcesses()
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    processListBox.Items.Add(new ProcessDisplay(process));
                }
            }
        }

        private void SetFocusButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (processListBox.SelectedItem is ProcessDisplay selectedProcessDisplay)
            {   
                QQProcess = selectedProcessDisplay.Process;
                mainWindow.proccessName.Text = selectedProcessDisplay.Name;
                Close();
            }
        }

        private void processListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }


        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            processListBox.Items.Clear();
            LoadRunningProcesses();
        }
    }

    public class WindowInterop
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }


    public class ProcessDisplay
    {
        public string Name { get; set; }
        public Process Process { get; set; }

        public ProcessDisplay(Process process)
        {
            Process = process;
            Name = process.MainWindowTitle;
        }

        public override string ToString()
        {
            return Name; // 当 ListBox 显示 ProcessDisplay 对象时调用
        }
    }
}


