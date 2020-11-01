namespace MiCarpeta.Application
{
    public interface IUsuariosApplicationService
    {
        bool ValidarToken(string token, string usuario);
    }
}
