using MiCarpeta.Domain.Entities;

namespace MiCarpeta.Domain
{
    public interface ICiudadanoDomainService
    {
        Response RegistrarCiudadano(Ciudadano ciudadano);

        Response BuscarCiudadano(int tipoDocumento, string identificacion);

        Response ListarCiudadanos();

        Response ActualizarCiudadano(Ciudadano pCiudadano);
    }
}
