using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalAPI.Models
{
    public class Asiento
    {
        public int ASI_CODIGO { get; set; }
        public string ASI_LETRA { get; set; }
        public int ASI_FILA { get; set; }
        public string ASI_ESTADO { get; set; }
    }
}