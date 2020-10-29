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
    [RoutePrefix("api/aeropuerto")]
    public class AeropuertoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Aeropuerto aeropuerto = new Aeropuerto();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AEP_CODIGO, AEP_NOMBRE, PAIS_CODIGO FROM AEROPUERTO
                                                             WHERE AEP_CODIGO = @AEP_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue(@"AEP_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {

                        aeropuerto.AEP_CODIGO = sqlDataReader.GetInt32(0);
                        aeropuerto.AEP_NOMBRE = sqlDataReader.GetString(1);
                        aeropuerto.PAIS_CODIGO = sqlDataReader.GetInt32(2);
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(aeropuerto);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Aeropuerto> aeropuertos = new List<Aeropuerto>();


            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AEP_CODIGO, AEP_NOMBRE, PAIS_CODIGO FROM AEROPUERTO", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Aeropuerto aeropuerto = new Aeropuerto()
                        {
                            AEP_CODIGO = sqlDataReader.GetInt32(0),
                            AEP_NOMBRE = sqlDataReader.GetString(1),
                            PAIS_CODIGO = sqlDataReader.GetInt32(2)
                        };

                        aeropuertos.Add(aeropuerto);

                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(aeropuertos);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Aeropuerto aeropuerto)
        {
            if (aeropuerto == null)
                return BadRequest();

            if (RegistrarAeropuerto(aeropuerto))
                return Ok(aeropuerto);
            else
                return InternalServerError();
        }

        private bool RegistrarAeropuerto(Aeropuerto aeropuerto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO AEROPUERTO (AEP_NOMBRE, PAIS_CODIGO) 
                                                        VALUES (@AEP_NOMBRE, @PAIS_CODIGO)",
                                                        sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AEP_NOMBRE", aeropuerto.AEP_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@PAIS_CODIGO", aeropuerto.PAIS_CODIGO);
            
                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Aeropuerto aeropuerto)
        {
            if (aeropuerto == null)
                return BadRequest();

            if (ActualizarAeropuerto(aeropuerto))
                return Ok(aeropuerto);
            else
                return InternalServerError();
        }


        private bool ActualizarAeropuerto(Aeropuerto aeropuerto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE AEROPUERTO SET AEP_NOMBRE = @AEP_NOMBRE, PAIS_CODIGO = @PAIS_CODIGO 
                                                        WHERE AEP_CODIGO = @AEP_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AEP_CODIGO", aeropuerto.AEP_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AEP_NOMBRE", aeropuerto.AEP_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@PAIS_CODIGO", aeropuerto.PAIS_CODIGO);

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

            if (EliminarAeropuerto(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarAeropuerto(int id)
        {

            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE AEROPUERTO WHERE AEP_CODIGO = @AEP_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AEP_CODIGO", id);

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
