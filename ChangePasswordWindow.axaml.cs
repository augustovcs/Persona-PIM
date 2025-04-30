using System;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
namespace AvaloniaApplication1;

public partial class ChangePasswordWindow : Window
{
    private string _savedPassword;
    private const string ConfigFileName = "FirstTimeCredentials.json";

    public ChangePasswordWindow()
    {
        InitializeComponent();
        CurrentPasswordBox = this.FindControl<TextBox>("CurrentPasswordBox");
        NewPasswordBox = this.FindControl<TextBox>("NewPasswordBox");
        ConfirmPasswordBox = this.FindControl<TextBox>("ConfirmPasswordBox");
        ErrorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock");

        var savedCredentials = GetSavedCredentials();
        if (savedCredentials != null)
        {
            _savedPassword = getCredentials().Password;
        }
        
    }

    public void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
    }

    public void OnYesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var _currentPassword = CurrentPasswordBox.Text ?? string.Empty;
            var _newPassword = NewPasswordBox.Text ?? string.Empty;
            var _confirmnewPassword = ConfirmPasswordBox.Text ?? string.Empty;

            if (_confirmnewPassword != _newPassword)
            {
                ErrorTextBlock.Text = "New passwords do not match!";
                return;
            }

            if (_currentPassword != _savedPassword)
            {
                ErrorTextBlock.Text = "Current password is incorrect!";
                return;
            }
            
            SignUpWindow.SaveCredentials(_newPassword, SignUpWindow.GetCredentials().Username);
            
        }
        catch (Exception ex)
        {
            ErrorTextBlock.Text = $"Error: {ex.Message}";
        }
        OpenDashboard();
}
    
    public void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        Hide();
    }
    

    private Credentials getCredentials()
    {
        var filePath = GetSavedCredentials();
    
        if (!File.Exists(filePath))
            return new Credentials();

        try
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Credentials>(json) ?? new Credentials();
        }
        
        catch
        {
            return new Credentials();  
        }
    }
    
    private string GetCredentialsPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".dashboardanalysis"  // 
        );
    }
    
    private string GetSavedCredentials()
    {
        return Path.Combine(GetCredentialsPath(), ConfigFileName);
    }
    
    
    private void OpenDashboard()
    {
        if (Application.Current?.ApplicationLifetime is not
            Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            return;

        Dispatcher.UIThread.Post(() =>
        {
            desktop.MainWindow = new DashboardWindow();
            this.Close();
        });
    }
 
}

