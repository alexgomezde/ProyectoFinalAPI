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
    [RoutePrefix("api/vuelo")]
    public class VueloController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Vuelo vuelo = new Vuelo();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT VUE_CODIGO, VUE_CODIGO_ASI, AER_ORIGEN_COD, AER_DESTINO_COD, AVI_CODIGO, TAR_CODIGO, 
                                                            VUE_ESTADO FROM VUELO WHERE VUE_CODIGO = @VUE_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@VUE_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        vuelo.VUE_CODIGO = sqlDataReader.GetInt32(0);
                        vuelo.VUE_CODIGO_ASI = sqlDataReader.GetInt32(1);
                        vuelo.AER_ORIGEN_COD = sqlDataReader.GetInt32(2);
                        vuelo.AER_DESTINO_COD = sqlDataReader.GetInt32(3);
                        vuelo.AVI_CODIGO = sqlDataReader.GetInt32(4);
                        vuelo.TAR_CODIGO = sqlDataReader.GetInt32(5);
                        vuelo.VUE_ESTADO = sqlDataReader.GetString(6);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(vuelo);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Vuelo> vuelos = new List<Vuelo>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT VUE_CODIGO, VUE_CODIGO_ASI, AER_ORIGEN_COD, AER_DESTINO_COD, AVI_CODIGO, 
                                                            TAR_CODIGO, VUE_ESTADO FROM VUELO", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Vuelo vuelo = new Vuelo()
                        {
                            VUE_CODIGO = sqlDataReader.GetInt32(0),
                            VUE_CODIGO_ASI = sqlDataReader.GetInt32(1),
                            AER_ORIGEN_COD = sqlDataReader.GetInt32(2),
                            AER_DESTINO_COD = sqlDataReader.GetInt32(3),
                            AVI_CODIGO = sqlDataReader.GetInt32(4),
                            TAR_CODIGO = sqlDataReader.GetInt32(5),
                            VUE_ESTADO = sqlDataReader.GetString(6)
                        };

                        vuelos.Add(vuelo);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(vuelos);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Vuelo vuelo)
        {
            if (vuelo == null)
                return BadRequest();

            if (RegistrarVuelo(vuelo))
                return Ok(vuelo);
            else
                return InternalServerError();
        }

        private bool RegistrarVuelo(Vuelo vuelo)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO VUELO (VUE_CODIGO_ASI, AER_ORIGEN_COD, AER_DESTINO_COD, AVI_CODIGO, TAR_CODIGO, 
                                                        VUE_ESTADO) VALUES (@VUE_CODIGO_ASI, @AER_ORIGEN_COD, @AER_DESTINO_COD, @AVI_CODIGO, @TAR_CODIGO, 
                                                        @VUE_ESTADO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@VUE_CODIGO_ASI", vuelo.VUE_CODIGO_ASI);
                sqlCommand.Parameters.AddWithValue("@AER_ORIGEN_COD", vuelo.AER_ORIGEN_COD);
                sqlCommand.Parameters.AddWithValue("@AER_DESTINO_COD", vuelo.AER_DESTINO_COD);
                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", vuelo.AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@TAR_CODIGO", vuelo.TAR_CODIGO);
                sqlCommand.Parameters.AddWithValue("@VUE_ESTADO", vuelo.VUE_ESTADO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Vuelo vuelo)
        {
            if (vuelo == null)
                return BadRequest();

            if (ActualizarVuelo(vuelo))
                return Ok(vuelo);
            else
                return InternalServerError();
        }

        private bool ActualizarVuelo(Vuelo vuelo)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE VUELO SET VUE_CODIGO_ASI = @VUE_CODIGO_ASI, AER_ORIGEN_COD = @AER_ORIGEN_COD, 
                                                        AER_DESTINO_COD = @AER_DESTINO_COD, AVI_CODIGO = @AVI_CODIGO, TAR_CODIGO = @TAR_CODIGO, VUE_ESTADO = @VUE_ESTADO
                                                        WHERE VUE_CODIGO = @VUE_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@VUE_CODIGO", vuelo.VUE_CODIGO);
                sqlCommand.Parameters.AddWithValue("@VUE_CODIGO_ASI", vuelo.VUE_CODIGO_ASI);
                sqlCommand.Parameters.AddWithValue("@AER_ORIGEN_COD", vuelo.AER_ORIGEN_COD);
                sqlCommand.Parameters.AddWithValue("@AER_DESTINO_COD", vuelo.AER_DESTINO_COD);
                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", vuelo.AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@TAR_CODIGO", vuelo.TAR_CODIGO);
                sqlCommand.Parameters.AddWithValue("@VUE_ESTADO", vuelo.VUE_ESTADO);

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

            if (EliminarVuelo(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarVuelo(int id)
        {

            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE VUELO WHERE VUE_CODIGO = @VUE_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@VUE_CODIGO", id);


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
