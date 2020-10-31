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
    [RoutePrefix("api/tarifa")]
    public class TarifaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Tarifa tarifa = new Tarifa();

            try
            {
                using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT TAR_CODIGO, TAR_CLASE, TAR_PRECIO, TAR_IMPUESTO, TAR_ESTADO
                                                            FROM TARIFA WHERE TAR_CODIGO = @TAR_CODIGO", sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@TAR_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())

                    {
                        tarifa.TAR_CODIGO = sqlDataReader.GetInt32(0);
                        tarifa.TAR_CLASE = sqlDataReader.GetString(1);
                        tarifa.TAR_PRECIO = sqlDataReader.GetDecimal(2);
                        tarifa.TAR_IMPUESTO = sqlDataReader.GetInt32(3);
                        tarifa.TAR_ESTADO = sqlDataReader.GetString(4);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(tarifa);
        }



        [HttpGet]
       // [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            List<Tarifa> tarifas = new List<Tarifa>();


            try
            {
                using (SqlConnection sqlConnection = new
                 SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT TAR_CODIGO, TAR_CLASE, TAR_PRECIO, TAR_IMPUESTO, TAR_ESTADO
                                                            FROM TARIFA", sqlConnection);

                   // sqlCommand.Parameters.AddWithValue("@TAR_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Tarifa tarifa = new Tarifa()
                        {

                            TAR_CODIGO = sqlDataReader.GetInt32(0),
                            TAR_CLASE = sqlDataReader.GetString(1),
                            TAR_PRECIO = sqlDataReader.GetDecimal(2),
                            TAR_IMPUESTO = sqlDataReader.GetInt32(3),
                            TAR_ESTADO = sqlDataReader.GetString(4),
                    };
                        tarifas.Add(tarifa);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(tarifas);
        }






        [HttpPost]
        [Route("Ingresar")]
        public IHttpActionResult Ingresar(Tarifa tarifa)
        {
            if (tarifa == null)
                return BadRequest();
            if (RegistrarTarifa(tarifa))
                return Ok(tarifa);
            else
                return InternalServerError();
        }
        private bool RegistrarTarifa(Tarifa tarifa)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO TARIFA (TAR_CLASE,TAR_PRECIO,TAR_IMPUESTO,TAR_ESTADO) 
                                                      VALUES (@TAR_CLASE,@TAR_PRECIO,@TAR_IMPUESTO,@TAR_ESTADO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TAR_CLASE", tarifa.TAR_CLASE);
                sqlCommand.Parameters.AddWithValue("@TAR_PRECIO", tarifa.TAR_PRECIO);
                sqlCommand.Parameters.AddWithValue("@TAR_IMPUESTO", tarifa.TAR_IMPUESTO);
                sqlCommand.Parameters.AddWithValue("@TAR_ESTADO", tarifa.TAR_ESTADO);
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
        public IHttpActionResult Put(Tarifa tarifa)
        {
            if (tarifa == null)
                return BadRequest();
            if (ActualizarTarifa(tarifa))
                return Ok(tarifa);
            else
                return InternalServerError();
        }
        private bool ActualizarTarifa(Tarifa tarifa)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE TARIFA SET 
                    TAR_CLASE = @TAR_CLASE,  
                    TAR_PRECIO = @TAR_PRECIO, 
                    TAR_IMPUESTO = @TAR_IMPUESTO,
                    TAR_ESTADO = @TAR_ESTADO  
                    WHERE TAR_CODIGO = @TAR_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TAR_CODIGO", tarifa.TAR_CODIGO);
                sqlCommand.Parameters.AddWithValue("@TAR_CLASE", tarifa.TAR_CLASE);
                sqlCommand.Parameters.AddWithValue("@TAR_PRECIO", tarifa.TAR_PRECIO);
                sqlCommand.Parameters.AddWithValue("@TAR_IMPUESTO", tarifa.TAR_IMPUESTO);
                sqlCommand.Parameters.AddWithValue("@TAR_ESTADO", tarifa.TAR_ESTADO);
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
                SqlCommand sqlCommand = new SqlCommand(@"DELETE TARIFA WHERE TAR_CODIGO = @TAR_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TAR_CODIGO", id);

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




