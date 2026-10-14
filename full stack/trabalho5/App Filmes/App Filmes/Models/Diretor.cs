using System.ComponentModel.DataAnnotations;
using FilmesApp.Services;

namespace FilmesApp.Models;

public class Diretor : IEntidade
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(60)]
    public string? Nacionalidade { get; set; }

    public ICollection<Filme> Filmes { get; set; } = new List<Filme>();
}
