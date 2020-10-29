using ProyectoFinalAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProyectoFinalAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/factura")]
    public class FacturaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Factura factura = new Factura();

            try
            {
                using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT FAC_CODIGO, PAG_CODIGO, FAC_COMPROBANTE, FAC_ESTADO
                    FROM FACTURA WHERE
                    FAC_CODIGO = @FAC_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@FAC_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        factura.FAC_CODIGO = sqlDataReader.GetInt32(0);
                        factura.PAG_CODIGO = sqlDataReader.GetInt32(1);
                        factura.FAC_COMPROBANTE = sqlDataReader.GetString(2);
                        factura.FAC_ESTADO = sqlDataReader.GetString(3);

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(factura);
        }


        [HttpGet]
        public IHttpActionResult GetAll(int id)
        {
            List<Factura> facturas = new List<Factura>();

            try
            {
                using (SqlConnection sqlConnection = new
                 SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT FAC_CODIGO, PAG_CODIGO, FAC_COMPROBANTE, FAC_ESTADO 
                    FROM FACTURA WHERE
                    FAC_CODIGO = @FAC_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@FAC_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Factura factura = new Factura()
                        {
                            FAC_CODIGO = sqlDataReader.GetInt32(0),
                            PAG_CODIGO = sqlDataReader.GetInt32(1),
                            FAC_COMPROBANTE = sqlDataReader.GetString(2),
                            FAC_ESTADO = sqlDataReader.GetString(3),

                        };
                        facturas.Add(factura);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(facturas);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Factura factura)
        {
            if (factura == null)
                return BadRequest();
            if (RegistrarFactura(factura))
                return Ok(factura);
            else
                return InternalServerError();
        }
        private bool RegistrarFactura(Factura factura)
        {
            bool resultado = false;



            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO FACTURA (PAG_CODIGO, FAC_COMPROBANTE, FAC_ESTADO)
                        VALUES (@PAG_CODIGO, @FAC_COMPROBANTE, @FAC_ESTADO) ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@PAG_CODIGO", factura.PAG_CODIGO);
                sqlCommand.Parameters.AddWithValue("@FAC_COMPROBANTE", factura.FAC_COMPROBANTE);
                sqlCommand.Parameters.AddWithValue("@FAC_ESTADO", factura.FAC_ESTADO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }
            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Factura factura)
        {
            if (factura == null)
                return BadRequest();
            if (ActualizarFactura(factura))
                return Ok(factura);
            else
                return InternalServerError();
        }
        private bool ActualizarFactura(Factura factura)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE FACTURA SET
                PAG_CODIGO = @PAG_CODIGO,
                FAC_COMPROBANTE = @FAC_COMPROBANTE,
                FAC_ESTADO = @FAC_ESTADO
                WHERE FAC_CODIGO = @FAC_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FAC_CODIGO", factura.FAC_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAG_CODIGO", factura.PAG_CODIGO);
                sqlCommand.Parameters.AddWithValue("@FAC_COMPROBANTE", factura.FAC_COMPROBANTE);
                sqlCommand.Parameters.AddWithValue("@FAC_ESTADO", factura.FAC_ESTADO);


                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }
            return resultado;


        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (EliminarFactura(id))
                return Ok(id);
            else
                return InternalServerError();
        }
        private bool EliminarFactura(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE FACTURA WHERE FAC_CODIGO = @FAC_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@FAC_CODIGO", id);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }
            return resultado;
        }
    }
}
