using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Academia.Models.db;

namespace Academia.Controllers
{
    public class NotasController : Controller
    {
        private AcademiaEntities db = new AcademiaEntities();

        // GET: Notas
        public ActionResult Index()
        {
            var nota = db.Nota.Include(n => n.Asignatura).Include(n => n.Estudiante);
            return View(nota.ToList());
        }

        // GET: Notas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Nota.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            return View(nota);
        }

        // GET: Notas/Create
        public ActionResult Create()
        {
            ViewBag.id_Asignatura = new SelectList(db.Asignatura, "id", "nombre");
            ViewBag.id_Estudiante = new SelectList(db.Estudiante, "id", "nombre");
            return View();
        }

        // POST: Notas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_Asignatura,id_Estudiante,actividad,descripcion,porcentaje,nota1")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                db.Nota.Add(nota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_Asignatura = new SelectList(db.Asignatura, "id", "nombre", nota.id_Asignatura);
            ViewBag.id_Estudiante = new SelectList(db.Estudiante, "id", "nombre", nota.id_Estudiante);
            return View(nota);
        }

        // GET: Notas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Nota.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Asignatura = new SelectList(db.Asignatura, "id", "nombre", nota.id_Asignatura);
            ViewBag.id_Estudiante = new SelectList(db.Estudiante, "id", "nombre", nota.id_Estudiante);
            return View(nota);
        }

        // POST: Notas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_Asignatura,id_Estudiante,actividad,descripcion,porcentaje,nota1")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Asignatura = new SelectList(db.Asignatura, "id", "nombre", nota.id_Asignatura);
            ViewBag.id_Estudiante = new SelectList(db.Estudiante, "id", "nombre", nota.id_Estudiante);
            return View(nota);
        }

        // GET: Notas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Nota.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            return View(nota);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nota nota = db.Nota.Find(id);
            db.Nota.Remove(nota);
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
