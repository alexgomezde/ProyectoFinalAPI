using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalAPI.Models
{
    public class Hotel
    {
        public int HOT_CODIGO { get; set; }
        public string HOT_NOMBRE { get; set; }
        public string HOT_EMAIL { get; set; }
        public string HOT_DIRECCION { get; set; }
        public string HOT_TELEFONO { get; set; }
        public string HOT_CATEGORIA { get; set; }
    }
}