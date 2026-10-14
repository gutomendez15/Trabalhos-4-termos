using System.Windows;
using System.Windows.Controls;
using FilmesApp.Models;
using FilmesApp.Services;

namespace FilmesApp;

public partial class GenerosWindow : Window
{
    private readonly GeneroService _service = new();
    private int _idAtual;

    public GenerosWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => Carregar();
    }

    private void Carregar()
    {
        try
        {
            GridDados.ItemsSource = _service.Listar();
            Limpar();
        }
        catch (ServiceException ex) { Erro(ex.Message); }
    }

    private void Limpar()
    {
        _idAtual = 0;
        TxtNome.Clear();
        GridDados.SelectedItem = null;
    }

    private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GridDados.SelectedItem is not Genero g) return;
        _idAtual = g.Id;
        TxtNome.Text = g.Nome;
    }

    private void Novo_Click(object sender, RoutedEventArgs e) => Limpar();

    private void Salvar_Click(object sender, RoutedEventArgs e)
    {
        var genero = new Genero { Id = _idAtual, Nome = TxtNome.Text.Trim() };
        try
        {
            if (_idAtual == 0) _service.Adicionar(genero);
            else _service.Atualizar(genero);
            Carregar();
        }
        catch (ServiceException ex) { Erro(ex.Message); }
    }

    private void Excluir_Click(object sender, RoutedEventArgs e)
    {
        if (_idAtual == 0) { Erro("Selecione um gênero para excluir."); return; }

        var r = MessageBox.Show("Deseja realmente excluir este gênero?", "Confirmar",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (r != MessageBoxResult.Yes) return;

        try
        {
            _service.Excluir(_idAtual);
            Carregar();
        }
        catch (ServiceException ex) { Erro(ex.Message); }
    }

    private static void Erro(string msg)
        => MessageBox.Show(msg, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
}
