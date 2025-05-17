using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System.Media;
using System.Threading;
using Avalonia.Input;
using Avalonia.Media;
using NAudio.Wave;

namespace AvaloniaApplication1;

public partial class DashboardWindow : Window
{
    private IWavePlayer? _player;
    private AudioFileReader? _audioFileReader;
    private DispatcherTimer? _dispatcherTimer;
    private DateTime _endTime;
    private int _pomodoroCount = 0;
    private bool _isWorkSession = true;
    private ObservableCollection<TodoItem> _todoItems;
    private ObservableCollection<TodoItem> _inProgressItems;
    private ObservableCollection<TodoItem> _completedItems;

    public DashboardWindow()
    {
        InitializeComponent();
        
        _todoItems = new ObservableCollection<TodoItem>();
        _inProgressItems = new ObservableCollection<TodoItem>();
        _completedItems = new ObservableCollection<TodoItem>();
        
        var todoList = this.FindControl<ItemsControl>("TodoList");
        var inProgressList = this.FindControl<ItemsControl>("InProgressList");
        var completedList = this.FindControl<ItemsControl>("CompletedList");
        
        if (todoList != null) todoList.ItemsSource = _todoItems;
        if (inProgressList != null) inProgressList.ItemsSource = _inProgressItems;
        if (completedList != null) completedList.ItemsSource = _completedItems;


        AddSampleItems();
        
        CounterBlock = this.FindControl<TextBlock>("CounterBlock");
        FirstTimerBreak = this.FindControl<TextBlock>("FirstTimerBreak");
    }

    //Pomodoro Timer Implementation 
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        StartWork();
    }

    private void Button_StopClick(object? sender, RoutedEventArgs e)
    {
        _dispatcherTimer?.Stop();
        CounterBlock.Text = "25:00";
        FirstTimerBreak.Text = "";


    }

    private void StartWork()
    {
        _isWorkSession = true;
        StartTimer(5); 
        FirstTimerBreak.Text = "You will get 5 minutes of break! Keep going bro.";

    }

    private void StartBreak(int minutes)
    {
        
        _isWorkSession = false;
        StartTimer(minutes * 60); 
    }

    private void StartTimer(int durationInSeconds)
    {
        _dispatcherTimer?.Stop(); 
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
                FirstTimerBreak.Text = "Rest a little bit please... just 5 minutes! ";

        }
        else
        {
            StartWork();
        }
    }
    
    //To-do Implementation
    private void AddSampleItems()
    {
        _todoItems.Add(new TodoItem("Do the dishes"));
        _todoItems.Add(new TodoItem("Do the laundry")); 
        _todoItems.Add(new TodoItem("Do your bed"));
        
        var todoList = this.FindControl<ItemsControl>("TodoList");
        if (todoList != null) todoList.ItemsSource = _todoItems;
        
    }

    private async void AddTask_Click(object? sender, RoutedEventArgs e)
    {
        var addButton = this.FindControl<Button>("AddTaskButton");
        var textBox = this.FindControl<TextBox>("NewTaskTextBox");
   
      
        if (addButton != null && textBox != null)
        {
            addButton.IsVisible = false;
            textBox.IsVisible = true;
            textBox.Focus();
        }
    }


    private int currentTaskIndex = 0;
    private void NewTaskTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                var taskBlock = this.FindControl<TextBlock>("test");
                if (taskBlock != null)
                {
                    taskBlock.Text = textBox.Text.ToUpper();
                    currentTaskIndex = (currentTaskIndex + 1) % 6;
                }
                
                textBox.Text = "";
                textBox.IsVisible = false;
                
                var addButton = this.FindControl<Button>("AddTaskButton");
                if (addButton != null)
                {
                    addButton.IsVisible = true;
                }
            }
            
        else if (e.Key == Key.Escape)
        {
            textBox.Text = "";
            textBox.IsVisible = false;
            
            var addButton = this.FindControl<Button>("AddTaskButton");
            if (addButton != null)
            {
                addButton.IsVisible = true;
            }
        }    
        }
    }
    
    
}
