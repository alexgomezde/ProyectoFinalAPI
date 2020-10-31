using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;


namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/tipoPago")]
    public class TipoPagoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            TipoPago tipoPago = new TipoPago();

            try
            {
                using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT TPA_CODIGO, TPA_DESCRIPCION
                                                            FROM TIPO_PAGO WHERE TPA_CODIGO = @TPA_CODIGO", sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@TPA_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())

                    {
                        tipoPago.TPA_CODIGO = sqlDataReader.GetInt32(0);
                        tipoPago.TPA_DESCRIPCION = sqlDataReader.GetString(1);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(tipoPago);
        }



        [HttpGet]
        //[Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            List<TipoPago> tipoPagos = new List<TipoPago>();


            try
            {
                using (SqlConnection sqlConnection = new
                 SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT TPA_CODIGO, TPA_DESCRIPCION
                                                            FROM TIPO_PAGO", sqlConnection);

                   // sqlCommand.Parameters.AddWithValue("@TPA_CODIGO");

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        TipoPago tipoPago = new TipoPago()
                        {

                            TPA_CODIGO = sqlDataReader.GetInt32(0),
                            TPA_DESCRIPCION = sqlDataReader.GetString(1),
                        };
                        tipoPagos.Add(tipoPago);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(tipoPagos);
        }






        [HttpPost]
        [Route("Ingresar")]
        public IHttpActionResult Ingresar(TipoPago tipoPago)
        {
            if (tipoPago == null)
                return BadRequest();
            if (RegistrarTipoPago(tipoPago))
                return Ok(tipoPago);
            else
                return InternalServerError();
        }
        private bool RegistrarTipoPago(TipoPago tipoPago)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO TIPO_PAGO (TPA_DESCRIPCION) 
                                                      VALUES (@TPA_DESCRIPCION) ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TPA_DESCRIPCION", tipoPago.TPA_DESCRIPCION);
                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }
            return resultado;
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Put(TipoPago tipoPago)
        {
            if (tipoPago == null)
                return BadRequest();
            if (ActualizarTipoPago(tipoPago))
                return Ok(tipoPago);
            else
                return InternalServerError();
        }
        private bool ActualizarTipoPago(TipoPago tipoPago)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE TIPO_PAGO SET            
                    TPA_DESCRIPCION = @TPA_DESCRIPCION
                    WHERE TPA_CODIGO = @TPA_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TPA_CODIGO", tipoPago.TPA_CODIGO);
                sqlCommand.Parameters.AddWithValue("@TPA_DESCRIPCION", tipoPago.TPA_DESCRIPCION);
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
            if (EliminarReserva(id))
                return Ok(id);
            else
                return InternalServerError();
        }
        private bool EliminarReserva(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE TIPO_PAGO WHERE TPA_CODIGO = @TPA_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TPA_CODIGO", id);

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




