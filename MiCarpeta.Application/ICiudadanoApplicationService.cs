using MiCarpeta.Common;
using MiCarpeta.Domain.Entities;

namespace MiCarpeta.Application
{
    public interface ICiudadanoApplicationService
    {
        ResponseViewModel RegistrarCiudadano(Ciudadano ciudadano);

        ResponseViewModel BuscarCiudadano(int tipoDocumento, string identificacion);

        ResponseViewModel ListarCiudadanos();

        ResponseViewModel ActualizarCiudadano(Ciudadano ciudadano);
    }
}
