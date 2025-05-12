using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace AvaloniaApplication1;

public partial class DashboardWindow : Window
{
    private DispatcherTimer? _dispatcherTimer;
    private DateTime _endTime;
    private int _pomodoroCount = 0;
    private bool _isWorkSession = true;

    public DashboardWindow()
    {
        InitializeComponent();
        CounterBlock = this.FindControl<TextBlock>("CounterBlock");
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        StartWork();
    }

    private void Button_StopClick(object? sender, RoutedEventArgs e)
    {
        _dispatcherTimer?.Stop();
        CounterBlock.Text = "00:00";
    }

    private void StartWork()
    {
        _isWorkSession = true;
        StartTimer(25 * 60); // 25 minutos
    }

    private void StartBreak(int minutes)
    {
        _isWorkSession = false;
        StartTimer(minutes * 60); // 5 ou 15 minutos
    }

    private void StartTimer(int durationInSeconds)
    {
        _dispatcherTimer?.Stop(); // Para timer anterior, se houver
        _endTime = DateTime.Now.AddSeconds(durationInSeconds);

        _dispatcherTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _dispatcherTimer.Tick += UpdateTimer;
        _dispatcherTimer.Start();
    }

    private void UpdateTimer(object? sender, EventArgs e)
    {
        var remaining = _endTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            _dispatcherTimer?.Stop();
            CounterBlock.Text = "00:00";
            OnTimerFinished();
        }
        else
        {
            CounterBlock.Text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
    }

    private void OnTimerFinished()
    {
        if (_isWorkSession)
        {
            _pomodoroCount++;
            if (_pomodoroCount % 4 == 0)
                StartBreak(15);
            else
                StartBreak(5);
        }
        else
        {
            StartWork();
        }
    }
}
