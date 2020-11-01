namespace MiCarpeta.Domain
{
    public interface IUsuariosDomainService
    {
        bool ValidarToken(string token, string idUsuario);
    }
}
