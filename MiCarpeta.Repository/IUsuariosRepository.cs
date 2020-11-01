using MiCarpeta.Domain.Entities;

namespace MiCarpeta.Repository
{
    public interface IUsuariosRepository : IRepository<Usuarios>
    {
        Usuarios ValidarToken(string token, string idUsuario);
    }
}
