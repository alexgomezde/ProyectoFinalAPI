using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using ProyectoFinalAPI.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApiSegura.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            Usuario usuarioValidado = ValidarUsuario(login);

            if (!string.IsNullOrEmpty(usuarioValidado.USU_IDENTIFICACION))
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Username);
                usuarioValidado.CadenaToken = token;
                return Ok(usuarioValidado);
            }
            else
            {
                return Unauthorized();
            }
        }

        private Usuario ValidarUsuario(LoginRequest loginRequest)
        {
            Usuario usuario = new Usuario();

            using (SqlConnection sqlConnection = 
                new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"SELECT USU_CODIGO, USU_IDENTIFICACION, USU_NOMBRE, USU_PASSWORD, USU_EMAIL, USU_ESTADO, 
                                                        USU_FEC_NAC, USU_TELEFONO FROM USUARIO WHERE USU_IDENTIFICACION = @USU_IDENTIFICACION", 
                                                        sqlConnection);

                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", loginRequest.Username);

                sqlConnection.Open();

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(); // ejecutar la consulta
                while (sqlDataReader.Read())
                {
                    if (loginRequest.Password.Equals(sqlDataReader.GetString(3).Trim()))
                    {
                        usuario.USU_CODIGO = sqlDataReader.GetInt32(0);
                        usuario.USU_IDENTIFICACION = sqlDataReader.GetString(1);
                        usuario.USU_NOMBRE = sqlDataReader.GetString(2);
                        usuario.USU_PASSWORD = sqlDataReader.GetString(3);
                        usuario.USU_EMAIL = sqlDataReader.GetString(4);
                        usuario.USU_ESTADO = sqlDataReader.GetString(5);
                        usuario.USU_FEC_NAC = sqlDataReader.GetDateTime(6);
                        usuario.USU_TELEFONO = sqlDataReader.GetString(7);
                    }
                }
                sqlConnection.Close();
            }

            return usuario;
        }


        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(Usuario usuario)
        {
            if (usuario == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                using (SqlConnection sqlConnection = 
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO USUARIO (USU_IDENTIFICACION, USU_NOMBRE, 
                                                            USU_PASSWORD, USU_EMAIL, USU_FEC_NAC, USU_ESTADO, 
                                                            USU_TELEFONO) VALUES (@USU_IDENTIFICACION, @USU_NOMBRE, 
                                                            @USU_PASSWORD, @USU_EMAIL, @USU_FEC_NAC, @USU_ESTADO, 
                                                            @USU_TELEFONO)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue(@"USU_IDENTIFICACION", usuario.USU_IDENTIFICACION);
                    sqlCommand.Parameters.AddWithValue(@"USU_NOMBRE", usuario.USU_NOMBRE);
                    sqlCommand.Parameters.AddWithValue(@"USU_PASSWORD", usuario.USU_PASSWORD);
                    sqlCommand.Parameters.AddWithValue(@"USU_EMAIL", usuario.USU_EMAIL);
                    sqlCommand.Parameters.AddWithValue(@"USU_FEC_NAC", usuario.USU_FEC_NAC);
                    sqlCommand.Parameters.AddWithValue(@"USU_ESTADO", usuario.USU_ESTADO);
                    sqlCommand.Parameters.AddWithValue(@"USU_TELEFONO", usuario.USU_TELEFONO);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    if (filasAfectadas < 0)
                        return InternalServerError();

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(usuario);
        }
    }
}