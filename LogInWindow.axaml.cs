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
        
        public LogInWindow()
        {
            InitializeComponent();
            
            PasswordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            ErrorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock");
            ResultTextBlock = this.FindControl<TextBlock>("ResultTextBlock");
            ConfirmPanel = this.FindControl<StackPanel>("ConfirmPanel");

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void OnLogInClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var password = PasswordTextBox.Text;
                ClearMessages();

                // Validações básicas
                if (string.IsNullOrEmpty(password))
                {
                    ErrorTextBlock.Text = "Password are required!";
                    return;
                }

                // Obtém a senha salva (ou null se não existir)
                var savedPassword = GetSavedPassword();


                if (password != savedPassword)
                {
                    ErrorTextBlock.Text = "Invalid password!";
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

        private string GetSavedPassword()
        {
            var configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".dashboardanalysis", // Pasta oculta no Linux
                "FirstTimeCredentials.json"
            );

            if (!File.Exists(configPath))
                return null; // Retorna null se o arquivo não existir

            try
            {
                var json = File.ReadAllText(configPath);
                var credentials = JsonSerializer.Deserialize<Credentials>(json);
                return credentials?.Password;
            }
            catch
            {
                return null; 
            }
        }

        private class Credentials
        {
            public string Password { get; set; }
        }


        private void OnChangePasswordClick(object sender, RoutedEventArgs e)
        {
            // Mostra o painel de confirmação
            ConfirmPanel.IsVisible = true;
        }

        private void OnConfirmYes(object sender, RoutedEventArgs e)
        {
            ConfirmPanel.IsVisible = false; // Esconde o painel
            OpenSignUpWindow(); // Abre a tela de cadastro
        }

        private void OnConfirmNo(object sender, RoutedEventArgs e)
        {
            ConfirmPanel.IsVisible = false; // Esconde o painel
        }
        

        private void OpenDashboard()
        {
            // Redireciona para a tela principal
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
            // Redireciona para a tela de cadastro para alterar a senha
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
