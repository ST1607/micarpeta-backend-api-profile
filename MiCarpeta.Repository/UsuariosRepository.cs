using MiCarpeta.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace MiCarpeta.Repository
{
    public class UsuariosRepository : Repository<Usuarios>, IUsuariosRepository
    {
        public UsuariosRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
