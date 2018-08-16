using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;

namespace RentYourCar_PWEB.Controllers
{
    public class AvaliacaoVeiculoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvaliacaoVeiculoes
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Index()
        {
            var avaliacoesVeiculos = db.AvaliacoesVeiculos.Include(a => a.Aluguer);
            return View(avaliacoesVeiculos.ToList());
        }

        // GET: AvaliacaoVeiculoes/Create
        public ActionResult Create()
        {
            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId");
            return View();
        }

        [Authorize]
        public ActionResult ClassificacaoAluguer(int? aluguerId)
        {
            if (aluguerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var listaClassificaoes = db.AvaliacoesVeiculos.Include(a => a.Aluguer).Where(i => i.AluguerId == aluguerId)
                .ToList();

            return View("Index", listaClassificaoes);
        }

        public ActionResult Create(int? aluguerId)
        {
            if (aluguerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aluguer = db.Alugueres.Include(a => a.AluguerState)
                .Include(a => a.Veiculo)
                .Include(a => a.AvaliacaoVeiculo)
                .SingleOrDefault(a => a.Id == aluguerId);
            if (aluguer == null)
            {
                return HttpNotFound();
            }

            if (aluguer.AvaliacaoVeiculo != null)
                return Edit(aluguer.AvaliacaoVeiculo);


            return View(new AvaliacaoVeiculo() {Aluguer = aluguer, AluguerId = aluguer.Id});
        }

        // POST: AvaliacaoVeiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AluguerId,Comentario,Limpeza,Consumo,Apresentacao")]
            AvaliacaoVeiculo avaliacaoVeiculo)
        {
            if (ModelState.IsValid)
            {
                db.AvaliacoesVeiculos.Add(avaliacaoVeiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoVeiculo.AluguerId);
            return View(avaliacaoVeiculo);
        }

        // GET: AvaliacaoVeiculoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var avaliacao = db.AvaliacoesVeiculos.Include(a => a.Aluguer)
                .SingleOrDefault(a => a.AluguerId == id);

            if (avaliacao == null)
            {
                return HttpNotFound();
            }

            var clienteId = User.Identity.GetUserId();

            if (avaliacao.Aluguer.Fim > DateTime.Today.AddMonths(-1) && avaliacao.Aluguer.Fim < DateTime.Today)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Já não é possivel altera a Avaliação");

            if (string.Compare(clienteId, avaliacao.Aluguer.ClienteId, StringComparison.Ordinal) == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Operação não autorizada.");
            }


            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacao.AluguerId);

            return View(avaliacao);
        }

        // POST: AvaliacaoVeiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AluguerId,Comentario,Limpeza,Consumo,Apresentacao")]
            AvaliacaoVeiculo avaliacaoVeiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliacaoVeiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId", avaliacaoVeiculo.AluguerId);
            return View(avaliacaoVeiculo);
        }

        // GET: AvaliacaoVeiculoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AvaliacaoVeiculo avaliacaoVeiculo = db.AvaliacoesVeiculos.Find(id);
            if (avaliacaoVeiculo == null)
            {
                return HttpNotFound();
            }

            return View(avaliacaoVeiculo);
        }

        // POST: AvaliacaoVeiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AvaliacaoVeiculo avaliacaoVeiculo = db.AvaliacoesVeiculos.Find(id);
            db.AvaliacoesVeiculos.Remove(avaliacaoVeiculo);
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