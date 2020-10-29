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
    [RoutePrefix("api/habitacion")]
    public class HabitacionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Habitacion habitacion = new Habitacion();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT HAB_CODIGO, HOT_CODIGO, HAB_NUMERO, HAB_CAPACIDAD, HAB_TIPO, HAB_DESCRIPCION, 
                                                            HAB_ESTADO, HAB_PRECIO FROM HABITACION WHERE HAB_CODIGO = @HAB_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@HAB_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        habitacion.HAB_CODIGO = sqlDataReader.GetInt32(0);
                        habitacion.HOT_CODIGO = sqlDataReader.GetInt32(1);
                        habitacion.HAB_NUMERO = sqlDataReader.GetInt32(2);
                        habitacion.HAB_CAPACIDAD = sqlDataReader.GetInt32(3);
                        habitacion.HAB_TIPO = sqlDataReader.GetString(4);
                        habitacion.HAB_DESCRIPCION = sqlDataReader.GetString(5);
                        habitacion.HAB_ESTADO = sqlDataReader.GetString(6);
                        habitacion.HAB_PRECIO = sqlDataReader.GetDecimal(7);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(habitacion);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Habitacion> habitaciones = new List<Habitacion>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT HAB_CODIGO, HOT_CODIGO, HAB_NUMERO, HAB_CAPACIDAD, HAB_TIPO, HAB_DESCRIPCION, 
                                                            HAB_ESTADO, HAB_PRECIO FROM HABITACION", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Habitacion habitacion = new Habitacion()
                        {

                            HAB_CODIGO = sqlDataReader.GetInt32(0),
                            HOT_CODIGO = sqlDataReader.GetInt32(1),
                            HAB_NUMERO = sqlDataReader.GetInt32(2),
                            HAB_CAPACIDAD = sqlDataReader.GetInt32(3),
                            HAB_TIPO = sqlDataReader.GetString(4),
                            HAB_DESCRIPCION = sqlDataReader.GetString(5),
                            HAB_ESTADO = sqlDataReader.GetString(6),
                            HAB_PRECIO = sqlDataReader.GetDecimal(7)

                        };

                        habitaciones.Add(habitacion);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(habitaciones);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Habitacion habitacion)
        {
            if (habitacion == null)
                return BadRequest();

            if (RegistrarHabitacion(habitacion))
                return Ok(habitacion);
            else
                return InternalServerError();
        }

        private bool RegistrarHabitacion(Habitacion habitacion)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO HABITACION (HOT_CODIGO, HAB_NUMERO, HAB_CAPACIDAD, HAB_TIPO, 
                                                        HAB_DESCRIPCION, HAB_ESTADO, HAB_PRECIO) VALUES (@HOT_CODIGO, @HAB_NUMERO, 
                                                        @HAB_CAPACIDAD, @HAB_TIPO, @HAB_DESCRIPCION, @HAB_ESTADO, @HAB_PRECIO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@HOT_CODIGO", habitacion.HOT_CODIGO);
                sqlCommand.Parameters.AddWithValue("@HAB_NUMERO", habitacion.HAB_NUMERO);
                sqlCommand.Parameters.AddWithValue("@HAB_CAPACIDAD", habitacion.HAB_CAPACIDAD);
                sqlCommand.Parameters.AddWithValue("@HAB_TIPO", habitacion.HAB_TIPO);
                sqlCommand.Parameters.AddWithValue("@HAB_DESCRIPCION", habitacion.HAB_DESCRIPCION);
                sqlCommand.Parameters.AddWithValue("@HAB_ESTADO", habitacion.HAB_ESTADO);
                sqlCommand.Parameters.AddWithValue("@HAB_PRECIO", habitacion.HAB_PRECIO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }

            return resultado;
        }


        [HttpPut]
        public IHttpActionResult Put(Habitacion habitacion)
        {
            if (habitacion == null)
                return BadRequest();

            if (ActualizarHabitacion(habitacion))
                return Ok(habitacion);
            else
                return InternalServerError();
        }

        private bool ActualizarHabitacion(Habitacion habitacion)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE HABITACION SET HOT_CODIGO = @HOT_CODIGO, HAB_NUMERO = @HAB_NUMERO, HAB_CAPACIDAD = @HAB_CAPACIDAD, 
                                                        HAB_TIPO = @HAB_TIPO, HAB_DESCRIPCION = @HAB_DESCRIPCION, HAB_ESTADO = @HAB_ESTADO,
                                                        HAB_PRECIO = @HAB_PRECIO
                                                        WHERE HAB_CODIGO = @HAB_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@HAB_CODIGO", habitacion.HAB_CODIGO);
                sqlCommand.Parameters.AddWithValue("@HOT_CODIGO", habitacion.HOT_CODIGO);
                sqlCommand.Parameters.AddWithValue("@HAB_NUMERO", habitacion.HAB_NUMERO);
                sqlCommand.Parameters.AddWithValue("@HAB_CAPACIDAD", habitacion.HAB_CAPACIDAD);
                sqlCommand.Parameters.AddWithValue("@HAB_TIPO", habitacion.HAB_TIPO);
                sqlCommand.Parameters.AddWithValue("@HAB_DESCRIPCION", habitacion.HAB_DESCRIPCION);
                sqlCommand.Parameters.AddWithValue("@HAB_ESTADO", habitacion.HAB_ESTADO);
                sqlCommand.Parameters.AddWithValue("@HAB_PRECIO", habitacion.HAB_PRECIO);

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

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE HABITACION WHERE HAB_CODIGO = @HAB_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@HAB_CODIGO", id);


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
