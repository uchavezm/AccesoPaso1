using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccesoPaso1.Models;

namespace AccesoPaso1.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {

        contextTienda db = new contextTienda();
        // GET: Pedidos
        public ActionResult Index()
        {
            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                         where c.email == correo
                         select c).ToList().FirstOrDefault();

            int id = cl.Id_cliente;

            var query = from o in db.orden
                        where o.id_cliente == id
                        orderby o.fecha_creacion ascending
                        select o;

            List<orden> ordenes = query.ToList();
            List<PedidoCliente> pedidos = new List<PedidoCliente>();
            PedidoCliente pedido;
            List<orden_producto> ordPed;
            List<ItemPedido> itemPed= new List<ItemPedido>();

            ItemPedido iPed;

            foreach (orden o in ordenes) {
                pedido = new PedidoCliente();
                pedido.Orden = o;
                pedido.envio = o.fecha_envio.GetValueOrDefault().ToShortDateString();
                if (o.fecha_envio.HasValue)
                {
                    pedido.envio = o.fecha_envio.GetValueOrDefault().ToShortDateString();

                }
                else {
                    pedido.envio = "Proximamanete";
                }
                if (o.fecha_entrega.HasValue)
                {
                    pedido.envio = o.fecha_envio.GetValueOrDefault().ToShortDateString();
                }
                else {
                    pedido.status = "Sin entregar";
                }
                pedido.Total = o.Total.ToString();
                pedidos.Add(pedido);
                ordPed = (from oP in db.orden_producto
                          where oP.Id_orden == o.Id_orden
                          select oP).ToList();
                pedido.ordenProductos = ordPed;
                foreach (orden_producto op in ordPed) {
                    iPed = new ItemPedido();
                    iPed.idOrd = op.Id_orden;
                    iPed.Product = db.producto.First(p => p.Id_producto == op.id_producto);
                    iPed.Cantidad = op.cantidad;
                    itemPed.Add(iPed);
                }
            }
            Session["misPedidos"] = pedidos;
            Session["Pedido"] = itemPed;


            return View();
        }
    }
}