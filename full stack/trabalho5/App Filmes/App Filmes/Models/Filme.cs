using System.ComponentModel.DataAnnotations;
using FilmesApp.Services;

namespace FilmesApp.Models;

public class Filme : IEntidade
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    public int AnoLancamento { get; set; }

    public int DuracaoMinutos { get; set; }

    public double Nota { get; set; }

    [MaxLength(500)]
    public string? Sinopse { get; set; }

    public int GeneroId { get; set; }
    public Genero? Genero { get; set; }

    public int DiretorId { get; set; }
    public Diretor? Diretor { get; set; }
}
