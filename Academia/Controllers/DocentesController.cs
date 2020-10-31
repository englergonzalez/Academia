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
    [Authorize]//obliga a iniciar sesion 
    public class DocentesController : Controller
    {
        private AcademiaEntities db = new AcademiaEntities();

        // GET: Docentes
        [AllowAnonymous]//Puede listar sin iniciar sesion
        public ActionResult Index()
        {
            var docente = db.Docente.Include(d => d.Dedicacion).Include(d => d.TipoSangre);
            return View(docente.ToList());
        }

        // GET: Docentes/Details/5
        [AllowAnonymous]//Puede ver detalles sin iniciar sesion
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Docente docente = db.Docente.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);
        }

        // GET: Docentes/Create
        public ActionResult Create()
        {
            ViewBag.id_dedicacion = new SelectList(db.Dedicacion, "id", "descripcion");
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre");
            return View();
        }

        // POST: Docentes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,Salario,eshombre,id_dedicacion,especialidad,fechanacimiento,id_tipoSangre")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                db.Docente.Add(docente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_dedicacion = new SelectList(db.Dedicacion, "id", "descripcion", docente.id_dedicacion);
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre", docente.id_tipoSangre);
            return View(docente);
        }

        // GET: Docentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Docente docente = db.Docente.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_dedicacion = new SelectList(db.Dedicacion, "id", "descripcion", docente.id_dedicacion);
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre", docente.id_tipoSangre);
            return View(docente);
        }

        // POST: Docentes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,Salario,eshombre,id_dedicacion,especialidad,fechanacimiento,id_tipoSangre")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_dedicacion = new SelectList(db.Dedicacion, "id", "descripcion", docente.id_dedicacion);
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre", docente.id_tipoSangre);
            return View(docente);
        }

        // GET: Docentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Docente docente = db.Docente.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);
        }

        // POST: Docentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Docente docente = db.Docente.Find(id);
            db.Docente.Remove(docente);
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
