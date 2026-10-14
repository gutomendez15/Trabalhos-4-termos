using System.ComponentModel.DataAnnotations;
using FilmesApp.Services;

namespace FilmesApp.Models;

public class Genero : IEntidade
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Nome { get; set; } = string.Empty;

    public ICollection<Filme> Filmes { get; set; } = new List<Filme>();
}
