using MiCarpeta.Domain.Entities;
using System.Collections.Generic;

namespace MiCarpeta.Repository
{
    public interface ICiudadanoRepository: IRepository<Ciudadano>
    {
        Ciudadano BuscarCiudadano(int tipoDocumento, string identificacion);

        Ciudadano BuscarCiudadano(long id);

        List<Ciudadano> ListarCiudadanos();
    }
}
