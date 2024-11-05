using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Windows;


namespace AudioToMicWPF.Services
{
    internal class LocateMicButton
    {

        public void MatchAndMoveMouse(string targetImagePath)
        {
            // 捕获屏幕(此时已推送QQ至焦点)
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap screenShot = new Bitmap(screenBounds.Width, screenBounds.Height))
            {
                using (Graphics g = Graphics.FromImage(screenShot))
                {
                    g.CopyFromScreen(0, 0, 0, 0, screenShot.Size);
                }

                // Bitmap -> OpenCvSharp Mat
                Mat screenMat = BitmapConverter.ToMat(screenShot);

                // 加载目标图片
                Mat targetMat = Cv2.ImRead(targetImagePath, ImreadModes.Color);
                if (targetMat.Empty())
                {
                    System.Windows.MessageBox.Show("图片错误！", "Title", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                Cv2.CvtColor(screenMat, screenMat, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(targetMat, targetMat, ColorConversionCodes.BGR2GRAY);


                // 使用模板匹配
                Mat result = new Mat();
                Cv2.MatchTemplate(screenMat, targetMat, result, TemplateMatchModes.CCoeffNormed);

                // 获取匹配的最大值和位置
                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                // 设置匹配的阈值
                double threshold = 0.7;
                if (maxVal >= threshold)
                {
                    // 计算匹配位置的中心
                    System.Drawing.Point matchCenter = new(maxLoc.X + targetMat.Width / 2, maxLoc.Y + targetMat.Height / 2);

                    // 移动鼠标到匹配位置
                    Cursor.Position = new System.Drawing.Point(matchCenter.X, matchCenter.Y);
                    //System.Windows.MessageBox.Show($"找到匹配，鼠标已移动到位置：{matchCenter}");
                }
                else
                {
                    //System.Windows.MessageBox.Show("未找到足够匹配的图像。");
                }

                // 释放资源
                screenMat.Dispose();
                targetMat.Dispose();
                result.Dispose();
            }
        }
    }
}

