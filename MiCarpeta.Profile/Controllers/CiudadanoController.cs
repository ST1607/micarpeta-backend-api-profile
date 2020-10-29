using MiCarpeta.Application;
using MiCarpeta.Common;
using MiCarpeta.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MiCarpeta.Controllers
{
    [ApiController]
    [Route("api/ciudadanos")]
    public class CiudadanoController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ICiudadanoApplicationService CiudadanoApplicationService;

        public CiudadanoController(IConfiguration config, ICiudadanoApplicationService ciudadanoApplicationService)
        {
            Configuration = config;
            CiudadanoApplicationService = ciudadanoApplicationService;
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
        
        [HttpPut("actualizar")]
        public IActionResult ActualizarCiudadano([FromBody] Ciudadano ciudadano)
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

                ResponseViewModel response = CiudadanoApplicationService.ActualizarCiudadano(ciudadano);

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

        [HttpGet("buscarCiudadano")]
        public IActionResult BuscarCiudadano(int tipoDocumento, string identificacion)
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

                ResponseViewModel response = CiudadanoApplicationService.BuscarCiudadano(tipoDocumento, identificacion);

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

        [HttpGet("listarCiudadanos")]
        public IActionResult ListarCiudadanos()
        {
            try
            {
                ResponseViewModel response = CiudadanoApplicationService.ListarCiudadanos();

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

    }
}
