﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            var listaAlugueres = _context.Alugueres.Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .Include(a => a.Veiculo.User)
                .Include(a => a.AluguerState)
                .ToList();


            return View(listaAlugueres);
        }

        // GET: Alugueres

        [Authorize]
        public ActionResult AlugueresVeiculo(int? veiculoId)
        {
            if (veiculoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var listaAlugueres = _context.Alugueres.Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .Include(a => a.Veiculo.User)
                .Include(a => a.AluguerState)
                .Where(i => i.Veiculo.Id == veiculoId)
                .ToList();

            return View("Index", listaAlugueres);
        }


        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
        public ActionResult AlugueresFornecedor()
        {
            var userId = User.Identity.GetUserId();
            var listaAlugueres = _context.Alugueres
                .Where(a => string.Compare(a.Veiculo.UserId, userId, StringComparison.Ordinal) == 0)
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .Include(a => a.Veiculo.User)
                .Include(a => a.AluguerState)
                .ToList();


            return View("Index", listaAlugueres);
        }


        [Authorize(Roles = RoleNames.Particular)]
        public ActionResult AlugueresCliente()
        {
            var userId = User.Identity.GetUserId();
            var listaAlugueres = _context.Alugueres
                .Where(a => string.Compare(a.ClienteId, userId, StringComparison.Ordinal) == 0)
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .Include(a => a.Veiculo.User)
                .Include(a => a.AluguerState)
                .ToList();

            return View("Index", listaAlugueres);
        }


        [Authorize(Roles = RoleNames.Particular)]
        public ActionResult Create(int? veiculoId)
        {
            if (veiculoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var veiculo = _context.Veiculos.Include(v => v.User)
                .Include(v => v.Categoria)
                .Include(v => v.Combustivel)
                .Include(v => v.Alugueres)
                .SingleOrDefault(v => v.Id == veiculoId);
            if (veiculo == null)
            {
                return HttpNotFound();
            }

            var clienteId = User.Identity.GetUserId();

            if (!veiculo.Aprovado || string.Compare(clienteId, veiculo.UserId, StringComparison.Ordinal) == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Operação não autorizada.");
            }

            var viewModel = new CreateAluguerViewModel
            {
                VeiculoId = (int)veiculoId,
                Veiculo = veiculo,
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
            var veiculo = _context.Veiculos.Include(v => v.User)
                .Include(v => v.Alugueres)
                .Include(v => v.Categoria)
                .Include(v => v.Combustivel)
                .Single(v => v.Id == viewModel.VeiculoId);

            if (!ModelState.IsValid)
            {
                viewModel.Inicio = DateTime.Today;
                viewModel.Fim = DateTime.Today;
                viewModel.Veiculo = veiculo;

                return View(viewModel);
            }


            if (!VeiculoDisponivel(veiculo, viewModel.Inicio, viewModel.Fim))
            {
                ModelState.AddModelError("Fim", @"O veículo não está disponível no período indicado.");

                viewModel.Inicio = DateTime.Today;
                viewModel.Fim = DateTime.Today;
                viewModel.Veiculo = veiculo;

                return View(viewModel);
            }

            //try
            //{


            var cliente = _context.Users.Single(c =>
                string.Compare(c.Id, viewModel.ClienteId, StringComparison.Ordinal) == 0);
            //var veiculo = _context.Veiculos.Single(v => v.Id == viewModel.VeiculoId);

            var aluguer = new Aluguer
            {
                VeiculoId = viewModel.VeiculoId,
                Veiculo = veiculo,
                ClienteId = viewModel.ClienteId,
                Cliente = cliente,
                Inicio = viewModel.Inicio,
                Fim = viewModel.Fim,
                AluguerState_id = AluguerState.Pendente
            };

            _context.Alugueres.Add(aluguer);

            _context.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Debug.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
            //                ve.PropertyName,
            //                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
            //                ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}

            return RedirectToAction("Details", new { id = aluguer.Id });
        }

        public ActionResult Details(int id)
        {
            var aluguer = _context.Alugueres.Include(a => a.Veiculo)
                .Include(v => v.Veiculo.User)
                .Include(v => v.Veiculo.Categoria)
                .Include(v => v.Veiculo.Combustivel)
                .Include(a => a.Cliente)
                .SingleOrDefault(a => a.Id == id);

            if (aluguer == null)
            {
                return HttpNotFound();
            }

            return View(aluguer);
        }


        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
        public ActionResult AprovarAluguer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aluguer = _context.Alugueres
                .Include(a => a.Veiculo)
                .SingleOrDefault(a => a.Id == id);

            if (aluguer == null)
            {
                return HttpNotFound();
            }

            string userId = User.Identity.GetUserId();

            //impedir aprovação por um utilizador que não o fornecedor de serviço
            //e no caso de o estado ser diferente de "Pendente"
            if (string.Compare(aluguer.Veiculo.UserId, userId, StringComparison.Ordinal) != 0
                || aluguer.AluguerState_id != AluguerState.Pendente)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            aluguer.AluguerState_id = AluguerState.Aceite;

            _context.SaveChanges();

            RejeitaAlugueres(aluguer, aluguer.VeiculoId);

            return RedirectToAction("AlugueresFornecedor");
        }


        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
        public ActionResult RejeitarAluguer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aluguer = _context.Alugueres
                .Include(a => a.Veiculo)
                .SingleOrDefault(a => a.Id == id);

            if (aluguer == null)
            {
                return HttpNotFound();
            }

            string userId = User.Identity.GetUserId();

            //impedir rejeição por um utilizador que não o fornecedor de serviço
            //e no caso de o estado ser diferente de "Pendente"
            if (string.Compare(aluguer.Veiculo.UserId, userId, StringComparison.Ordinal) != 0
                || aluguer.AluguerState_id != AluguerState.Pendente)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            aluguer.AluguerState_id = AluguerState.Rejeitado;

            _context.SaveChanges();

            return RedirectToAction("AlugueresFornecedor");
        }

        //TODO: ações de remover, editar, aprovar/rejeitar.
        //TODO: associar estado ao aluguer (Pendente, Aceite, Rejeitado, Em Curso, Concluído).
        //TODO: Os estados Em Curso e Concluído devem ser geridos automaticamente pelo sistema.



        #region DateValidations

        private bool VeiculoDisponivel(Veiculo veiculo, DateTime inicioAluguer, DateTime fimAluguer, int aluguerId = -1)
        {
            if (veiculo == null)
            {
                Debug.WriteLine("veiculo = null");
                return false;
            }

            //Verificar se o veículo está disponível no período pretendido
            DateTime inicioDisponibilidade = veiculo.InicioDisponibilidade;
            DateTime fimDisponibilidade = veiculo.FimDisponibilidade;
            if (inicioAluguer < inicioDisponibilidade || fimAluguer > fimDisponibilidade)
            {
                return false;
            }

            //Verificar se não existem alugueres para o mesmo período
            var alugueresVeiculo = veiculo.Alugueres;
            foreach (var item in alugueresVeiculo)
            {
                //Ignorar o item atual, se corresponder ao aluguer que se pretende editar
                if (aluguerId != -1 && item.Id == aluguerId)
                {
                    continue;
                }

                //Só é preciso verificar os alugueres aceites ou em curso
                if (item.AluguerState_id != AluguerState.Aceite && item.AluguerState_id != AluguerState.EmCurso)
                {
                    continue;
                }

                //Coincidência num dos extremos do intervalo
                if (inicioAluguer == item.Inicio || fimAluguer == item.Fim)
                {
                    return false;
                }

                //Novo aluguer tem início dentro do período de um aluguer mais antigo
                if (inicioAluguer > item.Inicio && inicioAluguer < item.Fim)
                {
                    return false;
                }

                //Novo aluguer termina dentro do período de um aluguer mais antigo
                if (fimAluguer > item.Inicio && fimAluguer < item.Fim)
                {
                    return false;
                }

                //O período do novo aluguer engloba completamente o período de um aluguer mais antigo
                if (inicioAluguer < item.Inicio && fimAluguer > item.Fim)
                {
                    return false;
                }
            }

            return true;
        }

        private bool AlugueresSobrepostos(Aluguer aluguer1, Aluguer aluguer2)
        {
            //Coincidência num dos extremos do intervalo
            if (aluguer1.Inicio == aluguer2.Inicio || aluguer1.Fim == aluguer2.Fim)
            {
                return true;
            }

            //Novo aluguer tem início dentro do período de um aluguer mais antigo
            if (aluguer1.Inicio > aluguer2.Inicio && aluguer1.Inicio < aluguer2.Fim)
            {
                return true;
            }

            //Novo aluguer termina dentro do período de um aluguer mais antigo
            if (aluguer1.Fim > aluguer2.Inicio && aluguer1.Fim < aluguer2.Fim)
            {
                return true;
            }

            //O período do novo aluguer engloba completamente o período de um aluguer mais antigo
            if (aluguer1.Inicio < aluguer2.Inicio && aluguer1.Fim > aluguer2.Fim)
            {
                return true;
            }

            return false;
        }

        #endregion




        #region ControloAutomatico

        private void RejeitaAlugueres(Aluguer aluguer, int veiculoId)
        {
            var veiculo = _context.Veiculos
                .Include(v => v.Alugueres)
                .Single(v => v.Id == veiculoId);

            foreach (var item in veiculo.Alugueres)
            {
                if (item.AluguerState_id == AluguerState.Pendente)
                {
                    if (AlugueresSobrepostos(aluguer, item))
                    {
                        item.AluguerState_id = AluguerState.Rejeitado;
                    }
                }
            }

            _context.SaveChanges();
        }

        #endregion
    }
}