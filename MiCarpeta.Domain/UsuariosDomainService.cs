using MiCarpeta.Domain.Entities;
using MiCarpeta.Repository;

namespace MiCarpeta.Domain
{
    public class UsuariosDomainService: IUsuariosDomainService
    {
        private readonly IUsuariosRepository UsuariosRepository;

        public UsuariosDomainService(IUsuariosRepository usuariosRepository)
        {
            UsuariosRepository = usuariosRepository;
        }

        public bool ValidarToken(string token, string idUsuario)
        {
            Usuarios usuarioDB = UsuariosRepository.ValidarToken(token, idUsuario);

            return usuarioDB != null;
        }
    }
}
