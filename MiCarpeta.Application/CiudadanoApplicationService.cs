using AutoMapper;
using MiCarpeta.Common;
using MiCarpeta.Domain;
using MiCarpeta.Domain.Entities;

namespace MiCarpeta.Application
{
    public class CiudadanoApplicationService: ICiudadanoApplicationService
    {
        public readonly ICiudadanoDomainService CiudadanoDomainService;
        private readonly IMapper Mapper;

        public CiudadanoApplicationService(ICiudadanoDomainService ciudadanoDomainService, IMapper mapper)
        {
            CiudadanoDomainService = ciudadanoDomainService;
            Mapper = mapper;
        }
        public ResponseViewModel RegistrarCiudadano(Ciudadano ciudadano)
        {
            Response response = CiudadanoDomainService.RegistrarCiudadano(ciudadano);

            return Mapper.Map<ResponseViewModel>(response);
        }

        public ResponseViewModel BuscarCiudadano(int tipoDocumento, string identificacion)
        {
            Response response = CiudadanoDomainService.BuscarCiudadano(tipoDocumento, identificacion);

            return Mapper.Map<ResponseViewModel>(response);
        }

        public ResponseViewModel ListarCiudadanos()
        {
            Response response = CiudadanoDomainService.ListarCiudadanos();

            return Mapper.Map<ResponseViewModel>(response);
        }

        public ResponseViewModel ActualizarCiudadano(Ciudadano ciudadano)
        {
            Response response = CiudadanoDomainService.ActualizarCiudadano(ciudadano);

            return Mapper.Map<ResponseViewModel>(response);
        }
    }
}
