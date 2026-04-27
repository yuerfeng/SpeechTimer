using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SpeechTimer
{
  public partial class MainWindow : Window
  {
    private TimeSpan _remainingTime = TimeSpan.Zero;
    private TimeSpan _totalTime = TimeSpan.Zero;
    private bool _isRunning = false;
    private DispatcherTimer _timer;
    private DispatcherTimer _alertTimer;
    private DesktopTimerWindow _desktopTimerWindow;
    private System.Windows.Forms.NotifyIcon _notifyIcon;

    public MainWindow()
    {
      InitializeComponent();
      InitializeDefaults();
      InitializeTimer();
      InitializeNotifyIcon();
    }

    private void InitializeDefaults()
    {
      HoursInput.Text = "0";
      MinutesInput.Text = "10";
      SecondsInput.Text = "0";
      UpdateTimeDisplay();
    }

    private void InitializeTimer()
    {
      _timer = new DispatcherTimer();
      _timer.Interval = TimeSpan.FromSeconds(1);
      _timer.Tick += Timer_Tick;

      _alertTimer = new DispatcherTimer();
      _alertTimer.Interval = TimeSpan.FromMilliseconds(500);
      _alertTimer.Tick += AlertTimer_Tick;
    }

    private void InitializeNotifyIcon()
    {
      _notifyIcon = new System.Windows.Forms.NotifyIcon();
      _notifyIcon.Text = "演讲倒计时器";

      string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "main.ico");
      if (File.Exists(iconPath))
      {
        _notifyIcon.Icon = new System.Drawing.Icon(iconPath);
      }
      else
      {
        _notifyIcon.Icon = System.Drawing.SystemIcons.Application;
      }

      _notifyIcon.Visible = true;
      _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

      var contextMenu = new System.Windows.Forms.ContextMenuStrip();
      var showItem = new System.Windows.Forms.ToolStripMenuItem("显示窗口");
      showItem.Click += (s, e) =>
      {
        this.Show();
        this.WindowState = WindowState.Normal;
        this.Activate();
      };
      contextMenu.Items.Add(showItem);

      var exitItem = new System.Windows.Forms.ToolStripMenuItem("退出");
      exitItem.Click += (s, e) =>
      {
        _notifyIcon.Dispose();
        Application.Current.Shutdown();
      };
      contextMenu.Items.Add(exitItem);

      _notifyIcon.ContextMenuStrip = contextMenu;
    }

    private void NotifyIcon_DoubleClick(object sender, EventArgs e)
    {
      this.Show();
      this.WindowState = WindowState.Normal;
      this.Activate();
    }

    private void DesktopTimerWindow_OpenMainWindowRequested(object sender, EventArgs e)
    {
      this.Show();
      this.WindowState = WindowState.Normal;
      this.Activate();
    }

    protected override void OnStateChanged(EventArgs e)
    {
      if (WindowState == WindowState.Minimized)
      {
        this.Hide();
      }
      base.OnStateChanged(e);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      if (_remainingTime.TotalSeconds > 0)
      {
        _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
        UpdateTimeDisplay();
        UpdateVisualFeedback();

        // 最后10秒每秒播放提示音
        if (_remainingTime.TotalSeconds <= 10 && _remainingTime.TotalSeconds > 0)
        {
          PlayTickSound();
        }

        if (_desktopTimerWindow != null)
        {
          _desktopTimerWindow.UpdateTime(_remainingTime);
        }
      }
      else
      {
        _timer.Stop();
        _isRunning = false;
        StartButton.IsEnabled = true;
        PauseButton.IsEnabled = false;
        StatusDisplay.Text = "时间到！";
        TimeDisplay.Foreground = new SolidColorBrush(Colors.Red);

        // 开始持续响铃
        StartContinuousAlert();

        if (_desktopTimerWindow != null)
        {
          _desktopTimerWindow.Close();
          _desktopTimerWindow = null;
        }
      }
    }

    private void AlertTimer_Tick(object sender, EventArgs e)
    {
      PlayAlertSound();
    }

    private void StartContinuousAlert()
    {
      // 先播放一次
      PlayAlertSound();
      // 然后每500毫秒播放一次，持续响铃
      _alertTimer.Start();
      // 5秒后自动停止响铃
      var stopAlertTimer = new DispatcherTimer();
      stopAlertTimer.Interval = TimeSpan.FromSeconds(5);
      stopAlertTimer.Tick += (s, args) =>
      {
        _alertTimer.Stop();
        stopAlertTimer.Stop();
      };
      stopAlertTimer.Start();
    }

    private void PlayTickSound()
    {
      if (SoundCheckBox?.IsChecked != true) return;
      try
      {
        SoundGenerator.PlayTick();
      }
      catch
      {
        // 静默处理
      }
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
      if (!_isRunning)
      {
        if (_remainingTime.TotalSeconds <= 0)
        {
          int hours = int.TryParse(HoursInput.Text, out int h) ? h : 0;
          int minutes = int.TryParse(MinutesInput.Text, out int m) ? m : 0;
          int seconds = int.TryParse(SecondsInput.Text, out int s) ? s : 0;

          _remainingTime = new TimeSpan(hours, minutes, seconds);
          _totalTime = _remainingTime;
        }

        if (_remainingTime.TotalSeconds > 0)
        {
          _isRunning = true;
          StartButton.IsEnabled = false;
          PauseButton.IsEnabled = true;
          StatusDisplay.Text = "倒计时进行中...";

          if (_desktopTimerWindow == null)
          {
            _desktopTimerWindow = new DesktopTimerWindow();
            _desktopTimerWindow.Left = 100;
            _desktopTimerWindow.Top = 100;
            _desktopTimerWindow.OpenMainWindowRequested += DesktopTimerWindow_OpenMainWindowRequested;
          }
          _desktopTimerWindow.Show();
          _desktopTimerWindow.UpdateTime(_remainingTime);

          _timer.Start();
        }
      }
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
      if (_isRunning)
      {
        _timer.Stop();
        _alertTimer.Stop();
        _isRunning = false;
        StartButton.IsEnabled = true;
        PauseButton.IsEnabled = false;
        StatusDisplay.Text = "已暂停";
      }
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
      _timer.Stop();
      _alertTimer.Stop();
      _isRunning = false;
      _remainingTime = TimeSpan.Zero;
      StartButton.IsEnabled = true;
      PauseButton.IsEnabled = false;
      StatusDisplay.Text = "请设置倒计时时间";
      TimeDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4FF"));
      UpdateTimeDisplay();

      if (_desktopTimerWindow != null)
      {
        _desktopTimerWindow.Close();
        _desktopTimerWindow = null;
      }
    }

    private void PresetButton_Click(object sender, RoutedEventArgs e)
    {
      _timer.Stop();
      _isRunning = false;
      StartButton.IsEnabled = true;
      PauseButton.IsEnabled = false;

      string preset = ((Button)sender).Tag.ToString();
      if (TimeSpan.TryParse(preset, out TimeSpan time))
      {
        HoursInput.Text = time.Hours.ToString();
        MinutesInput.Text = time.Minutes.ToString();
        SecondsInput.Text = time.Seconds.ToString();
        _remainingTime = time;
        _totalTime = time;
        UpdateTimeDisplay();
        StatusDisplay.Text = "预设时间已设置";
        TimeDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4FF"));
      }
    }

    private void UpdateTimeDisplay()
    {
      TimeDisplay.Text = _remainingTime.ToString(@"hh\:mm\:ss");
    }

    private void PlayAlertSound()
    {
      if (SoundCheckBox?.IsChecked != true) return;
      try
      {
        SoundGenerator.PlayAlert();
      }
      catch
      {
        // 如果播放失败，静默处理
      }
    }

    private void UpdateVisualFeedback()
    {
      double timePercentage = _remainingTime.TotalSeconds / _totalTime.TotalSeconds;

      if (timePercentage > 0.5)
      {
        TimeDisplay.Foreground = new SolidColorBrush(Colors.LimeGreen);
      }
      else if (timePercentage > 0.25)
      {
        TimeDisplay.Foreground = new SolidColorBrush(Colors.Yellow);
      }
      else
      {
        TimeDisplay.Foreground = new SolidColorBrush(Colors.Red);
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      _timer.Stop();
      _alertTimer?.Stop();
      _notifyIcon?.Dispose();
      if (_desktopTimerWindow != null)
      {
        _desktopTimerWindow.Close();
      }
      base.OnClosing(e);
    }
  }
}
