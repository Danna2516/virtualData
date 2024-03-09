using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlClient;
using PRUEBAS_LOGIN.Models;
using System.Data.Common;

using System.Web.Helpers;
using PRUEBAS_LOGIN.Controllers;
using PRUEBAS_LOGIN.Permisos;

namespace PRUEBAS_LOGIN.Controllers
{

    [ValidarSesion]
    public class HomeController : Controller
    {
        public List<Producto> product = new List<Producto>();

        public void consultaProductos()

        {
            SqlConnection conn = new SqlConnection("Data Source=YULI11;Initial Catalog=VIRTUALDATA1.0;User ID=sa;Password=Yukieth11;Connect Timeout=30;Encrypt=False");
            conn.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            List<Producto> produc = new List<Producto>();
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "spConsultarProducto";
            com.ExecuteNonQuery();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                Producto art = new Producto
                {
                    IdProducto = registros["idproducto"].ToString(),
                    NombreProducto = registros["nombreproducto"].ToString(),
                    Marca = registros["marca"].ToString(),
                    Presentacion = registros["presentacion"].ToString(),
                    Descripcion = registros["Descripcion"].ToString(),
                    PrecioUnitarioEntrada = registros["PrecioUnitarioEntrada"].ToString(),
                    PrecioUnitarioSalida = registros["PrecioUnitarioSalida"].ToString(),

                };
                produc.Add(art);
            }
            conn.Close();
            product = produc;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Productos(string IdProducto
            , string NombreProducto, string Marca, string Presentacion, string Descripcion, string PrecioUnitarioEntrada, string PrecioUnitarioSalida, string boton)
        {
            ViewBag.Message = "DISTRIBUIDORA Y REPRESENTACIONES JF EU";

            if (boton is null)
            {
                consultaProductos();
                return View(product);
            }
            else if (boton == "Agregar")
            {
                SqlConnection conn = new SqlConnection("Data Source=YULI11;Initial Catalog=VIRTUALDATA1.0;User ID=sa;Password=Yukieth11;Connect Timeout=30;Encrypt=False");

                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@IdProducto", SqlDbType.Int).Value = int.Parse(IdProducto); // Convertir a int
                com.Parameters.Add("@NombreProducto", SqlDbType.NVarChar).Value = NombreProducto;
                com.Parameters.Add("@Marca", SqlDbType.NVarChar).Value = Marca;
                com.Parameters.Add("@Presentacion", SqlDbType.NVarChar).Value = Presentacion;
                com.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = Descripcion;

                if (float.TryParse(PrecioUnitarioEntrada, out float precioUnitariEntrada) && float.TryParse(PrecioUnitarioSalida, out float precioSalida))
                {
                    com.Parameters.Add("@PrecioUnitarioEntrada", SqlDbType.Float).Value = PrecioUnitarioEntrada;
                    com.Parameters.Add("@PrecioUnitarioSalida", SqlDbType.Float).Value = PrecioUnitarioSalida;
                    com.CommandText = "spInsertarProducto";
                    com.ExecuteNonQuery();
                    consultaProductos();
                    return View(product);
                }
            }
            else if (boton == "Eliminar")
            {
                SqlConnection conn = new SqlConnection("Data Source=YULI11;Initial Catalog=VIRTUALDATA1.0;User ID=sa;Password=Yukieth11;Connect Timeout=30;Encrypt=False");

                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@IdProducto", SqlDbType.Int).Value = int.Parse(IdProducto);
                com.CommandText = "spEliminarProducto";
                com.ExecuteNonQuery();
                consultaProductos();
                return View(product);
            }

            else if (boton == "Actualizar")
            {
                SqlConnection conn = new SqlConnection("Data Source=YULI11;Initial Catalog=VIRTUALDATA1.0;User ID=sa;Password=Yukieth11;Connect Timeout=30;Encrypt=False");

                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
                com.Parameters.Add("@NombreProducto", SqlDbType.NVarChar).Value = NombreProducto;
                com.Parameters.Add("@Marca", SqlDbType.NVarChar).Value = Marca;
                com.Parameters.Add("@Presentacion", SqlDbType.NVarChar).Value = Presentacion;
                com.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = Descripcion;

                if (float.TryParse(PrecioUnitarioEntrada, out float precioUnitariEntrada) && float.TryParse(PrecioUnitarioSalida, out float precioSalida))
                {
                    com.Parameters.Add("@PrecioUnitarioEntrada", SqlDbType.Float).Value = PrecioUnitarioEntrada;
                    com.Parameters.Add("@PrecioUnitarioSalida", SqlDbType.Float).Value = PrecioUnitarioSalida;
                    com.CommandText = "sp_ModificarProducto";
                    com.ExecuteNonQuery();
                    consultaProductos();
                    return View(product);
                }
            }
            return View(product);
        }
        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }

    }
}








