using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.IO;

namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();  // Certifique-se de chamar a base apenas uma vez

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;

                // Verifica se as credenciais existem
                if (CredentialsExist())
                {
                    // Se as credenciais existirem, abre o LogInWindow
                    desktop.MainWindow = new LogInWindow();
                }
                else
                {
                    // Se as credenciais não existirem, abre o SignUpWindow
                    desktop.MainWindow = new SignUpWindow();
                }

                desktop.MainWindow.Show();
            }
        }

        private bool CredentialsExist()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "dashboardanalysis.json",  // O caminho onde você armazenou o arquivo de credenciais
                "FirstTimeCredentials.json"
            );

            return File.Exists(path);  // Retorna true se o arquivo de credenciais existir
        }
    }
}