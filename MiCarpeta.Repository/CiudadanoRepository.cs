using MiCarpeta.Common;
using MiCarpeta.Domain.Entities;
using MiCarpeta.Repository.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MiCarpeta.Repository
{
    public class CiudadanoRepository : Repository<Ciudadano>, ICiudadanoRepository
    {
        public CiudadanoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Ciudadano BuscarCiudadano(int tipoDocumento, string identificacion)
        {
            List<FilterQuery> filtros = new List<FilterQuery>()
            {
                new FilterQuery {
                    AtributeName = "Identificacion",
                    Operator = (int)Enumerators.QueryScanOperator.Equal,
                    ValueAtribute = identificacion
                },
                new FilterQuery {
                    AtributeName = "TipoDocumento",
                    Operator = (int)Enumerators.QueryScanOperator.Equal,
                    ValueAtribute = tipoDocumento
                }
            };

            Ciudadano ciudadano = GetByList(filtros).FirstOrDefault();

            return ciudadano;
        }

        public Ciudadano BuscarCiudadano(long id)
        {
            List<FilterQuery> filtros = new List<FilterQuery>()
            {
                new FilterQuery {
                    AtributeName = "Id",
                    Operator = (int)Enumerators.QueryScanOperator.Equal,
                    ValueAtribute = id
                }
            };

            Ciudadano ciudadano = GetByList(filtros).FirstOrDefault();

            return ciudadano;
        }

        public List<Ciudadano> ListarCiudadanos()
        {
            List<Ciudadano> ciudadano = GetByList(new List<FilterQuery>());

            return ciudadano;
        }
    }
}
