using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;

namespace RentYourCar_PWEB.Controllers
{
    public class AvaliacaoFornecedorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvaliacaoFornecedors
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Index()
        {
            var avaliacoesFornecedores = db.AvaliacoesFornecedores
                .Include(a => a.Aluguer)
                .Include(a => a.Aluguer.Veiculo)
                .Include(a => a.Aluguer.Cliente)
                .Include(a => a.Aluguer.Veiculo.User);

            return View(avaliacoesFornecedores.ToList());
        }

        [Authorize(Roles = RoleNames.Particular)]
        public ActionResult MinhasAvaliacoes() //Retorna avaliações de veículos feitas pelo utilizador
        {
            var userId = User.Identity.GetUserId();

            var listaAvaliacoes = db.AvaliacoesFornecedores
                .Include(a => a.Aluguer)
                .Include(a => a.Aluguer.Veiculo)
                .Include(a => a.Aluguer.Cliente)
                .Include(a => a.Aluguer.Veiculo.User)
                .Where(a => string.Compare(userId, a.Aluguer.ClienteId, StringComparison.Ordinal) == 0)
                .ToList();

            return View("Index", listaAvaliacoes);
        }

        // GET: AvaliacaoFornecedors/Create
        [Authorize(Roles = RoleNames.Particular)]
        public ActionResult Create(int? aluguerId)
        {
            if (aluguerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aluguer = db.Alugueres.Include(a => a.AluguerState)
                .Include(a => a.Veiculo)
                .Include(a => a.AvaliacaoFornecedor)
                .Include(a => a.Veiculo.User)
                .SingleOrDefault(a => a.Id == aluguerId);
            if (aluguer == null)
            {
                return HttpNotFound();
            }


            if (aluguer.AvaliacaoFornecedor != null)
                return Edit(aluguer.AvaliacaoFornecedor);

            if (aluguer.Fim < DateTime.Today.AddMonths(-1) && aluguer.Fim > DateTime.Today)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Já não é possivel altera a Avaliação");

            return View(new AvaliacaoFornecedor() {Aluguer = aluguer, AluguerId = aluguer.Id});
        }

        // POST: AvaliacaoFornecedors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AluguerId,Comentario,Simpatia,Rapidez")]
            AvaliacaoFornecedor avaliacaoFornecedor)
        {
            if (ModelState.IsValid)
            {
                db.AvaliacoesFornecedores.Add(avaliacaoFornecedor);
                db.SaveChanges();
                return RedirectToAction("Details", "Alugueres", new {id = avaliacaoFornecedor.AluguerId});
            }

            avaliacaoFornecedor.Aluguer = db.Alugueres.Include(a => a.AluguerState)
                .Include(a => a.Veiculo)
                .Include(a => a.AvaliacaoVeiculo)
                .Include(a => a.Veiculo.User)
                .SingleOrDefault(a => a.Id == avaliacaoFornecedor.AluguerId);

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoFornecedor.AluguerId);
            return View(avaliacaoFornecedor);
        }

        // GET: AvaliacaoFornecedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var avaliacaoFornecedor = db.AvaliacoesFornecedores
                .Include(a => a.Aluguer)
                .Include(a => a.Aluguer.Veiculo)
                .Include(a => a.Aluguer.Cliente)
                .Include(a=>a.Aluguer.Veiculo.User)
                .SingleOrDefault(a => a.AluguerId == id);

            if (avaliacaoFornecedor == null)
            {
                return HttpNotFound();
            }


            var clienteId = User.Identity.GetUserId();


            if (avaliacaoFornecedor.Aluguer.Fim < DateTime.Today.AddMonths(-1) &&
                avaliacaoFornecedor.Aluguer.Fim > DateTime.Today)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Já não é possivel altera a Avaliação");

            if (string.Compare(clienteId, avaliacaoFornecedor.Aluguer.ClienteId, StringComparison.Ordinal) != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Operação não autorizada.");
            }

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoFornecedor.AluguerId);
            return View(avaliacaoFornecedor);
        }

        // POST: AvaliacaoFornecedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AluguerId,Comentario,Simpatia,Rapidez")]
            AvaliacaoFornecedor avaliacaoFornecedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliacaoFornecedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Alugueres", new {id = avaliacaoFornecedor.AluguerId});
            }

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoFornecedor.AluguerId);
            return View(avaliacaoFornecedor);
        }

        // GET: AvaliacaoFornecedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AvaliacaoFornecedor avaliacaoFornecedor = db.AvaliacoesFornecedores.Find(id);
            if (avaliacaoFornecedor == null)
            {
                return HttpNotFound();
            }

            db.AvaliacoesFornecedores.Remove(avaliacaoFornecedor);
            db.SaveChanges();
            return RedirectToAction("Details", "Alugueres", new {id});
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