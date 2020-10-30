using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Avion
    {
        public int AVI_CODIGO { get; set; }
        public int AER_CODIGO { get; set; }
        public string AVI_FABRICANTE { get; set; }
        public string AVI_TIPO { get; set; }
        public int AVI_CAPACIDAD { get; set; }
        public string AVI_ESTADO { get; set; }
    }
}