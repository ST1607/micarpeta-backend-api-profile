using MiCarpeta.Repository;
using MiCarpeta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.IO;
using RestSharp;

namespace MiCarpeta.Domain
{
    public class CiudadanoDomainService : ICiudadanoDomainService
    {
        private readonly IConfiguration Configuration;
        private readonly ICiudadanoRepository CiudadanoRepository;
        private readonly IUsuariosRepository UsuariosRepository;

        public CiudadanoDomainService(ICiudadanoRepository ciudadanoRepository, IConfiguration configuration, IUsuariosRepository usuariosRepository)
        {
            CiudadanoRepository = ciudadanoRepository;
            Configuration = configuration;
            UsuariosRepository = usuariosRepository;
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
                string centralizadorResponse = ValidarCiudadanoCentralizador(ciudadano.Identificacion);

                if (string.IsNullOrEmpty(centralizadorResponse))
                {
                    IRestResponse respuesta = RegistrarCiudadanoCentralizador(ciudadano);

                    if (respuesta.StatusCode.Equals(HttpStatusCode.Created))
                    {
                        //Si no existre en el centralizador procedo con el registro
                        ciudadano.Id = DateTime.UtcNow.Ticks;

                        if (CiudadanoRepository.Add(ciudadano)) 
                        {
                            Usuarios usuario = new Usuarios {
                                IdUsuario = ciudadano.Id,
                                Usuario = ciudadano.Correo,
                                Clave = ciudadano.Clave,
                                IdRol = 1
                            };

                            UsuariosRepository.Add(usuario);


                            return new Response()
                            {
                                Estado = 200,
                                Mensaje = "El ciudadano ha sido registrado exitosamente."
                            };
                        }
                            

                        return new Response()
                        {
                            Estado = 400,
                            Errores = new List<string>()
                        {
                            $"Ha ocurrido un error al registrar el ciudadano. Por favor contacte al administrador del sistema"
                        }
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Estado = 400,
                            Errores = new List<string>()
                            {
                                respuesta.Content
                            }
                        };
                    }
                }
                else
                {
                    return new Response()
                    {
                        Estado = 400,
                        Errores = new List<string>()
                        {
                            $"Ha ocurrido un error al registrar el ciudadano. Por favor contacte al administrador del sistema"
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

        #region Centralizador
        private string ValidarCiudadanoCentralizador(string identificacion)
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
                    if (strReader == null) return string.Empty;
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                        return responseBody;
                    }
                }
            }
            catch (WebException ex)
            {
                Exception excep = new Exception(ex.Message);
                throw excep;
            }
        }

        private IRestResponse RegistrarCiudadanoCentralizador(Ciudadano ciudadano)
        {
            var client = new RestClient($"{Configuration["MiCarpeta:URL"]}apis/registerCitizen");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", $"{{\n  \"id\": {ciudadano.Identificacion},\n  \"name\": \"{ciudadano.Nombres} {ciudadano.Apellidos}\",\n  \"address\": \"{ciudadano.Direccion}\",\n  \"email\": \"{ciudadano.Correo}\",\n  \"operatorId\": {Configuration["MiCarpeta:IdOperador"]},\n  \"operatorName\": \"{Configuration["MiCarpeta:NombreOperador"]}\"\n}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response;
        }
        #endregion

    }
}
