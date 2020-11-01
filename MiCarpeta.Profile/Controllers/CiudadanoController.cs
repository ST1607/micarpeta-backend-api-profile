using MiCarpeta.Application;
using MiCarpeta.Common;
using MiCarpeta.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiCarpeta.Controllers
{
    [ApiController]
    [Route("api/ciudadanos")]
    public class CiudadanoController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ICiudadanoApplicationService CiudadanoApplicationService;
        private readonly IUsuariosApplicationService UsuariosApplicationService;

        public CiudadanoController(IConfiguration config, ICiudadanoApplicationService ciudadanoApplicationService,
            IUsuariosApplicationService usuariosApplicationService)
        {
            Configuration = config;
            CiudadanoApplicationService = ciudadanoApplicationService;
            UsuariosApplicationService = usuariosApplicationService;
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarCiudadano([FromBody] Ciudadano ciudadano)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseViewModel
                    {
                        Estado = 400,
                        Errores = new List<string>() { "Todos los campos son obligatorios" }
                    });
                }

                ResponseViewModel response = CiudadanoApplicationService.RegistrarCiudadano(ciudadano);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel
                {
                    Estado = 400,
                    Errores = new List<string>() { ex.Message }
                });
            }
        }

        [Authorize(Roles = "Ciudadano")]
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCiudadanoAsync([FromBody] Ciudadano ciudadano)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseViewModel
                    {
                        Estado = 400,
                        Errores = new List<string>() { "Todos los campos son obligatorios" }
                    });
                }

                string token = await HttpContext.GetTokenAsync("access_token");

                Claim claimIdUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("IdUsuario", StringComparison.InvariantCultureIgnoreCase));

                if (claimIdUsuario != null)
                {
                    string idUsuario = claimIdUsuario.Value;

                    if (UsuariosApplicationService.ValidarToken(token, idUsuario))
                    {
                        ResponseViewModel response = CiudadanoApplicationService.ActualizarCiudadano(ciudadano);

                        return Ok(response);
                    }
                }

                return BadRequest(new ResponseViewModel
                {
                    Estado = 401,
                    Errores = new List<string>() { "Unauthorized" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel
                {
                    Estado = 400,
                    Errores = new List<string>() { ex.Message }
                });
            }
        }

        [Authorize(Roles = "Operador")]
        [HttpGet("buscarCiudadano")]
        public async Task<IActionResult> BuscarCiudadanoAsync(int tipoDocumento, string identificacion)
        {
            try
            {
                if (string.IsNullOrEmpty(identificacion))
                {
                    return BadRequest(new ResponseViewModel
                    {
                        Estado = 400,
                        Errores = new List<string>() { "Todos los campos son obligatorios" }
                    });
                }

                string token = await HttpContext.GetTokenAsync("access_token");

                Claim claimIdUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("IdUsuario", StringComparison.InvariantCultureIgnoreCase));

                if (claimIdUsuario != null)
                {
                    string idUsuario = claimIdUsuario.Value;

                    if (UsuariosApplicationService.ValidarToken(token, idUsuario))
                    {
                        ResponseViewModel response = CiudadanoApplicationService.BuscarCiudadano(tipoDocumento, identificacion);

                        return Ok(response);
                    }
                }

                return BadRequest(new ResponseViewModel
                {
                    Estado = 401,
                    Errores = new List<string>() { "Unauthorized" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel
                {
                    Estado = 400,
                    Errores = new List<string>() { ex.Message }
                });
            }
        }

        [Authorize(Roles = "Operador")]
        [HttpGet("listarCiudadanos")]
        public async Task<IActionResult> ListarCiudadanos()
        {
            try
            {
                string token = await HttpContext.GetTokenAsync("access_token");

                Claim claimIdUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("IdUsuario", StringComparison.InvariantCultureIgnoreCase));

                if (claimIdUsuario != null)
                {
                    string idUsuario = claimIdUsuario.Value;

                    if (UsuariosApplicationService.ValidarToken(token, idUsuario))
                    {
                        ResponseViewModel response = CiudadanoApplicationService.ListarCiudadanos();

                        return Ok(response);
                    }
                }

                return BadRequest(new ResponseViewModel
                {
                    Estado = 401,
                    Errores = new List<string>() { "Unauthorized" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel
                {
                    Estado = 400,
                    Errores = new List<string>() { ex.Message }
                });
            }
        }

    }
}
