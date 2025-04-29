using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication1;

public partial class ChangePasswordWindow : Window
{
    public ChangePasswordWindow()
    {
        InitializeComponent();
    }

    public void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
        public void OnYesClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        public void OnCancelClick(object? sender, RoutedEventArgs e)
        {
            Hide();
        }
    }

