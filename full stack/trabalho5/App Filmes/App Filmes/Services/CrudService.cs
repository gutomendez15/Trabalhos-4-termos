using FilmesApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmesApp.Services;

public abstract class CrudService<T> where T : class, IEntidade
{
    protected abstract void Validar(T entidade);

    protected virtual IQueryable<T> Consulta(FilmesContext db) => db.Set<T>().AsNoTracking();

    public List<T> Listar() => Executar(() =>
    {
        using var db = new FilmesContext();
        return Consulta(db).OrderBy(e => e.Id).ToList();
    });

    public int Contar() => Executar(() =>
    {
        using var db = new FilmesContext();
        return db.Set<T>().Count();
    });

    public void Adicionar(T entidade) => Executar(() =>
    {
        Validar(entidade);
        using var db = new FilmesContext();
        db.Set<T>().Add(entidade);
        db.SaveChanges();
    });

    public void Atualizar(T entidade) => Executar(() =>
    {
        Validar(entidade);
        using var db = new FilmesContext();
        db.Set<T>().Update(entidade);
        db.SaveChanges();
    });

    public void Excluir(int id) => Executar(() =>
    {
        using var db = new FilmesContext();
        var entidade = db.Set<T>().Find(id)
            ?? throw new ServiceException("Registro não encontrado.");
        db.Set<T>().Remove(entidade);
        db.SaveChanges();
    });

    protected static TResult Executar<TResult>(Func<TResult> acao)
    {
        try { return acao(); }
        catch (ServiceException) { throw; }
        catch (DbUpdateException ex)
        {
            throw new ServiceException(
                "Não foi possível concluir a operação. O registro pode estar em uso por outro cadastro, " +
                "estar duplicado ou já ter sido removido.", ex);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Erro ao acessar o banco de dados: " + ex.Message, ex);
        }
    }

    protected static void Executar(Action acao) => Executar<object?>(() => { acao(); return null; });
}
