using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccesoPaso1.Models;

namespace AccesoPaso1.Controllers
{
    public class OrdenController : Controller
    {
        private contextTienda db = new contextTienda();

        // GET: Orden

        public ActionResult Index()
        {
            var orden = db.orden.Where(o => o.fecha_envio == null).OrderByDescending(o => o.fecha_creacion).Include(o => o.cliente).Include(o => o.Paqueteria)
                ;
            return View(orden.ToList());
        }
        public ActionResult Index1()
        {
            var orden = db.orden.Where(o => o.fecha_entrega == null && o.fecha_envio == null).OrderByDescending(o => o.fecha_creacion).Include(o => o.cliente).Include(o => o.Paqueteria)
                ;
            return View(orden.ToList());
        }


        // GET: Orden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orden orden = db.orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: Orden/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente = new SelectList(db.cliente, "Id_cliente", "nombre");
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "IdPaqueteria", "Nombre");
            return View();
        }

        // POST: Orden/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_orden,fecha_creacion,num_confirmacion,Total,id_cliente,id_dirEntrega,id_paqueteria,num_guia,fecha_envio,fecha_entrega")] orden orden)
        {
            if (ModelState.IsValid)
            {
                db.orden.Add(orden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente = new SelectList(db.cliente, "Id_cliente", "nombre", orden.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "IdPaqueteria", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // GET: Orden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orden orden = db.orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.cliente, "Id_cliente", "nombre", orden.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "IdPaqueteria", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_orden,id_paqueteria,fecha_entrega")] orden orden)
        {
            if (ModelState.IsValid)
            {
                orden o = db.orden.Find(orden.Id_orden);
                o.fecha_entrega = orden.fecha_entrega;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.cliente, "Id_cliente", "nombre", orden.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "IdPaqueteria", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // GET: Orden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orden orden = db.orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: Orden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            orden orden = db.orden.Find(id);
            db.orden.Remove(orden);
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
