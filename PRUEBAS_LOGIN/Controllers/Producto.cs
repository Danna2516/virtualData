﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRUEBAS_LOGIN.Controllers
{
    public class Producto
    {
        public string IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Presentacion { get; set; }
        public string Descripcion { get; set; }
        public string PrecioUnitarioEntrada { get; set; }
        public string PrecioUnitarioSalida { get; set; }
        // public bool Activo { get; set; }
    }

}