using System.Collections.Generic;

namespace MiCarpeta.Common
{
    public class ResponseViewModel
    {
        public string Mensaje { get; set; }
        public int Estado { get; set; }
        public List<string> Errores { get; set; }
        public object Data { get; set; }
    }
}
