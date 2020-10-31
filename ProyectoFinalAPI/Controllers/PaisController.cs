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
    [RoutePrefix("api/pais")]
    public class PaisController: ApiController
    {
        [HttpGet]
       
        public IHttpActionResult GetId(int id)
        {
            Pais pais = new Pais();

            try
            {
                using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PAIS_CODIGO, PAIS_NOMBRE
                                                            FROM PAIS WHERE PAIS_CODIGO = @PAIS_CODIGO", sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@PAIS_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())

                    {
                        pais.PAIS_CODIGO = sqlDataReader.GetInt32(0);
                        pais.PAIS_NOMBRE = sqlDataReader.GetString(1);
                     
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(pais);
        }



        [HttpGet]
        //[Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            List<Pais> paises = new List<Pais>();


            try
            {
                using (SqlConnection sqlConnection = new
                 SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PAIS_CODIGO, PAIS_NOMBRE
                                                            FROM PAIS", sqlConnection);

                    //sqlCommand.Parameters.AddWithValue("@PAIS_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Pais pais = new Pais()
                        {

                            PAIS_CODIGO = sqlDataReader.GetInt32(0),
                            PAIS_NOMBRE = sqlDataReader.GetString(1),
                    };
                       paises.Add(pais);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(paises);
        }

        [HttpPost]
        [Route("Ingresar")]
        public IHttpActionResult Ingresar(Pais pais)
        {
            if (pais == null)
                return BadRequest();
            if (RegistrarPais(pais))
                return Ok(pais);
            else
                return InternalServerError();
        }
        private bool RegistrarPais(Pais pais)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO PAIS (PAIS_NOMBRE) 
                                                      VALUES (@PAIS_NOMBRE) ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@PAIS_NOMBRE", pais.PAIS_NOMBRE);
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
        public IHttpActionResult Put(Pais pais)
        {
            if (pais == null)
                return BadRequest();
            if (ActualizarPais(pais))
                return Ok(pais);
            else
                return InternalServerError();
        }
        private bool ActualizarPais(Pais pais)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
               SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE PAIS SET            
                    PAIS_NOMBRE = @PAIS_NOMBRE
                    WHERE PAIS_CODIGO = @PAIS_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@PAIS_CODIGO", pais.PAIS_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAIS_NOMBRE", pais.PAIS_NOMBRE);
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
                SqlCommand sqlCommand = new SqlCommand(@"DELETE PAIS WHERE PAIS_CODIGO = @PAIS_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@PAIS_CODIGO", id);

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




