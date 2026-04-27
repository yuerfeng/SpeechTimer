using System;
using System.IO;
using System.Media;

namespace SpeechTimer
{
  public static class SoundGenerator
  {
    private static readonly string _tickPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "tick.wav");
    private static readonly string _alertPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "alert.wav");

    public static void PlayTick()
    {
      try
      {
        if (File.Exists(_tickPath))
        {
          using (var player = new SoundPlayer(_tickPath))
          {
            player.Play();
          }
        }
      }
      catch
      {
        // 静默处理
      }
    }

    public static void PlayAlert()
    {
      try
      {
        if (File.Exists(_alertPath))
        {
          using (var player = new SoundPlayer(_alertPath))
          {
            player.Play();
          }
        }
      }
      catch
      {
        // 静默处理
      }
    }
  }
}
