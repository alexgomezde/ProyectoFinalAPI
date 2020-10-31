using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    
    [RoutePrefix("api/Pago")]

    public class PagoController : ApiController
    {

        [HttpGet]

        public IHttpActionResult getId(int id)
        {
            Pago pago = new Pago();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT *
                                                    FROM  PAGO where PAG_CODIGO = @PAG_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@PAG_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        pago.PAG_CODIGO = sqlDataReader.GetInt32(0);
                        pago.RES_CODIGO = sqlDataReader.GetInt32(1);
                        pago.PAG_FECHA = sqlDataReader.GetDateTime(2);
                        pago.PAG_TIPO = sqlDataReader.GetString(3);
                        pago.TPA_CODIGO = sqlDataReader.GetInt32(4);
                        pago.PAG_ESTADO = sqlDataReader.GetString(5);

                        
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(pago);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Pago> pagos = new List<Pago>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PAG_CODIGO, RES_CODIGO, PAG_FECHA, PAG_TIPO,TPA_CODIGO,PAG_ESTADO
                                                    FROM  PAGO", sqlConnection);

                    

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Pago pago = new Pago()
                        {
                            PAG_CODIGO = sqlDataReader.GetInt32(0),
                            RES_CODIGO = sqlDataReader.GetInt32(1),
                            PAG_FECHA = sqlDataReader.GetDateTime(2),
                            PAG_TIPO = sqlDataReader.GetString(3),
                            TPA_CODIGO = sqlDataReader.GetInt32(4),
                            PAG_ESTADO = sqlDataReader.GetString(5),
                            
                        };
                        pagos.Add(pago);

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(pagos);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Pago pago)
        {
            if (pago == null)
                return BadRequest();


            if (RegistrarHabitacion(pago))
                return Ok(pago);

            else
                return InternalServerError();



        }
        private bool RegistrarHabitacion(Pago pago)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO PAGO (RES_CODIGO, PAG_FECHA,PAG_TIPO,TPA_CODIGO,PAG_ESTADO) VALUES(
                            @RES_CODIGO,@PAG_FECHA, @PAG_TIPO,@TPA_CODIGO,
                           @PAG_ESTADO)", sqlConnection);


                
                sqlCommand.Parameters.AddWithValue("@RES_CODIGO", pago.RES_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAG_FECHA", pago.PAG_FECHA);
                sqlCommand.Parameters.AddWithValue("@PAG_TIPO", pago.PAG_TIPO);
                sqlCommand.Parameters.AddWithValue("@TPA_CODIGO", pago.TPA_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAG_ESTADO", pago.PAG_ESTADO);
                

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
        public IHttpActionResult Put(Pago pago)
        {
            if (pago == null)
                return BadRequest();


            if (ActualizarHabitacion(pago))
                return Ok(pago);

            else
                return InternalServerError();



        }
        private bool ActualizarHabitacion(Pago pago)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["reservas"].ConnectionString))
            {

                SqlCommand sqlCommand = new SqlCommand(@"UPDATE PAGO SET
                                                    
                                                        
                                                           RES_CODIGO   = @RES_CODIGO,
                                                          PAG_FECHA= @PAG_FECHA,
                                                           PAG_TIPO =@PAG_TIPO,
                                                               TPA_CODIGO = @TPA_CODIGO,
                                                                  PAG_ESTADO = @PAG_ESTADO
                                                                   WHERE PAG_CODIGO=@PAG_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@PAG_CODIGO", pago.PAG_CODIGO);
                sqlCommand.Parameters.AddWithValue("@RES_CODIGO", pago.RES_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAG_FECHA", pago.PAG_FECHA);
                sqlCommand.Parameters.AddWithValue("@PAG_TIPO", pago.PAG_TIPO);
                sqlCommand.Parameters.AddWithValue("@TPA_CODIGO", pago.TPA_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAG_ESTADO", pago.PAG_ESTADO);
                

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


            if (EliminarHabitacion(id))
                return Ok(id);

            else
                return InternalServerError();





        }

        private bool EliminarHabitacion(int id)
        {
            bool resultado = false;


            using (SqlConnection sqlConnection = new
                 SqlConnection(ConfigurationManager.ConnectionStrings["reservas"].ConnectionString))
            {

                SqlCommand sqlCommand = new SqlCommand(@"DELETE PAGO
                                                            WHERE PAG_CODIGO = @PAG_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@PAG_CODIGO", id);


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
