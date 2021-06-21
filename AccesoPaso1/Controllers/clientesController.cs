using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AccesoPaso1.Models;
using Microsoft.AspNet.Identity;

namespace AccesoPaso1.Controllers
{
    public class clientesController : Controller
    {
        private contextTienda db = new contextTienda();

        // GET: clientes
        public ActionResult Index()
        {
            return View(db.cliente.ToList());
        }

        // GET: clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "Id_cliente,nombre,email,calle_t,colonia_t,estado_t,num_tarj_cred_principal")] cliente cliente)
        public ActionResult Create(string nombre, string email, string calle_t, string colonia_t, string estado_t, string num_tarj_cred_principal, string TipoTarj, string Mes, string Anio, string CVV)

        {
            cliente cliente = new cliente();
            int id = 0;
            if (!(db.cliente.Max(c => (int?)c.Id_cliente) == null))
            {
                id = db.cliente.Max(c => c.Id_cliente);
            }
            else {
                id = 0;
            }
            id++;
            
            if (Tarjeta(num_tarj_cred_principal, TipoTarj, Mes, Anio, CVV))
            {
                if (validaPago(nombre, calle_t, colonia_t, estado_t, num_tarj_cred_principal, Mes, Anio, CVV))
                {
                  cliente.Id_cliente = id;

                    cliente.nombre = nombre;
                    cliente.email = Session["correo"].ToString();
                    cliente.calle_t = calle_t;
                    cliente.colonia_t = colonia_t;
                    cliente.estado_t = estado_t;
                    cliente.num_tarj_cred_principal = num_tarj_cred_principal;
                    dirEntrega dir = new dirEntrega();
                    dir.Calle = calle_t;
                    dir.Colonia = colonia_t;
                    dir.Estado = estado_t;
                    dir.id_cliente = id;
                    db.cliente.Add(cliente);
                    db.SaveChanges();
                    db.dirEntrega.Add(dir);
                    db.SaveChanges();
                    string[] nombres = cliente.nombre.ToString().Split(' ');
                    Session["name"] = nombres[0];
                    Session["usr"] = cliente.nombre;

                    if (Session["CrearOrden"] != null)
                    {
                        if (Session["CrearOrden"].Equals("pend"))
                        {
                            return RedirectToAction("CrearOrden", "Pago");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("Invalida");
                }

            }

            else
            {
                return RedirectToAction("Invalida");
            }
            return View(cliente);
        }
        private bool validaPago(string nombre, string calle_t, string colonia_t, string estado_t, string num_tarj_cred_principal, string mes, string anio, string cVV)
        {
            bool retona = true;
            return retona;
        }

        private bool Tarjeta(string tarj, string tipo, string mes, string anio, string cvv)
        {
            bool retorna = validaTarj(tarj);
            if (retorna)
            {
                if (tarj.StartsWith("4") && (tipo.Equals("Visa")))
                {
                    retorna = true;
                }
                else
                {
                    if (tarj.StartsWith("5") && (tipo.Equals("Masterd")))
                    {
                        retorna = true;
                    }
                    else
                    {
                        if (tarj.StartsWith("3") && (tipo.Equals("American")))
                        {
                            retorna = true;
                        }
                        else
                        {
                            retorna = false;
                        }
                    }
                }
                DateTime hoy = new DateTime();
                if (Convert.ToInt32(anio) >= hoy.Year)
                {
                    if (Convert.ToInt32(mes) > hoy.Month)
                    {
                        retorna = true;
                    }
                    else
                    {
                        retorna = false;
                    }

                }
                else
                {
                    retorna = false;
                }
            }
            return retorna;
        }

        private bool validaTarj(string tarj)
        {
            bool retorna = true;
            StringBuilder digitsOnly = new StringBuilder();
            foreach (Char c in tarj)
            {
                if (Char.IsDigit(c)) digitsOnly.Append(c);
            };
            if (digitsOnly.Length > 18 || digitsOnly.Length < 12) return false;
            int sum = 0;
            int digit = 0;
            int addend = 0;
            bool timesTwo = false;

            for (int i = digitsOnly.Length - 1; i >= 0; i--)
            {
                digit = Int32.Parse(digitsOnly.ToString(i, 1));
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9)
                        addend -= 9;
                }
                else
                    addend = digit;

                sum += addend;
                timesTwo = !timesTwo;
            }
            // retorna = ((sum % 10) == 0);
            return retorna;
        }

        //      if (ModelState.IsValid)
        //      {
        //         db.cliente.Add(cliente);
        //       db.SaveChanges();
        //       return RedirectToAction("Index");
        //      }

        //       return View(cliente);
        //    }



        public ActionResult Invalida()
        {
            return View();
        }

        public ActionResult BorrarUser()
        {
            string idUser = User.Identity.GetUserId();
            return RedirectToAction("Delete", "Account", routeValues: new { id = idUser });

        }



        // GET: clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_cliente,nombre,email,calle_t,colonia_t,estado_t,num_tarj_cred_principal")] cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cliente cliente = db.cliente.Find(id);
            db.cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
