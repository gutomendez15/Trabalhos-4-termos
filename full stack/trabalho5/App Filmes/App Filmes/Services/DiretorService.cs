using FilmesApp.Models;

namespace FilmesApp.Services;

public class DiretorService : CrudService<Diretor>
{
    protected override void Validar(Diretor d)
    {
        if (string.IsNullOrWhiteSpace(d.Nome))
            throw new ServiceException("Informe o nome do diretor.");
        if (d.Nome.Length > 100)
            throw new ServiceException("O nome do diretor deve ter no máximo 100 caracteres.");
    }
}
