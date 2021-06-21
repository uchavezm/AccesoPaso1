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
    public class productoController : Controller
    {
        private contextTienda db = new contextTienda();

        // GET: producto
        public ActionResult Index()
        {
            var producto = db.producto.Include(p => p.categoria);
            return View(producto.ToList());
        }

        // GET: producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: producto/Create
        public ActionResult Create()
        {
            ViewBag.id_categoria = new SelectList(db.categoria, "Id_categoria", "nombre");
            return View();
        }

        // POST: producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_producto,nombre,descripcion,precio,imagen,existencia,stock,id_categoria")] producto producto)
        {
            if (ModelState.IsValid)
            {
                db.producto.Add(producto);
                db.SaveChanges();
                int id = producto.Id_producto;
                var prod = db.producto.Find(id);
                DateTime hoy = DateTime.Now;
                prod.ult_actualización = hoy;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_categoria = new SelectList(db.categoria, "Id_categoria", "nombre", producto.id_categoria);
            return View(producto);
        }

        // GET: producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.categoria, "Id_categoria", "nombre", producto.id_categoria);
            return View(producto);
        }

        // POST: producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_producto,nombre,descripcion,precio,imagen,existencia,stock,id_categoria")] producto producto)
        {
            if (ModelState.IsValid)
            {
                int id = producto.Id_producto;
                var prod = db.producto.Find(id);
                decimal precio_ant = prod.precio;
                decimal precio_act = producto.precio;

                //db.Entry(producto).State = EntityState.Modified;
                prod.nombre = producto.nombre;
                prod.descripcion = producto.descripcion;
                prod.precio = producto.precio;
                prod.imagen = producto.imagen;
                prod.existencia = producto.existencia;
                prod.stock = producto.stock;
                prod.id_categoria = producto.id_categoria;
                if (precio_act != precio_ant)
                {
                    DateTime hoy = DateTime.Now;
                    prod.ult_actualización = hoy;
                }
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.id_categoria = new SelectList(db.categoria, "Id_categoria", "nombre", producto.id_categoria);
            return View(producto);
        }

        // GET: producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            producto producto = db.producto.Find(id);
            db.producto.Remove(producto);
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
