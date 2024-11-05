using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                Filter = "音频文件 (*.mp3;*.aac)|*.mp3;*.aac|视频文件 (*.mp4)|*.mp4",
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
