using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;
using RentYourCar_PWEB.Models.AlugueresViewModels;

namespace RentYourCar_PWEB.Controllers
{
    public class AlugueresController : Controller
    {
        private ApplicationDbContext _context;

        public AlugueresController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);
        }

        // GET: Alugueres
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Index()
        {
            var listaAlugueres = _context.Alugueres.ToList();

            return View(listaAlugueres);
        }

        [Authorize(Roles = RoleNames.Particular)]
        public ActionResult Create(int? veiculoId)
        {
            if (veiculoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var veiculo = _context.Veiculos.SingleOrDefault(v => v.Id == veiculoId);
            if (veiculo == null)
            {
                return HttpNotFound();
            }

            var clienteId = User.Identity.GetUserId();
            
            var viewModel = new CreateAluguerViewModel
            {
                VeiculoId = (int)veiculoId,
                ClienteId = clienteId,
                Inicio = DateTime.Today,
                Fim = DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Particular)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAluguerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = new CreateAluguerViewModel
                {
                    Inicio = DateTime.Today,
                    Fim = DateTime.Today
                };
                return View(viewModel);
            }

            try
            {
                var cliente = _context.Users.Single(c =>
                    string.Compare(c.Id, viewModel.ClienteId, StringComparison.Ordinal) == 0);
                var veiculo = _context.Veiculos.Single(v => v.Id == viewModel.VeiculoId);

                var aluguer = new Aluguer
                {
                    VeiculoId = viewModel.VeiculoId,
                    Veiculo = veiculo,
                    ClienteId = viewModel.ClienteId,
                    Cliente = cliente,
                    Inicio = viewModel.Inicio,
                    Fim = viewModel.Fim
                };

                _context.Alugueres.Add(aluguer);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }

            return RedirectToAction("ListaVeiculos", "Veiculos");
        }


    }
}