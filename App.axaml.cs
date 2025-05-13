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

                if (CredentialsExist())
                {
                    desktop.MainWindow = new LogInWindow();
                }
                else
                {
                    desktop.MainWindow = new SignUpWindow();
                }

                desktop.MainWindow.Show();
            }
        }

        private bool CredentialsExist()
        {
            var path = SignUpWindow.GetCredentialsFilePath();
            return File.Exists(path); 
        }
    }
}