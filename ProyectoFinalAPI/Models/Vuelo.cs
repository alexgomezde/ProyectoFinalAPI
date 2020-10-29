using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalAPI.Models
{
    public class Vuelo
    {
        public int VUE_CODIGO { get; set; }
        public int VUE_CODIGO_ASI { get; set; }
        public int AER_ORIGEN_COD { get; set; }
        public int AER_DESTINO_COD { get; set; }
        public int AVI_CODIGO { get; set; }
        public int TAR_CODIGO { get; set; }
        public string VUE_ESTADO { get; set; }
    }
}