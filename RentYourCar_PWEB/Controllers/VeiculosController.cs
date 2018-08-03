using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentYourCar_PWEB.Models;

namespace RentYourCar_PWEB.Controllers
{
    public class VeiculosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Veiculos
        public ActionResult Index()
        {
            return View(db.Veiculoes.ToList());
        }

        // GET: Veiculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculo veiculo = db.Veiculoes.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }
            return View(veiculo);
        }

        // GET: Veiculos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Modelo,Marca,Lutacao,NPortas,PrecoDiario,PrecoMensal,Aprovado,CondicoesArrendamento")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                db.Veiculoes.Add(veiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculo veiculo = db.Veiculoes.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Modelo,Marca,Lutacao,NPortas,PrecoDiario,PrecoMensal,Aprovado,CondicoesArrendamento")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(veiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculo veiculo = db.Veiculoes.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }
            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Veiculo veiculo = db.Veiculoes.Find(id);
            db.Veiculoes.Remove(veiculo);
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
