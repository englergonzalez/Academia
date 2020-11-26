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
    [Authorize]//necesita autorizacion (iniciar sesion)
    public class EstudiantesController : Controller
    {
        private AcademiaEntities db = new AcademiaEntities();

        // GET: Estudiantes
        [AllowAnonymous]//Puede listar sin iniciar sesion
        public ActionResult Index()
        {
            var estudiante = db.Estudiante.Include(e => e.TipoSangre);
            return View(estudiante.ToList());
        }

        // GET: Estudiantes/Details/5
        [AllowAnonymous]//Puede ver detalles sin iniciar sesion
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }
        // GET: Estudiantes/Details2?name=engler&eshombre=true
        [AllowAnonymous]
        public ActionResult Details2(string name, bool? eshombre)
        {
            if ((name == null && name == "") || eshombre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Where(e => e.eshombre == eshombre || e.nombre == name).FirstOrDefault();

            return View("Details", estudiante);


        }

        // GET: Estudiantes/Create
        public ActionResult Create()
        {
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre");
            return View();
        }

        // POST: Estudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,fechanacimiento,promedionotas,eshombre,id_tipoSangre,direccion,celular")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Estudiante.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre", estudiante.id_tipoSangre);
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre", estudiante.id_tipoSangre);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,fechanacimiento,promedionotas,eshombre,id_tipoSangre,direccion,celular")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_tipoSangre = new SelectList(db.TipoSangre, "id", "nombre", estudiante.id_tipoSangre);
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudiante estudiante = db.Estudiante.Find(id);
            db.Estudiante.Remove(estudiante);
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
