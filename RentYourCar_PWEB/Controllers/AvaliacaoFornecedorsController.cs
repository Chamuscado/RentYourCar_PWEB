using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RentYourCar_PWEB.Models;

namespace RentYourCar_PWEB.Controllers
{
    public class AvaliacaoFornecedorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AvaliacaoFornecedors
        public ActionResult Index()
        {
            var avaliacoesFornecedores = db.AvaliacoesFornecedores.Include(a => a.Aluguer);
            return View(avaliacoesFornecedores.ToList());
        }

        // GET: AvaliacaoFornecedors/Create
        public ActionResult Create()
        {
            ViewBag.AluguerId = new SelectList(db.Alugueres, "Id", "ClienteId");
            return View();
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
                return RedirectToAction("Index");
            }

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

            AvaliacaoFornecedor avaliacaoFornecedor = db.AvaliacoesFornecedores.Find(id);
            if (avaliacaoFornecedor == null)
            {
                return HttpNotFound();
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
                return RedirectToAction("Index");
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

            return View(avaliacaoFornecedor);
        }

        // POST: AvaliacaoFornecedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AvaliacaoFornecedor avaliacaoFornecedor = db.AvaliacoesFornecedores.Find(id);
            db.AvaliacoesFornecedores.Remove(avaliacaoFornecedor);
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