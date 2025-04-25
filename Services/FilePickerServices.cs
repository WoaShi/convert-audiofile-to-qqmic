using Microsoft.Win32;

namespace AudioToMicWPF.Services
{
    internal class FilePickerServices
    {
        public string OpenFilePicker()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "选择目标音频",
                Filter = "音频文件 (*.mp3;*.aac;*.flac;*.ogg;*.wav)|*.mp3;*.aac;*.flac;*.ogg;*.wav",
                Multiselect = false // 设置为 true ：允许多选
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                selectedFilePath = selectedFilePath.Replace("\\", @"\\");
                return selectedFilePath;
            }
            else
            {
                return "未选择音频文件！";
            }
        }
    }
}
