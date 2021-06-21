using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccesoPaso1.Models;
using System.Security.Cryptography;


namespace AccesoPaso1.Controllers
{
    [Authorize]

    public class PagoController : Controller
    {

        private contextTienda db = new contextTienda();
        private string NumconfirPago;

        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CrearOrden()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session["CrearOrden"] = "pend";
                return RedirectToAction("Login", "Account");
            }
       
 
            string correo = User.Identity.Name;

            var db = new contextTienda();
            string fechaCreacion = DateTime.Today.ToShortDateString();
            string fechaProbEntrga = DateTime.Today.AddDays(3).ToShortDateString();
            var cliente = (
                from c in db.cliente
                where c.email == correo
                select c).ToList().FirstOrDefault();

            Session["dirCliente"] = cliente.calle_t + " " + cliente.colonia_t + " " + cliente.estado_t;
            Session["fechaOrden"] = fechaCreacion;
            Session["fPEntreg"] = fechaProbEntrga;
            Session["nTarj"] = cliente.num_tarj_cred_principal;

            if (cliente.num_tarj_cred_principal.StartsWith("4"))
                Session["tTarj"] = "1";
            if (cliente.num_tarj_cred_principal.StartsWith("5"))
                Session["tTarj"] = "2";
            if (cliente.num_tarj_cred_principal.StartsWith("3"))
                Session["tTarj"] = "3";



 //           var orden = new orden();
 //           var bd = new contextTienda();
            return View();
        }

        public ActionResult Pagar(String tipoPago)
        {
            string correo = User.Identity.Name;
            DateTime fechaCreacion = DateTime.Today;
            DateTime fechaProbEntrega = fechaCreacion.AddDays(3);
            var cliente = (from c in db.cliente
                         where c.email == correo
                         select c).ToList().FirstOrDefault();
            int idClient = cliente.Id_cliente;

            if (tipoPago.Equals("T")) {
                if (!validaPago(cliente))
                {
                    return RedirectToAction("pagoNoAceptado");
                }
                else {
                    var dirEnt = (from d in db.dirEntrega
                                   where d.id_cliente == cliente.Id_cliente
                                   select d).ToList().FirstOrDefault();

                    int idDir = dirEnt.Id_dirEnt;
                    return RedirectToAction("pagoAceptado", routeValues: new { idC = idClient, idD = idDir } );
                }

            }
            if (tipoPago.Equals("p"))
            {
                var dirEnt = (from d in db.dirEntrega
                              where d.id_cliente == cliente.Id_cliente
                              select d).ToList().FirstOrDefault();
                int idDir = dirEnt.Id_dirEnt;
                validaPago(cliente);
                return RedirectToAction("pagoPaypal", routeValues: new { idC = idClient, idD = idDir });

            }
            return View();
        }

        public ActionResult pagoNoAceptado()
        {
            return View();
        }

            public ActionResult pagoAceptado(int idC, int idD)
        {
            orden orden_cliente = new orden();
            int id = 0;
            if (!(db.orden.Max(o => (int?)o.Id_orden) == null))
            {

                id = db.orden.Max(o => o.Id_orden);
            }
            else
            {
                id = 0;
            }
            id++;
            orden_cliente.Id_orden = id;
            orden_cliente.fecha_creacion = DateTime.Today;
            orden_cliente.num_confirmacion = Session["nConfirma"].ToString();
            var carro = Session["cart"] as List<Item>;
            var total = carro.Sum(item => item.Product.precio * item.Cantidad);
            orden_cliente.Total = total;
            orden_cliente.id_cliente = idC;
            orden_cliente.id_dirEntrega = idD;
            db.orden.Add(orden_cliente);
            db.SaveChanges();

            orden_producto ordenProd;
            List<orden_producto> listaProdOrd = new List<orden_producto>();
            foreach (Item linea in carro)
            {
                ordenProd = new orden_producto();
                ordenProd.Id_orden = orden_cliente.Id_orden;
                ordenProd.id_producto = linea.Product.Id_producto;
                ordenProd.cantidad = linea.Cantidad;
                db.orden_producto.Add(ordenProd);

            }
            db.SaveChanges();
            Session["cart"] = null;
            Session["nConfirma"] = null;
            Session["itemTotal"] = 0;
            return View();
        }



        public ActionResult pagoPaypaly(int idC, int idD)
        {
            Session["idDir"] = idC;
            Session["idClient"] = idD;
            return View();
        }


        public ActionResult pagandoPaypaly(int idC, int idD)
        {
            Session["idDir"] = idC;
            Session["idClient"] = idD;
            return View();
        }

      

        private bool validaPago(cliente cliente)
        {

            bool retorna = true;
            int randomValue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider()) {
                byte[] val = new byte[6];
                crypto.GetBytes(val);
                randomValue = BitConverter.ToInt32(val, 1);

            }
            NumconfirPago = Math.Abs(randomValue * 1000).ToString();
            Session["nConfirma"] = NumconfirPago;
                return true;
        }
    }
}