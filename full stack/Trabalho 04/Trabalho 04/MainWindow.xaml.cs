using System.Windows;
using LojaPerfumesWPF.Data;
using LojaPerfumesWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaPerfumesWPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        CarregarPerfumes();
    }

    private void CarregarPerfumes()
    {
        using var db = new AppDbContext();
        TabelaPerfumes.ItemsSource = db.Perfumes.AsNoTracking().ToList();
    }

    private void Adicionar_Click(object sender, RoutedEventArgs e)
    {
        var tela = new PerfumeWindow();
        if (tela.ShowDialog() == true)
        {
            using var db = new AppDbContext();
            db.Perfumes.Add(tela.Perfume);
            db.SaveChanges();
            CarregarPerfumes();
        }
    }

    private void Editar_Click(object sender, RoutedEventArgs e)
    {
        if (TabelaPerfumes.SelectedItem is not Perfume selecionado)
        {
            MessageBox.Show("Selecione um perfume.");
            return;
        }

        var tela = new PerfumeWindow(selecionado);
        if (tela.ShowDialog() == true)
        {
            using var db = new AppDbContext();
            var perfume = db.Perfumes.Find(selecionado.Id);

            if (perfume != null)
            {
                perfume.Nome = tela.Perfume.Nome;
                perfume.Marca = tela.Perfume.Marca;
                perfume.Preco = tela.Perfume.Preco;
                perfume.Estoque = tela.Perfume.Estoque;
                db.SaveChanges();
            }

            CarregarPerfumes();
        }
    }

    private void Excluir_Click(object sender, RoutedEventArgs e)
    {
        if (TabelaPerfumes.SelectedItem is not Perfume selecionado)
        {
            MessageBox.Show("Selecione um perfume.");
            return;
        }

        var resposta = MessageBox.Show(
            $"Deseja excluir o perfume {selecionado.Nome}?",
            "Excluir",
            MessageBoxButton.YesNo);

        if (resposta != MessageBoxResult.Yes)
            return;

        using var db = new AppDbContext();
        var perfume = db.Perfumes.Find(selecionado.Id);

        if (perfume != null)
        {
            db.Perfumes.Remove(perfume);
            db.SaveChanges();
        }

        CarregarPerfumes();
    }
}
