using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;
using RentYourCar_PWEB.Models.VeiculosView;

namespace RentYourCar_PWEB.Controllers
{
    public class VeiculosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Veiculos
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var veiculos = new List<Veiculo>(db.Veiculos.Where(v => v.UserId == userId));

            var detailsVeiculos = new List<DetailsVeiculoViewModel>(veiculos.Count);
            foreach (var veiculo in veiculos)
            {
                detailsVeiculos.Add(new DetailsVeiculoViewModel()
                {
                    Categoria = db.Categorias.First(c => c.Id == veiculo.Categoria_id).Nome,
                    Combustivel = db.Combustiveis.First(c => c.Id == veiculo.Combustivel_id).Nome,
                    Veiculo = veiculo
                });
            }


            return View(detailsVeiculos);
        }

        // GET: Veiculos/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null ||
                string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0)
            {
                return HttpNotFound();
            }

            var detailsVeiculo = new DetailsVeiculoViewModel()
            {
                Categoria = db.Categorias.First(c => c.Id == veiculo.Categoria_id).Nome,
                Combustivel = db.Combustiveis.First(c => c.Id == veiculo.Combustivel_id).Nome,
                Veiculo = veiculo
            };


            return View(detailsVeiculo);
        }

        // GET: Veiculos/Create
        [Authorize]
        public ActionResult Create()
        {
            var combustiveis = db.Combustiveis.ToList();
            var categorias = db.Categorias.ToList();
            var createVeiculo = new CreateVeiculoViewModel()
            {
                Combustivels = combustiveis,
                Categorias = categorias
            };
            return View(createVeiculo);
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                veiculo.UserId = User.Identity.GetUserId();
                veiculo.Matricula = veiculo.Matricula.ToUpper();
                db.Veiculos.Add(veiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var combustiveis = db.Combustiveis.ToList();
            var categorias = db.Categorias.ToList();
            var createVeiculo = new CreateVeiculoViewModel()
            {
                Combustivels = combustiveis,
                Categorias = categorias,
                Veiculo = veiculo
            };

            return View(createVeiculo);
        }

        // GET: Veiculos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null ||
                string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0)
            {
                return HttpNotFound();
            }

            var combustiveis = db.Combustiveis.ToList();
            var categorias = db.Categorias.ToList();
            var createVeiculo = new CreateVeiculoViewModel()
            {
                Combustivels = combustiveis,
                Categorias = categorias,
                Veiculo = veiculo
            };


            return View(createVeiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Veiculo veiculo)
        {
            if (ModelState.IsValid ||
                string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0)
            {
                try
                {
                    veiculo.Matricula = veiculo.Matricula.ToUpper();
                    db.Entry(veiculo).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return RedirectToAction("Index");
            }

            var combustiveis = db.Combustiveis.ToList();
            var categorias = db.Categorias.ToList();
            var createVeiculo = new CreateVeiculoViewModel()
            {
                Combustivels = combustiveis,
                Categorias = categorias,
                Veiculo = veiculo
            };

            return View(createVeiculo);
        }

        // GET: Veiculos/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null ||
                string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0)
            {
                return HttpNotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Veiculo veiculo = db.Veiculos.Find(id);
            db.Veiculos.Remove(veiculo);
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