using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Academia.Models;
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

        public JsonResult NotasAcademia()
        {
            var q = db.Nota.GroupBy(m => new { m.Estudiante, m.Asignatura });
            var p = db.Asignatura.OrderBy(m => new { m.id });

            List<NotasEstudiante> estudiantes = new List<NotasEstudiante>();

            List<String> est = new List<string>();
            List<String> asig = new List<string>();

            NotasEstudiante notasEstudiante = new NotasEstudiante();

            foreach (var item in q)
            { 
                if (!asig.Contains(item.Key.Asignatura.nombre))
                {
                    asig.Add(item.Key.Asignatura.nombre);
                }
            }
            foreach (var item in q)
            {
                String nombreEstudiante = item.Key.Estudiante.nombre;
                String nombreAsignatura = item.Key.Asignatura.nombre;
                double notaCero = 0;

                for (int i = 0; i < asig.Count; i++)
                {
                    
                    if ( asig.ElementAt(i).Equals(nombreAsignatura) && !est.Contains(nombreEstudiante) )
                    {
                        notasEstudiante = new NotasEstudiante() { name = nombreEstudiante, data = new List<double>() };

                        notasEstudiante.data.Add(item.Average(m => m.nota1).Value);
                        est.Add(nombreEstudiante);
                        estudiantes.Add(notasEstudiante);
                    }
                    else if (!asig.ElementAt(i).Equals(nombreAsignatura) && !est.Contains(nombreEstudiante))
                    {
                        notasEstudiante = new NotasEstudiante() { name = nombreEstudiante, data = new List<double>() };
                        notasEstudiante.data.Add(notaCero);

                        est.Add(nombreEstudiante);
                        estudiantes.Add(notasEstudiante);
                    }
                    else if( asig.ElementAt(i).Equals(nombreAsignatura) && est.Contains(nombreEstudiante) )
                    {
                        notasEstudiante.data.Add(item.Average(m => m.nota1).Value);
                        try
                        {
                            notasEstudiante.data.RemoveAll(x => x == 0);
                        }
                        catch { }
                    }
                    else if( !asig.ElementAt(i).Equals(nombreAsignatura) && est.Contains(nombreEstudiante) )
                    {
                        notasEstudiante.data.Add(notaCero);
                    }
                }
            }
            return Json(estudiantes, JsonRequestBehavior.AllowGet);
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
