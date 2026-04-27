using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SpeechTimer
{
    public partial class DesktopTimerWindow : Window
    {
        private bool _isDragging = false;
        private Point _dragStartPoint;

        public event EventHandler OpenMainWindowRequested;

        public DesktopTimerWindow()
        {
            InitializeComponent();
        }

        public void UpdateTime(TimeSpan time)
        {
            if (time < TimeSpan.Zero)
            {
                TimeDisplay.Text = "-" + time.Negate().ToString(@"hh\:mm\:ss");
                TimeDisplay.Foreground = new SolidColorBrush(Colors.Red);
                WindowBorder.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else if (time.TotalSeconds <= 10 && time.TotalSeconds > 0)
            {
                TimeDisplay.Text = time.ToString(@"hh\:mm\:ss");
                TimeDisplay.Foreground = new SolidColorBrush(Colors.Red);
                WindowBorder.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else if (time.TotalSeconds <= 30 && time.TotalSeconds > 10)
            {
                TimeDisplay.Text = time.ToString(@"hh\:mm\:ss");
                TimeDisplay.Foreground = new SolidColorBrush(Colors.Yellow);
                WindowBorder.BorderBrush = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                TimeDisplay.Text = time.ToString(@"hh\:mm\:ss");
                TimeDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4FF"));
                WindowBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3300B4FF"));
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OpenMainWindowRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            _isDragging = true;
            _dragStartPoint = e.GetPosition(this);
            this.CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPosition = e.GetPosition(this);
                Vector diff = currentPosition - _dragStartPoint;
                this.Left += diff.X;
                this.Top += diff.Y;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _isDragging = false;
            this.ReleaseMouseCapture();
            base.OnMouseLeftButtonUp(e);
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            WindowBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E61A1A2E"));
            WindowBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9900B4FF"));
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            WindowBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC1A1A2E"));
            WindowBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3300B4FF"));
        }
    }
}
