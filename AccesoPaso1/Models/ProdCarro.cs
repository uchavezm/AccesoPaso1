using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccesoPaso1.Models
{
    public class ProdCarro
    {
        private contextTienda db = new contextTienda();
        private List<producto> products;
        public ProdCarro()
        {
            products = db.producto.ToList();
        }
        public List<producto> findAll()
        {
            return this.products;
        }
        public producto find(int id)
        {
            producto pp = this.products.Single(p => p.Id_producto.Equals(id));
            return pp;
        }
    }
}