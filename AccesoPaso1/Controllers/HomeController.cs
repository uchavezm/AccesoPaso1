using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccesoPaso1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (HttpContext.Request.Cookies["usuario"] != null)
            {
                HttpCookie usuario = HttpContext.Request.Cookies.Get("usuario");
                string rol;
                string items;
                rol = Request.Cookies.Get("usuario").Values.Get("rol");
                if (rol != "cliente")
                {
                    Session["itemToal"] = 0;
                    if ((Request.IsAuthenticated) && (Session["name"] == null))
                    {
                        string correo = User.Identity.Name;
                        return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                    }
                }

                else
                {
                    items = Request.Cookies.Get("usuario").Values.Get("itemTotal");
                    Session["itemTotal"] = items;

                    if ((Request.IsAuthenticated) && (Session["name"] == null))
                    {
                        string correo = User.Identity.Name;
                        return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                    }
                }
            }
            else
            {
                if (Session["itemTotal"] == null)
                    Session["itemTotal"] = 0;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}