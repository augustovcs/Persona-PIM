using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tmds.DBus.Protocol;

namespace AvaloniaApplication1

{
    public partial class LogInWindow : Window
    {
        private string _savedUsername;
        private string _savedPassword;
        
        public LogInWindow()
        {
            InitializeComponent();
            UsernameTextBox = this.FindControl<TextBox>("UsernameTextBox");
            PasswordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            ErrorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock");
            ResultTextBlock = this.FindControl<TextBlock>("ResultTextBlock");
            ConfirmPanel = this.FindControl<Border>("ConfirmPanel");
            
            
            var savedCredentials = GetSavedCredentials();
            if (savedCredentials != null)
            {
                _savedUsername = savedCredentials.Username;
                _savedPassword = savedCredentials.Password;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void OnLogInClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var username = UsernameTextBox.Text;
                var password = PasswordTextBox.Text;
                ClearMessages();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    
                {
                    ErrorTextBlock.Text = "Username and password are required!";
                    return;
                }
                
                var savedCredentials = GetSavedCredentials();
                if (savedCredentials == null)
                {
                    ErrorTextBlock.Text = "Error: Credentials not found!";
                    return;
                }

                if (password != savedCredentials.Password || username != savedCredentials.Username)
                {
                    ErrorTextBlock.Text = "Error: Invalid credentials!";
                    return;
                } 
                
                OpenDashboard();
                
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Error: {ex.Message}";
            }
        }
        
        private void ClearMessages()
        {
            ErrorTextBlock.Text = string.Empty;
            ResultTextBlock.Text = string.Empty;

        }
        
        
        private Credentials GetSavedCredentials()
        {
            var configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".dashboardanalysis", 
                "FirstTimeCredentials.json"
            );

            if (!File.Exists(configPath))
                return null; 

            try
            {
                var json = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<Credentials>(json);
            }
            catch
            {
                return null; 
            }
        }
        
        private class Credentials
        {
            
            public string Password { get; set; }
            public string Username { get; set; }

        }
        
        private void OnChangePasswordClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new ChangePasswordWindow();
                desktop.MainWindow.Show();
            }
        }

        private void OnConfirmYes(object sender, RoutedEventArgs e)
        {
            ConfirmPanel.IsVisible = false; 
            OpenSignUpWindow(); 
        }

        private void OnConfirmNo(object sender, RoutedEventArgs e)
        {
            ConfirmPanel.IsVisible = false; 
        }
        

        private void OpenDashboard()
        {
            if (Application.Current?.ApplicationLifetime is
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new DashboardWindow();
                desktop.MainWindow.Show();
                this.Close();
            }
        }

        private void OpenSignUpWindow()
        {
            if (Application.Current?.ApplicationLifetime is
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new SignUpWindow();
                desktop.MainWindow.Show();
                this.Close();
            }
        }
    }
}
