using System;
using System.Windows;
using System.Windows.Threading;

namespace CybersecurityPOE
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                MessageBox.Show(
                    $"An unexpected error occurred:\n\n{ex?.Message}\n\nThe application will now close.",
                    "GRACE AI — Unhandled Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            };

           
            DispatcherUnhandledException += (_, args) =>
            {
                MessageBox.Show(
                    $"An unexpected UI error occurred:\n\n{args.Exception.Message}",
                    "GRACE AI — UI Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                args.Handled = true;
            };
        }
    }
}
