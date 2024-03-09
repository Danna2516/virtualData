using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRUEBAS_LOGIN.Models
{
    public class Usuario
    {
        public string IdUsuario { get; set; }
        public string  Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string FechaRegistro { get; set; }
        public string IdRol { get; set; }
        public string Contraseña { get; set; }


        public string ConfirmarContraseña { get; set; }

    }
}