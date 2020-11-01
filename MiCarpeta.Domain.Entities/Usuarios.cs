using System;

namespace MiCarpeta.Domain.Entities
{
    public class Usuarios
    {
        public long IdUsuario { get; set; }

        public string Usuario { get; set; }

        public string Clave { get; set; }

        public int IdRol { get; set; }

        public string Token { get; set; }

        public DateTime VencimientoToken { get; set; }
    }
}
