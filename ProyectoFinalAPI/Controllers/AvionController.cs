using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/avion")]
    public class AvionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Avion avion = new Avion();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AVI_CODIGO ,AER_CODIGO ,AVI_FABRICANTE
                                                            ,AVI_TIPO ,AVI_CAPACIDAD ,AVI_ESTADO
                                                            FROM AVION WHERE AVI_CODIGO = @AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        avion.AVI_CODIGO = sqlDataReader.GetInt32(0);
                        avion.AER_CODIGO = sqlDataReader.GetInt32(1);
                        avion.AVI_FABRICANTE = sqlDataReader.GetString(2);
                        avion.AVI_TIPO = sqlDataReader.GetString(3);
                        avion.AVI_CAPACIDAD = sqlDataReader.GetInt32(4);
                        avion.AVI_ESTADO = sqlDataReader.GetString(5);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(avion);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Avion> aviones = new List<Avion>();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AVI_CODIGO ,AER_CODIGO ,AVI_FABRICANTE
                                                            ,AVI_TIPO ,AVI_CAPACIDAD ,AVI_ESTADO
                                                            FROM AVION", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Avion avion = new Avion();
                        {
                            avion.AVI_CODIGO = sqlDataReader.GetInt32(0);
                            avion.AER_CODIGO = sqlDataReader.GetInt32(1);
                            avion.AVI_FABRICANTE = sqlDataReader.GetString(2);
                            avion.AVI_TIPO = sqlDataReader.GetString(3);
                            avion.AVI_CAPACIDAD = sqlDataReader.GetInt32(4);
                            avion.AVI_ESTADO = sqlDataReader.GetString(5);
                        };

                        aviones.Add(avion);

                    }

                    sqlConnection.Close();

                }



            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(aviones);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Avion avion)
        {
            if (avion == null)
                return BadRequest();

            if (RegistrarAvion(avion))
                return Ok(avion);
            else
                return InternalServerError();
        }

        private bool RegistrarAvion(Avion avion)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO AVION (AER_CODIGO ,AVI_FABRICANTE
                                                            ,AVI_TIPO ,AVI_CAPACIDAD ,AVI_ESTADO)
                                                        VALUES (@AER_CODIGO, @AVI_FABRICANTE, 
                                                        @AVI_TIPO, @AVI_CAPACIDAD, @AVI_ESTADO)", sqlConnection);


                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", avion.AER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AVI_FABRICANTE", avion.AVI_FABRICANTE);
                sqlCommand.Parameters.AddWithValue("@AVI_TIPO", avion.AVI_TIPO);
                sqlCommand.Parameters.AddWithValue("@AVI_CAPACIDAD", avion.AVI_CAPACIDAD);
                sqlCommand.Parameters.AddWithValue("@AVI_ESTADO", avion.AVI_ESTADO);

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
        public IHttpActionResult Put(Avion avion)
        {
            if (avion == null)
                return BadRequest();

            if (ActualizarAvion(avion))
                return Ok(avion);
            else
                return InternalServerError();
        }

        private bool ActualizarAvion(Avion avion)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE AVION SET 
                                                            AER_CODIGO = @AER_CODIGO, 
                                                            AVI_FABRICANTE = @AVI_FABRICANTE, 
                                                            AVI_TIPO = @AVI_TIPO, 
                                                            AVI_CAPACIDAD = @AVI_CAPACIDAD,
                                                            AVI_ESTADO = @AVI_ESTADO
                                                            WHERE AVI_CODIGO = @AVI_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", avion.AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", avion.AER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AVI_FABRICANTE", avion.AVI_FABRICANTE);
                sqlCommand.Parameters.AddWithValue("@AVI_TIPO", avion.AVI_TIPO);
                sqlCommand.Parameters.AddWithValue("@AVI_CAPACIDAD", avion.AVI_CAPACIDAD);
                sqlCommand.Parameters.AddWithValue("@AVI_ESTADO", avion.AVI_ESTADO);


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

            if (EliminarAvion(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarAvion(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE AVION
                                                            WHERE AVI_CODIGO = @AVI_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", id);

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