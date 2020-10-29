using MiCarpeta.Repository;
using MiCarpeta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.IO;

namespace MiCarpeta.Domain
{
    public class CiudadanoDomainService : ICiudadanoDomainService
    {
        private readonly IConfiguration Configuration;
        private readonly ICiudadanoRepository CiudadanoRepository;

        public CiudadanoDomainService(ICiudadanoRepository ciudadanoRepository, IConfiguration configuration)
        {
            CiudadanoRepository = ciudadanoRepository;
            Configuration = configuration;
        }

        public Response RegistrarCiudadano(Ciudadano ciudadano)
        {
            try
            {
                // Validar ante mi BD
                if (CiudadanoRepository.BuscarCiudadano(ciudadano.TipoDocumento, ciudadano.Identificacion) != null)
                    return new Response()
                    {
                        Estado = 201,
                        Errores = new List<string>() {
                            $"El ciudadano con número de identificación {ciudadano.Identificacion} ya se encuentra registrado."
                        }
                    };

                // Validar ante el centralizador
                ValidarCiudadano(ciudadano.Identificacion);

                //Si no existre en el centralizador procedo con el registro
                ciudadano.Id = DateTime.UtcNow.Ticks;

                if (CiudadanoRepository.Add(ciudadano))
                    return new Response()
                    {
                        Estado = 200,
                        Mensaje = "El ciudadano ha sido registrado exitosamente."
                    };

                return new Response()
                {
                    Estado = 400,
                    Errores = new List<string>()
                    {
                        $"Ha ocurrido un error al registrar el ciudadano. Por favor contacte al administrador del sistema"
                    }
                };
            }
            catch (Exception ex)
            {
                Exception excep = new Exception(ex.Message);
                throw excep;
            }
        }

        public Response BuscarCiudadano(int tipoDocumento, string identificacion)
        {
            Ciudadano ciudadano = CiudadanoRepository.BuscarCiudadano(tipoDocumento, identificacion);

            if (ciudadano != null)
                return new Response
                {
                    Data = ciudadano,
                    Estado = 200
                };
            else
                return new Response
                {
                    Estado = 201,
                    Errores = new List<string>()
                    {
                        "El usuario no se encuentra registrado."
                    }
                };
        }

        public Response ListarCiudadanos()
        {
            List<Ciudadano> ciudadanos = CiudadanoRepository.ListarCiudadanos();

            if (ciudadanos.Any())
                return new Response
                {
                    Data = ciudadanos,
                    Estado = 200
                };
            else
                return new Response
                {
                    Estado = 201,
                    Errores = new List<string>()
                    {
                        "No hay usuarios registrados."
                    }
                };
        }

        public Response ActualizarCiudadano(Ciudadano pCiudadano)
        {
            try
            {
                Ciudadano ciudadano = CiudadanoRepository.BuscarCiudadano(pCiudadano.Id);

                if (ciudadano != null)
                {
                    ciudadano.Nombres = pCiudadano.Nombres;
                    ciudadano.Apellidos = pCiudadano.Apellidos;
                    ciudadano.Telefono = pCiudadano.Telefono;
                    ciudadano.IdOperador = pCiudadano.IdOperador;

                    if (CiudadanoRepository.Update(ciudadano))
                    {
                        return new Response()
                        {
                            Estado = 200,
                            Mensaje = "El ciudadano ha sido actualizado exitosamente."
                        };
                    }

                    return new Response()
                    {
                        Estado = 400,
                        Errores = new List<string>()
                        {
                            "Se ha presentado un error al tratar de actualizar los datos del ciudadano."
                        }
                    };

                }
                else
                {
                    return new Response()
                    {
                        Estado = 201,
                        Errores = new List<string>()
                        {
                            "No existe un ciudadano con ese ID."
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Exception excep = new Exception(ex.Message);
                throw excep;
            }
        }

        private void ValidarCiudadano(string identificacion)
        {
            string url = $"{Configuration["MiCarpeta:URL"]}apis/validateCitizen/{identificacion}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = Configuration["MiCarpeta:ContentType"];
            request.Accept = Configuration["MiCarpeta:ContentType"];

            try
            {
                string responseBody = string.Empty;

                using (WebResponse response = request.GetResponse())
                {
                    Stream strReader = response.GetResponseStream();
                    if (strReader == null) return;
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                Exception excep = new Exception(ex.Message);
                throw excep;
            }
        }
    }
}
