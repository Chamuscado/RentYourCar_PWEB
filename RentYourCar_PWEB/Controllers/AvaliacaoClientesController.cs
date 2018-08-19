using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;

namespace RentYourCar_PWEB.Controllers
{
    public class AvaliacaoClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvaliacaoClientes
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Index()
        {
            var avaliacoesClientes = db.AvaliacoesClientes
                .Include(a => a.Aluguer)
                .Include(a => a.Aluguer.Veiculo)
                .Include(a => a.Aluguer.Cliente)
                .Include(a=>a.Aluguer.Veiculo.User);
            return View(avaliacoesClientes.ToList());
        }

        [Authorize(Roles = RoleNames.Particular + "," + RoleNames.Profissional)]
        public ActionResult MinhasAvaliacoes() //Retorna avaliações de veículos feitas pelo utilizador
        {
            var userId = User.Identity.GetUserId();

            var listaAvaliacoes = db.AvaliacoesClientes
                .Include(a => a.Aluguer)
                .Include(a => a.Aluguer.Veiculo)
                .Include(a => a.Aluguer.Cliente)
                .Include(a=>a.Aluguer.Veiculo.User)
                .Where(a => string.Compare(userId, a.Aluguer.Veiculo.UserId, StringComparison.Ordinal) == 0)
                .ToList();

            return View("Index", listaAvaliacoes);
        }

        // GET: AvaliacaoClientes/Create
        [Authorize(Roles = RoleNames.Particular + "," + RoleNames.Profissional)]
        public ActionResult Create(int? aluguerId)
        {
            if (aluguerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aluguer = db.Alugueres.Include(a => a.AluguerState)
                .Include(a => a.Veiculo)
                .Include(a => a.Cliente)
                .SingleOrDefault(a => a.Id == aluguerId);
            if (aluguer == null)
            {
                return HttpNotFound();
            }


            if (aluguer.AvaliacaoCliente != null)
                return Edit(aluguer.AvaliacaoCliente);

            if (aluguer.Fim < DateTime.Today.AddMonths(-1) && aluguer.Fim > DateTime.Today)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Já não é possivel altera a Avaliação");

            return View(new AvaliacaoCliente() { Aluguer = aluguer, AluguerId = aluguer.Id });

        }

        // POST: AvaliacaoClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AluguerId,Comentario,Limpeza,Cuidado,Pontualidade,Pagamento")]
            AvaliacaoCliente avaliacaoCliente)
        {
            if (ModelState.IsValid)
            {
                db.AvaliacoesClientes.Add(avaliacaoCliente);
                db.SaveChanges();
                return RedirectToAction("Details", "Alugueres", new { id = avaliacaoCliente.AluguerId });
            }

            avaliacaoCliente.Aluguer = db.Alugueres.Include(a => a.AluguerState)
                .Include(a => a.Veiculo)
                .Include(a => a.AvaliacaoVeiculo)
                .SingleOrDefault(a => a.Id == avaliacaoCliente.AluguerId);

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

            var avaliacaoCliente = db.AvaliacoesClientes
                .Include(a => a.Aluguer)
                .Include(a => a.Aluguer.Veiculo)
                .SingleOrDefault(a => a.AluguerId == id);
            
            if (avaliacaoCliente == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();

            if (avaliacaoCliente.Aluguer.Fim < DateTime.Today.AddMonths(-1) && avaliacaoCliente.Aluguer.Fim > DateTime.Today)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Já não é possivel altera a Avaliação");

            if (string.Compare(userId, avaliacaoCliente.Aluguer.Veiculo.UserId, StringComparison.Ordinal) != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Operação não autorizada.");
            }

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoCliente.AluguerId);
            return View(avaliacaoCliente);
        }

        // POST: AvaliacaoClientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AluguerId,Comentario,Limpeza,Cuidado,Pontualidade,Pagamento")]
            AvaliacaoCliente avaliacaoCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliacaoCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Alugueres", new { id = avaliacaoCliente.AluguerId });
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

            db.AvaliacoesClientes.Remove(avaliacaoCliente);
            db.SaveChanges();

            return RedirectToAction("Details", "Alugueres", new { id });
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