using System.Windows;
using FilmesApp.Services;

namespace FilmesApp;

public partial class MainWindow : Window
{
    private readonly FilmeService _filmes = new();
    private readonly GeneroService _generos = new();
    private readonly DiretorService _diretores = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => AtualizarDashboard();
    }

    private void AtualizarDashboard()
    {
        try
        {
            TxtFilmes.Text = _filmes.Contar().ToString();
            TxtGeneros.Text = _generos.Contar().ToString();
            TxtDiretores.Text = _diretores.Contar().ToString();
            TxtNota.Text = _filmes.NotaMedia().ToString("0.0");
        }
        catch (ServiceException ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Abrir(Window janela)
    {
        janela.Owner = this;
        janela.ShowDialog();
        AtualizarDashboard();
    }

    private void Filmes_Click(object sender, RoutedEventArgs e) => Abrir(new FilmesWindow());
    private void Generos_Click(object sender, RoutedEventArgs e) => Abrir(new GenerosWindow());
    private void Diretores_Click(object sender, RoutedEventArgs e) => Abrir(new DiretoresWindow());
    private void Pesquisa_Click(object sender, RoutedEventArgs e) => Abrir(new PesquisaWindow());
}
