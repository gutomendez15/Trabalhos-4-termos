using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using FilmesApp.Models;
using FilmesApp.Services;

namespace FilmesApp;

public partial class FilmesWindow : Window
{
    private readonly FilmeService _filmes = new();
    private readonly GeneroService _generos = new();
    private readonly DiretorService _diretores = new();
    private int _idAtual;

    public FilmesWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => Carregar();
    }

    private void Carregar()
    {
        try
        {
            CbGenero.ItemsSource = _generos.Listar();
            CbDiretor.ItemsSource = _diretores.Listar();
            GridDados.ItemsSource = _filmes.Listar();
            Limpar();
        }
        catch (ServiceException ex) { Erro(ex.Message); }
    }

    private void Limpar()
    {
        _idAtual = 0;
        TxtTitulo.Clear(); TxtAno.Clear(); TxtDuracao.Clear();
        TxtNota.Clear(); TxtSinopse.Clear();
        CbGenero.SelectedIndex = -1; CbDiretor.SelectedIndex = -1;
        GridDados.SelectedItem = null;
    }

    private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GridDados.SelectedItem is not Filme f) return;
        _idAtual = f.Id;
        TxtTitulo.Text = f.Titulo;
        TxtAno.Text = f.AnoLancamento.ToString();
        TxtDuracao.Text = f.DuracaoMinutos.ToString();
        TxtNota.Text = f.Nota.ToString(CultureInfo.CurrentCulture);
        TxtSinopse.Text = f.Sinopse;
        CbGenero.SelectedValue = f.GeneroId;
        CbDiretor.SelectedValue = f.DiretorId;
    }

    private void Novo_Click(object sender, RoutedEventArgs e) => Limpar();

    private void Salvar_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtAno.Text, out var ano) ||
            !int.TryParse(TxtDuracao.Text, out var duracao) ||
            !double.TryParse(TxtNota.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out var nota))
        {
            Erro("Ano, duração e nota devem ser números válidos.");
            return;
        }

        var filme = new Filme
        {
            Id = _idAtual,
            Titulo = TxtTitulo.Text.Trim(),
            AnoLancamento = ano,
            DuracaoMinutos = duracao,
            Nota = nota,
            Sinopse = string.IsNullOrWhiteSpace(TxtSinopse.Text) ? null : TxtSinopse.Text.Trim(),
            GeneroId = CbGenero.SelectedValue is int g ? g : 0,
            DiretorId = CbDiretor.SelectedValue is int d ? d : 0
        };

        try
        {
            if (_idAtual == 0) _filmes.Adicionar(filme);
            else _filmes.Atualizar(filme);
            Carregar();
        }
        catch (ServiceException ex) { Erro(ex.Message); }
    }

    private void Excluir_Click(object sender, RoutedEventArgs e)
    {
        if (_idAtual == 0) { Erro("Selecione um filme para excluir."); return; }

        var r = MessageBox.Show("Deseja realmente excluir este filme?", "Confirmar",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (r != MessageBoxResult.Yes) return;

        try
        {
            _filmes.Excluir(_idAtual);
            Carregar();
        }
        catch (ServiceException ex) { Erro(ex.Message); }
    }

    private static void Erro(string msg)
        => MessageBox.Show(msg, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
}
