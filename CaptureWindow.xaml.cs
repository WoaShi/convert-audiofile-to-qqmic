using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace AudioToMicWPF
{
    public partial class CaptureWindow : Window
    {
        private Point? _firstPoint;
        private Point? _currentPoint;
        private System.Drawing.Rectangle? _capturedRect;
        private bool _isFinalized;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        public System.Drawing.Rectangle? CapturedRect => _isFinalized ? _capturedRect : null;

        public CaptureWindow()
        {
            InitializeComponent();

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = 0;
            Top = 0;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;

            // 确保按钮初始化完成
            ButtonPanel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            ButtonPanel.Arrange(new Rect(0, 0, ButtonPanel.DesiredSize.Width, ButtonPanel.DesiredSize.Height));
        }

        private void CanvasRoot_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFinalized) return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var clickPoint = e.GetPosition(this);

                if (!_firstPoint.HasValue)
                {
                    // 第一次点击：设置起点
                    _firstPoint = clickPoint;
                    SelectionRect.Visibility = Visibility.Visible;
                }
                else
                {
                    // 第二次点击：锁定选区
                    _currentPoint = clickPoint;
                    FinalizeSelection();
                    ShowConfirmationButtons();
                    _isFinalized = true;
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                ResetSelection();
            }
        }

        private void CanvasRoot_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isFinalized) return;

            if (_firstPoint.HasValue && !_currentPoint.HasValue)
            {
                var currentPos = e.GetPosition(this);
                UpdateSelectionRect(_firstPoint.Value, currentPos);
            }
        }

        private void CanvasRoot_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // 仅处理第二次点击后的状态
            if (_firstPoint.HasValue && _currentPoint.HasValue)
            {
                FinalizeSelection();
            }
        }

        private void UpdateSelectionRect(Point start, Point end)
        {
            var (x1, y1, x2, y2) = NormalizePoints(start, end);

            Canvas.SetLeft(SelectionRect, x1);
            Canvas.SetTop(SelectionRect, y1);
            SelectionRect.Width = x2 - x1;
            SelectionRect.Height = y2 - y1;
        }

        private (double x1, double y1, double x2, double y2) NormalizePoints(Point p1, Point p2)
        {
            return (
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Max(p1.X, p2.X),
                Math.Max(p1.Y, p2.Y)
            );
        }

        private void ShowConfirmationButtons()
        {
            // 计算按钮位置
            var (_, _, x2, y2) = NormalizePoints(_firstPoint.Value, _currentPoint.Value);

            // 确保按钮在屏幕内
            double maxX = SystemParameters.VirtualScreenWidth - ButtonPanel.ActualWidth - 10;
            double maxY = SystemParameters.VirtualScreenHeight - ButtonPanel.ActualHeight - 10;

            Canvas.SetLeft(ButtonPanel, Math.Min(x2, maxX));
            Canvas.SetTop(ButtonPanel, Math.Min(y2 + 10, maxY));

            ButtonPanel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ButtonPanel, 9999); // 确保按钮在最上层
        }

        private void FinalizeSelection()
        {
            var start = PointToScreen(_firstPoint.Value);
            var end = PointToScreen(_currentPoint.Value);

            _capturedRect = new System.Drawing.Rectangle(
                (int)Math.Min(start.X, end.X),
                (int)Math.Min(start.Y, end.Y),
                (int)Math.Abs(end.X - start.X),
                (int)Math.Abs(end.Y - start.Y)
            );
        }

        private void ResetSelection()
        {
            _firstPoint = null;
            _currentPoint = null;
            _capturedRect = null;
            _isFinalized = false;
            SelectionRect.Visibility = Visibility.Collapsed;
            ButtonPanel.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_capturedRect.HasValue && _capturedRect.Value.Width > 5 && _capturedRect.Value.Height > 5)
                {
                    var imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    Directory.CreateDirectory(imagesDir);

                    using (var bmp = new Bitmap(_capturedRect.Value.Width, _capturedRect.Value.Height))
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(
                            new System.Drawing.Point(_capturedRect.Value.X, _capturedRect.Value.Y),
                            System.Drawing.Point.Empty,
                            bmp.Size
                        );
                        bmp.Save(Path.Combine(imagesDir, "MicButton.png"), ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}");
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // 重置所有状态
            _firstPoint = null;
            _currentPoint = null;
            _capturedRect = null;
            _isFinalized = false;

            // 清除界面元素
            SelectionRect.Visibility = Visibility.Collapsed;
            ButtonPanel.Visibility = Visibility.Collapsed;

            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            // 最终状态清理
            if (!_isFinalized)
            {
                _capturedRect = null;
            }
            base.OnClosed(e);
            Application.Current.MainWindow?.Activate();
        }
    }
}
