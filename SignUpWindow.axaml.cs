using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System.Text.Json;

namespace AvaloniaApplication1;

public partial class SignUpWindow : Window
{
    private const string ConfigFileName = "FirstTimeCredentials.json";

    public SignUpWindow()
    {
        InitializeComponent();
    }

    private void OnSubmitClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var password = PasswordTextBox.Text ?? string.Empty;
            ClearMessages();

            if (!ValidatePassword(password, out var cleanPassword))
                return;

            _saveCredentials(cleanPassword);
            _showDashboard();

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

    private bool ValidatePassword(string password, out string cleanPassword)
    {

        cleanPassword = new string(password.Where(c => char.IsDigit(c)).ToArray());

        if (string.IsNullOrEmpty(password))
        {
            ResultTextBlock.Text = "Password cannot be empty";
            return false;
        }

        if (cleanPassword.Length < 4)
        {
            ErrorTextBlock.Text = "Password must be at least 4 characters";
            return false;
        }

        return true;
    }

    private void _saveCredentials(string password)
    {
        try
        {
            var configPath = GetConfigPath();
            Directory.CreateDirectory(configPath);  // Cria o diretório se não existir

            var credentials = new Credentials { Password = password };
            var jsonString = JsonSerializer.Serialize(credentials);

            // Escreve em um arquivo temporário primeiro (evita corrupção)
            var tempFilePath = Path.Combine(configPath, "temp_" + ConfigFileName);
            File.WriteAllText(tempFilePath, jsonString);

            // Renomeia para o arquivo final (operação atômica)
            var finalFilePath = GetCredentialsFilePath();
            File.Move(tempFilePath, finalFilePath, overwrite: true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving credentials", ex);
        }
    }

    private string GetPassword()
    {
        var filePath = GetCredentialsFilePath();
    
        if (!File.Exists(filePath))
            return string.Empty;  // Retorna vazio se o arquivo não existir

        try
        {
            var json = File.ReadAllText(filePath);
            var credentials = JsonSerializer.Deserialize<Credentials>(json);
            return credentials?.Password ?? string.Empty;
        }
        catch
        {
            return string.Empty;  // Se o arquivo estiver corrompido
        }
    }

    private string GetConfigPath()
    {
        // Retorna o caminho do DIRETÓRIO (não do arquivo)
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".dashboardanalysis"  // 
        );
    }

    private string GetCredentialsFilePath()
    {
        // Retorna o caminho COMPLETO do arquivo JSON
        return Path.Combine(GetConfigPath(), ConfigFileName);
    }

    private void _showDashboard()
    {
        if (Application.Current?.ApplicationLifetime is not
            IClassicDesktopStyleApplicationLifetime desktop)
            return;

        Dispatcher.UIThread.Post(() =>
        {
            desktop.MainWindow = new DashboardWindow();
            desktop.MainWindow.Show();
            Close();
        });
    }


    private class Credentials
    {
        public string Password { get; set; }
    }
}


    

    
    
    

    
    
