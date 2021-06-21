using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccesoPaso1.Models
{

    public class ItemPedido
    {

        public int idOrd { get; set; }
        public producto Product { get; set; }
        public int Cantidad { get; set; }
    }

}