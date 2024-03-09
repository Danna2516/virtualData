using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using PRUEBAS_LOGIN.Models;
using PRUEBAS_LOGIN.Permisos;

namespace PRUEBAS_LOGIN.Controllers
{
    public class AccesoController : Controller
    {
       
        static string cadena = "Data Source=YULI11;Initial Catalog=VIRTUALDATA1.0;User ID=sa;Password=Yukieth11;Connect Timeout=30;Encrypt=False";

        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario oUsuario)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    // Encriptar la contraseña antes de registrar el usuario
                    oUsuario.Contraseña = ConvertirSha256(oUsuario.Contraseña);

                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                    cmd.Parameters.AddWithValue("@Email", oUsuario.Email);
                    cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@Contraseña", oUsuario.Contraseña);
                    cmd.Parameters.AddWithValue("@Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", oUsuario.Apellido);
                    cmd.Parameters.AddWithValue("@Cedula", oUsuario.Cedula);
                    cmd.Parameters.AddWithValue("@Telefono", oUsuario.Telefono);
                    cmd.Parameters.AddWithValue("@FechaRegistro", oUsuario.FechaRegistro);
                    cmd.Parameters.AddWithValue("@IdRol", oUsuario.IdRol);
                    cmd.Parameters.Add("@Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(cmd.Parameters["@Registrado"].Value);
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewData["Mensaje"] = "Error en la base de datos al registrar usuario.";
                    return View();
                }
            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_IniciarSesion", cn))
                    {
                        cmd.Parameters.AddWithValue("@Email", oUsuario.Email);
                        cmd.Parameters.AddWithValue("@Contraseña", oUsuario.Contraseña);

                        // Parámetros de salida
                        SqlParameter idUsuarioParam = new SqlParameter("@IdUsuario", SqlDbType.VarChar, 50);
                        idUsuarioParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(idUsuarioParam);

                        SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 100);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();

                        // Obtener los valores de los parámetros de salida
                        string idUsuario = idUsuarioParam.Value as string;
                        string mensaje = mensajeParam.Value.ToString();

                        if (!string.IsNullOrEmpty(idUsuario))
                        {
                            oUsuario.IdUsuario = idUsuario;
                            Session["usuario"] = oUsuario;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewData["Mensaje"] = mensaje;
                            return View();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewData["Mensaje"] = "Error en la base de datos al iniciar sesión.";
                    return View();
                }
            }
        }

        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
  

    
}
