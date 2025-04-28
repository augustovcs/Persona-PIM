using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;

namespace AvaloniaApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()  // Detecta a plataforma automaticamente (Windows, Linux, etc.)
                .LogToTrace();  // Registra logs (opcional)
        }
    }
}