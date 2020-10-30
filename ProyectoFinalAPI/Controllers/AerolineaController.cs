using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/aerolinea")]
    public class AerolineaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Aerolinea aerolinea = new Aerolinea();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AER_CODIGO ,AER_RUC ,AER_NOMBRE
                                                            ,AER_ESTADO
                                                            FROM AEROLINEA WHERE AER_CODIGO = @AER_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@AER_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        aerolinea.AER_CODIGO = sqlDataReader.GetInt32(0);
                        aerolinea.AER_RUC = sqlDataReader.GetString(1);
                        aerolinea.AER_NOMBRE = sqlDataReader.GetString(2);
                        aerolinea.AER_ESTADO = sqlDataReader.GetString(3);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(aerolinea);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Aerolinea> aerolineas = new List<Aerolinea>();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AER_CODIGO ,AER_RUC ,AER_NOMBRE
                                                            ,AER_ESTADO
                                                            FROM AEROLINEA", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Aerolinea aerolinea = new Aerolinea();
                        {
                            aerolinea.AER_CODIGO = sqlDataReader.GetInt32(0);
                            aerolinea.AER_RUC = sqlDataReader.GetString(1);
                            aerolinea.AER_NOMBRE = sqlDataReader.GetString(2);
                            aerolinea.AER_ESTADO = sqlDataReader.GetString(3);

                        };

                        aerolineas.Add(aerolinea);

                    }

                    sqlConnection.Close();

                }



            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(aerolineas);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Aerolinea aerolinea)
        {
            if (aerolinea == null)
                return BadRequest();

            if (RegistrarAerolinea(aerolinea))
                return Ok(aerolinea);
            else
                return InternalServerError();
        }

        private bool RegistrarAerolinea(Aerolinea aerolinea)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO AEROLINEA (AER_RUC ,AER_NOMBRE,AER_ESTADO)
                                                        VALUES (@AER_RUC, @AER_NOMBRE, @AER_ESTADO)", sqlConnection);


                sqlCommand.Parameters.AddWithValue("@AER_RUC", aerolinea.AER_RUC);
                sqlCommand.Parameters.AddWithValue("@AER_NOMBRE", aerolinea.AER_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@AER_ESTADO", aerolinea.AER_ESTADO);

                sqlConnection.Open();

                //ExecuteNonQuery elimina o inserta datos
                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Aerolinea aerolinea)
        {
            if (aerolinea == null)
                return BadRequest();

            if (ActualizarAerolinea(aerolinea))
                return Ok(aerolinea);
            else
                return InternalServerError();
        }

        private bool ActualizarAerolinea(Aerolinea aerolinea)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE AEROLINEA SET
                                                            AER_RUC = @AER_RUC, 
                                                            AER_NOMBRE = @AER_NOMBRE, 
                                                            AER_ESTADO = @AER_ESTADO
                                                            WHERE AER_CODIGO = @AER_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", aerolinea.AER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AER_RUC", aerolinea.AER_RUC);
                sqlCommand.Parameters.AddWithValue("@AER_NOMBRE", aerolinea.AER_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@AER_ESTADO", aerolinea.AER_ESTADO);


                sqlConnection.Open();

                //ExecuteNonQuery elimina o inserta datos
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

            if (EliminarAerolinea(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarAerolinea(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE AEROLINEA
                                                            WHERE AER_CODIGO = @AER_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", id);

                sqlConnection.Open();

                //ExecuteNonQuery elimina o inserta datos
                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();

            }

            return resultado;

        }

    }
}