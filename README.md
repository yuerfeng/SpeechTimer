# 演讲倒计时器

一个简洁专业的演讲倒计时工具，基于 WPF + .NET 9 开发。

## 功能特点

- **大号数字时间显示** — 格式 HH:MM:SS，清晰易读
- **自定义时间设置** — 支持小时、分钟、秒输入
- **预设时间快捷选择** — 5/10/15/30 分钟一键设置
- **智能颜色提示** —
  - 剩余时间 > 50% → 绿色
  - 剩余时间 25-50% → 黄色
  - 剩余时间 < 25% → 红色
- **桌面悬浮窗口** — 半透明无边框小窗口，可拖拽移动，双击打开主窗口
- **系统托盘图标** — 最小化后驻留托盘，双击恢复窗口
- **可选提示音** — 最后 10 秒滴答提示，结束持续响铃 5 秒（默认关闭）

## 编译运行

### 环境要求
- .NET 9 SDK
- Visual Studio 2022（可选，用于IDE开发）

### 命令行编译
```bash
dotnet build -c Debug -p:Platform=x64 SpeechTimer.Wpf.csproj
```

### 运行程序
```bash
dotnet run --project SpeechTimer.Wpf.csproj
```

或直接运行编译后的可执行文件：
```bash
.\bin\x64\Debug\net9.0-windows\SpeechTimer.Wpf.exe
```

## 使用说明

1. **设置时间**：输入小时/分钟/秒，或点击预设按钮
2. **开始倒计时**：点击「开始」按钮，桌面显示悬浮窗口
3. **控制计时**：使用「暂停」「重置」按钮
4. **拖拽窗口**：鼠标按住桌面悬浮窗口可移动位置
5. **声音开关**：勾选「播放提示音」开启倒计时提示音

## 项目结构

```
SpeechTimer/
├── App.xaml                  # 应用入口
├── App.xaml.cs               # 应用逻辑（异常处理）
├── MainWindow.xaml           # 主窗口界面
├── MainWindow.xaml.cs        # 主窗口逻辑（计时器、托盘）
├── DesktopTimerWindow.xaml   # 桌面悬浮窗口界面
├── DesktopTimerWindow.xaml.cs # 桌面悬浮窗口逻辑
├── SoundGenerator.cs         # 提示音播放
├── Assets/
│   ├── main.ico              # 程序图标
│   ├── tick.wav              # 最后10秒提示音
│   └── alert.wav             # 结束响铃音
└── SpeechTimer.Wpf.csproj    # 项目文件
```

## 技术栈

- **UI 框架**：WPF (Windows Presentation Foundation)
- **运行时**：.NET 9
- **语言**：C# / XAML
- **核心组件**：
  - `DispatcherTimer` — 倒计时
  - `NotifyIcon` — 系统托盘
  - `SoundPlayer` — 音频播放
