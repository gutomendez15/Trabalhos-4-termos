using FilmesApp.Data;
using FilmesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApp.Services;

public class EstatisticaGenero
{
    public string Genero { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public double NotaMedia { get; set; }
}

public class FilmeService : CrudService<Filme>
{
    protected override IQueryable<Filme> Consulta(FilmesContext db)
        => db.Filmes.AsNoTracking().Include(f => f.Genero).Include(f => f.Diretor);

    protected override void Validar(Filme f)
    {
        if (string.IsNullOrWhiteSpace(f.Titulo))
            throw new ServiceException("Informe o título do filme.");
        if (f.AnoLancamento < 1888 || f.AnoLancamento > DateTime.Now.Year + 5)
            throw new ServiceException("Ano de lançamento inválido.");
        if (f.DuracaoMinutos <= 0)
            throw new ServiceException("A duração deve ser maior que zero.");
        if (f.Nota < 0 || f.Nota > 10)
            throw new ServiceException("A nota deve estar entre 0 e 10.");
        if (f.GeneroId <= 0)
            throw new ServiceException("Selecione um gênero.");
        if (f.DiretorId <= 0)
            throw new ServiceException("Selecione um diretor.");
    }

    public List<Filme> Buscar(string? titulo, int? generoId, int? diretorId,
                              int? anoInicial, double? notaMinima) => Executar(() =>
    {
        using var db = new FilmesContext();
        IQueryable<Filme> q = Consulta(db);

        if (!string.IsNullOrWhiteSpace(titulo))
        {
            var termo = $"%{titulo.Trim()}%";
            q = q.Where(f => EF.Functions.Like(f.Titulo, termo));
        }
        if (generoId is > 0)     q = q.Where(f => f.GeneroId == generoId);
        if (diretorId is > 0)    q = q.Where(f => f.DiretorId == diretorId);
        if (anoInicial.HasValue) q = q.Where(f => f.AnoLancamento >= anoInicial);
        if (notaMinima.HasValue) q = q.Where(f => f.Nota >= notaMinima);

        return q.OrderByDescending(f => f.Nota).ThenBy(f => f.Titulo).ToList();
    });

    public List<EstatisticaGenero> EstatisticasPorGenero() => Executar(() =>
    {
        using var db = new FilmesContext();
        return db.Filmes
            .GroupBy(f => f.Genero!.Nome)
            .Select(g => new EstatisticaGenero
            {
                Genero = g.Key,
                Quantidade = g.Count(),
                NotaMedia = g.Average(f => f.Nota)
            })
            .OrderByDescending(e => e.Quantidade)
            .ToList();
    });

    public double NotaMedia() => Executar(() =>
    {
        using var db = new FilmesContext();
        return db.Filmes.Any() ? db.Filmes.Average(f => f.Nota) : 0.0;
    });
}
