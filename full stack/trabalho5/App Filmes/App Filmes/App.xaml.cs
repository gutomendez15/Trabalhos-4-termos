using System.Windows;
using System.Windows.Threading;
using FilmesApp.Data;

namespace FilmesApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        DispatcherUnhandledException += OnErroNaoTratado;

        using (var db = new FilmesContext())
        {
            db.Database.EnsureCreated();
        }

        base.OnStartup(e);
    }

    private static void OnErroNaoTratado(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show("Erro inesperado: " + e.Exception.Message, "Erro",
            MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }
}
