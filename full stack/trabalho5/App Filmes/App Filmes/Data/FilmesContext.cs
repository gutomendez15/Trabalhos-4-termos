using FilmesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApp.Data;

public class FilmesContext : DbContext
{
    public DbSet<Genero> Generos => Set<Genero>();
    public DbSet<Diretor> Diretores => Set<Diretor>();
    public DbSet<Filme> Filmes => Set<Filme>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=filmes.db");

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Filme>()
            .HasOne(f => f.Genero).WithMany(g => g.Filmes)
            .HasForeignKey(f => f.GeneroId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Filme>()
            .HasOne(f => f.Diretor).WithMany(d => d.Filmes)
            .HasForeignKey(f => f.DiretorId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Genero>().HasIndex(g => g.Nome).IsUnique();

        mb.Entity<Genero>().HasData(
            new Genero { Id = 1, Nome = "Ação" },
            new Genero { Id = 2, Nome = "Drama" },
            new Genero { Id = 3, Nome = "Ficção Científica" },
            new Genero { Id = 4, Nome = "Comédia" },
            new Genero { Id = 5, Nome = "Terror" });

        mb.Entity<Diretor>().HasData(
            new Diretor { Id = 1, Nome = "Christopher Nolan", Nacionalidade = "Reino Unido" },
            new Diretor { Id = 2, Nome = "Greta Gerwig", Nacionalidade = "Estados Unidos" },
            new Diretor { Id = 3, Nome = "Fernando Meirelles", Nacionalidade = "Brasil" },
            new Diretor { Id = 4, Nome = "Denis Villeneuve", Nacionalidade = "Canadá" });

        mb.Entity<Filme>().HasData(
            new Filme { Id = 1, Titulo = "Interestelar", AnoLancamento = 2014, DuracaoMinutos = 169, Nota = 8.7, GeneroId = 3, DiretorId = 1, Sinopse = "Exploradores viajam por um buraco de minhoca para salvar a humanidade." },
            new Filme { Id = 2, Titulo = "A Origem", AnoLancamento = 2010, DuracaoMinutos = 148, Nota = 8.8, GeneroId = 3, DiretorId = 1, Sinopse = "Ladrões invadem sonhos para plantar uma ideia." },
            new Filme { Id = 3, Titulo = "Barbie", AnoLancamento = 2023, DuracaoMinutos = 114, Nota = 6.8, GeneroId = 4, DiretorId = 2, Sinopse = "Barbie sai da Barbieland e descobre o mundo real." },
            new Filme { Id = 4, Titulo = "Cidade de Deus", AnoLancamento = 2002, DuracaoMinutos = 130, Nota = 8.6, GeneroId = 2, DiretorId = 3, Sinopse = "A vida de jovens em uma comunidade do Rio de Janeiro." },
            new Filme { Id = 5, Titulo = "Duna", AnoLancamento = 2021, DuracaoMinutos = 155, Nota = 8.0, GeneroId = 3, DiretorId = 4, Sinopse = "Paul Atreides assume seu destino no planeta Arrakis." });
    }
}
