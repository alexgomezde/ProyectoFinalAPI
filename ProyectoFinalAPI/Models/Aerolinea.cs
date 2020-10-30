using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Aerolinea
    {
        public int AER_CODIGO { get; set; }
        public string AER_RUC { get; set; }
        public string AER_NOMBRE { get; set; }
        public string AER_ESTADO { get; set; }
    }
}