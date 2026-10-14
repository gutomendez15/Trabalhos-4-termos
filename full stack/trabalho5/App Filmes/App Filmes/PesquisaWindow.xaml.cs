using System.Globalization;
using System.Windows;
using FilmesApp.Models;
using FilmesApp.Services;

namespace FilmesApp;

public partial class PesquisaWindow : Window
{
    private readonly FilmeService _filmes = new();
    private readonly GeneroService _generos = new();
    private readonly DiretorService _diretores = new();

    public PesquisaWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => Iniciar();
    }

    private void Iniciar()
    {
        try
        {
            var generos = _generos.Listar();
            generos.Insert(0, new Genero { Id = 0, Nome = "Todos" });
            CbGenero.ItemsSource = generos;

            var diretores = _diretores.Listar();
            diretores.Insert(0, new Diretor { Id = 0, Nome = "Todos" });
            CbDiretor.ItemsSource = diretores;

            Limpar_Click(this, new RoutedEventArgs());
        }
        catch (ServiceException ex) { Aviso(ex.Message); }
    }

    private void Pesquisar_Click(object sender, RoutedEventArgs e)
    {
        int? ano = null;
        double? nota = null;

        if (!string.IsNullOrWhiteSpace(TxtAno.Text))
        {
            if (!int.TryParse(TxtAno.Text, out var a)) { Aviso("Ano inválido."); return; }
            ano = a;
        }
        if (!string.IsNullOrWhiteSpace(TxtNota.Text))
        {
            if (!double.TryParse(TxtNota.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out var n))
            { Aviso("Nota inválida."); return; }
            nota = n;
        }

        try
        {
            var resultado = _filmes.Buscar(
                TxtTitulo.Text,
                CbGenero.SelectedValue as int?,
                CbDiretor.SelectedValue as int?,
                ano, nota);

            GridResultado.ItemsSource = resultado;

            TxtResumo.Text = resultado.Count == 0
                ? "Nenhum filme encontrado."
                : $"{resultado.Count} filme(s) • nota média {resultado.Average(f => f.Nota):0.0} • " +
                  $"mais longo: {resultado.OrderByDescending(f => f.DuracaoMinutos).First().Titulo}";

            GridEstatisticas.ItemsSource = _filmes.EstatisticasPorGenero();
        }
        catch (ServiceException ex) { Aviso(ex.Message); }
    }

    private void Limpar_Click(object sender, RoutedEventArgs e)
    {
        TxtTitulo.Clear(); TxtAno.Clear(); TxtNota.Clear();
        CbGenero.SelectedIndex = 0;
        CbDiretor.SelectedIndex = 0;
        Pesquisar_Click(sender, e);
    }

    private static void Aviso(string msg)
        => MessageBox.Show(msg, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
}
