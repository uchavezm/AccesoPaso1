using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccesoPaso1.Models;

namespace AccesoPaso1.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {

        private contextTienda db = new contextTienda();
        // GET: Usuario
        public ActionResult Index(String email)
        {
            if (User.Identity.IsAuthenticated)
            {
                string correo = email;
                string rol = "";

                using (db)
                {
                    var query = from st in db.Empleado
                                where st.email == correo
                                select st;
                    var lista = query.ToList();
                    if (lista.Count > 0)
                    {
                        var empleado = query.FirstOrDefault<Empleado>();
                        string[] nombres = empleado.Nombre.ToString().Split(' ');
                        Session["name"] = nombres[0];
                        Session["usr"] = empleado.Nombre;
                        rol = empleado.rol.ToString().TrimEnd();
                        if (HttpContext.Request.Cookies["usuario"] == null)
                        {
                            HttpCookie cookie = new HttpCookie("usuario");
                            cookie["rol"] = rol;
                            cookie["name"] = Session["name"].ToString();
                            cookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(cookie);
                        }
                    }
                    else
                    {
                        var query1 = from st in db.cliente
                                     where st.email == correo
                                     select st;
                        var lista1 = query1.ToList();
                        if (lista1.Count > 0)
                        {
                            cliente cliente = query1.FirstOrDefault<cliente>();
                            string[] nombres = cliente.nombre.ToString().Split(' ');
                            Session["name"] = nombres[0];
                            Session["usr"] = cliente.nombre;
                            rol = "cliente";
                            if (HttpContext.Request.Cookies["usuario"] == null)
                            {
                                HttpCookie cookie = new HttpCookie("usuario");
                                cookie["rol"] = rol;
                                cookie["name"] = Session["name"].ToString();
                                cookie["itemTotal"] = "0";
                                cookie.Expires = DateTime.Now.AddDays(1);
                                Response.Cookies.Add(cookie);
                            }
                        }

                    }

                }
                if (rol == "comprador")
                {
                    return RedirectToAction("Index", "Compras");
                }
                if (rol == "enviador")
                {
                    return RedirectToAction("Index", "Envios");
                }
                if (rol == "chateador")
                {
                    return RedirectToAction("Index", "Chat");
                }
                if (rol == "cliente")
                {
                    if (Session["CrearOrden"] != null)
                    {
                        if (Session["CrearOrden"].Equals("pend"))
                        {
                            return RedirectToAction("CrearOrden", "Pago");
                        }

                    }
                    else
                        return RedirectToAction("Index", "Home");

                }
                if (rol == "admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");


        }
        
    }
}