using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalAPI.Models
{
    public class Habitacion
    {
        public int HAB_CODIGO { get; set; }
        public int HOT_CODIGO { get; set; }
        public int HAB_NUMERO { get; set; }
        public int HAB_CAPACIDAD { get; set; }
        public string HAB_TIPO { get; set; }
        public string HAB_DESCRIPCION { get; set; }
        public string HAB_ESTADO { get; set; }
        public decimal HAB_PRECIO { get; set; }
    }
}