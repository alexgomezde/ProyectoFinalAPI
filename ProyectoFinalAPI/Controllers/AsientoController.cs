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
    [RoutePrefix("api/asiento")]
    public class AsientoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Asiento asiento = new Asiento();

            try
            {
                using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT ASI_CODIGO, ASI_LETRA, ASI_FILA, ASI_ESTADO
                    FROM ASIENTO WHERE
                    ASI_CODIGO = @ASI_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ASI_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        asiento.ASI_CODIGO = sqlDataReader.GetInt32(0);
                        asiento.ASI_LETRA = sqlDataReader.GetString(1);
                        asiento.ASI_FILA = sqlDataReader.GetInt32(2);
                        asiento.ASI_ESTADO = sqlDataReader.GetString(3);

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(asiento);
        }


        [HttpGet]
        public IHttpActionResult GetAll(int id)
        {
            List<Asiento> asientos = new List<Asiento>();

            try
            {
                using (SqlConnection sqlConnection = new
                 SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT ASI_CODIGO, ASI_LETRA, ASI_FILA, ASI_ESTADO 
                    FROM ASIENTO WHERE
                    ASI_CODIGO = @ASI_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ASI_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Asiento asiento = new Asiento()
                        {
                            ASI_CODIGO = sqlDataReader.GetInt32(0),
                            ASI_LETRA = sqlDataReader.GetString(1),
                            ASI_FILA = sqlDataReader.GetInt32(2),
                            ASI_ESTADO = sqlDataReader.GetString(3),

                        };
                        asientos.Add(asiento);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(asientos);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Asiento asiento)
        {
            if (asiento == null)
                return BadRequest();
            if (RegistrarAsiento(asiento))
                return Ok(asiento);
            else
                return InternalServerError();
        }
        private bool RegistrarAsiento(Asiento asiento)
        {
            bool resultado = false;



            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO ASIENTO (ASI_LETRA, ASI_FILA, ASI_ESTADO)
                        VALUES (@ASI_LETRA, @ASI_FILA, @ASI_ESTADO) ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@ASI_LETRA", asiento.ASI_LETRA);
                sqlCommand.Parameters.AddWithValue("@ASI_FILA", asiento.ASI_FILA);
                sqlCommand.Parameters.AddWithValue("@ASI_ESTADO", asiento.ASI_ESTADO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }
            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Asiento asiento)
        {
            if (asiento == null)
                return BadRequest();
            if (ActualizarAsiento(asiento))
                return Ok(asiento);
            else
                return InternalServerError();
        }
        private bool ActualizarAsiento(Asiento asiento)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE ASIENTO SET
                ASI_LETRA = @ASI_LETRA,
                ASI_FILA = @ASI_FILA,
                ASI_ESTADO = @ASI_ESTADO
                WHERE ASI_CODIGO = @ASI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ASI_CODIGO", asiento.ASI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@ASI_LETRA", asiento.ASI_LETRA);
                sqlCommand.Parameters.AddWithValue("@ASI_FILA", asiento.ASI_FILA);
                sqlCommand.Parameters.AddWithValue("@ASI_ESTADO", asiento.ASI_ESTADO);


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
            if (EliminarAsiento(id))
                return Ok(id);
            else
                return InternalServerError();
        }
        private bool EliminarAsiento(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE ASIENTO WHERE ASI_CODIGO = @ASI_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@ASI_CODIGO", id);

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
