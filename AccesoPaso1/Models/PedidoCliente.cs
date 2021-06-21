using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccesoPaso1.Models
{
    public class PedidoCliente
    {
        public contextTienda db = new contextTienda();
        private List<orden_producto> detalle_orden;

        public PedidoCliente() {
            detalle_orden = db.orden_producto.ToList();
        }

        public orden Orden { get; set; }
        public string Fecha { get; set; }
        public string envio { get; set; }
        public string status { get; set; }
        public string Total { get; set; }
        public List<orden_producto> ordenProductos { get; set; }








    }
}