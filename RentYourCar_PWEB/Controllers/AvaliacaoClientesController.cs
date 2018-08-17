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
    public class AvaliacaoClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvaliacaoClientes
        public ActionResult Index()
        {
            var avaliacoesClientes = db.AvaliacoesClientes.Include(a => a.Aluguer);
            return View(avaliacoesClientes.ToList());
        }

        // GET: AvaliacaoClientes/Create
        public ActionResult Create()
        {
            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId");
            return View();
        }

        // POST: AvaliacaoClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AluguerId,Comentario,Limpeza,Cuidado,Pontualidade,Pagamento")] AvaliacaoCliente avaliacaoCliente)
        {
            if (ModelState.IsValid)
            {
                db.AvaliacoesClientes.Add(avaliacaoCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoCliente.AluguerId);
            return View(avaliacaoCliente);
        }

        // GET: AvaliacaoClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvaliacaoCliente avaliacaoCliente = db.AvaliacoesClientes.Find(id);
            if (avaliacaoCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoCliente.AluguerId);
            return View(avaliacaoCliente);
        }

        // POST: AvaliacaoClientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AluguerId,Comentario,Limpeza,Cuidado,Pontualidade,Pagamento")] AvaliacaoCliente avaliacaoCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliacaoCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoCliente.AluguerId);
            return View(avaliacaoCliente);
        }

        // GET: AvaliacaoClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvaliacaoCliente avaliacaoCliente = db.AvaliacoesClientes.Find(id);
            if (avaliacaoCliente == null)
            {
                return HttpNotFound();
            }
            return View(avaliacaoCliente);
        }

        // POST: AvaliacaoClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AvaliacaoCliente avaliacaoCliente = db.AvaliacoesClientes.Find(id);
            db.AvaliacoesClientes.Remove(avaliacaoCliente);
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
