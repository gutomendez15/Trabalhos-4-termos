using FilmesApp.Models;

namespace FilmesApp.Services;

public class GeneroService : CrudService<Genero>
{
    protected override void Validar(Genero g)
    {
        if (string.IsNullOrWhiteSpace(g.Nome))
            throw new ServiceException("Informe o nome do gênero.");
        if (g.Nome.Length > 50)
            throw new ServiceException("O nome do gênero deve ter no máximo 50 caracteres.");
    }
}
