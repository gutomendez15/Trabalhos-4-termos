namespace FilmesApp.Services;

public class ServiceException : Exception
{
    public ServiceException(string mensagem, Exception? inner = null) : base(mensagem, inner) { }
}
