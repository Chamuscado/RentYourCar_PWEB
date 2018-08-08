using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentYourCar_PWEB.Models;
using RentYourCar_PWEB.Models.UsersViewModels;

namespace RentYourCar_PWEB.Controllers
{
    public class UtilizadoresController : Controller
    {
        private ApplicationDbContext _context;

        public UtilizadoresController()
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

        // GET: Admin/GerirUtilizadores
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult GerirUtilizadores()
        {
            IList<ApplicationUser> userList = new List<ApplicationUser>();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            foreach (var user in _context.Users.ToList())
            {
                if (!userManager.IsInRole(user.Id, RoleNames.Admin))
                {
                    userList.Add(user);
                }
            }

            return View(userList);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Aprovar(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            if (user != null)
            {
                user.Aprovado = true;
                _context.SaveChanges();
            }

            return RedirectToAction("GerirUtilizadores");
        }

        [AllowAnonymous]
        public ActionResult Detalhes(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound("O utilizador não foi encontrado");
            }
            return View(user);
        }


        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Remover(string id)
        {
            var userToRemove = _context.Users.SingleOrDefault(u => u.Id == id);
            if (userToRemove == null)
            {
                return HttpNotFound("O utilizador que pretende remover não foi encontrado.");
            }

            _context.Users.Remove(userToRemove);
            _context.SaveChanges();

            return RedirectToAction("GerirUtilizadores");
        }

        public ActionResult Editar(string id)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = _context.Users.SingleOrDefault(u => u.Id == currentUserId);

            //Não permitir a edição do utilizador, a não ser pelo administrador ou pelo próprio utilizador
            if (currentUser == null || (!User.IsInRole(RoleNames.Admin) && currentUserId != id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Operação não autorizada.");
            }

            var userToEdit = _context.Users.SingleOrDefault(u => u.Id == id);
            if (userToEdit == null)
            {
                return HttpNotFound("O utilizador que pretende editar não foi encontrado");
            }

            var viewModel = new EditUserViewModel
            {
                Id = userToEdit.Id,
                Nome = userToEdit.Nome,
                Telefone = userToEdit.Telefone,
                Morada = userToEdit.Morada,
                Email = userToEdit.Email
            };

            return View("Editar", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(EditUserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Editar", userViewModel);
            }

            //var userInDb = _context.Users.Single(u => u.Id == userViewModel.Id);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userInDb = await userManager.FindByIdAsync(userViewModel.Id);

            userInDb.Nome = userViewModel.Nome;
            userInDb.Telefone = userViewModel.Telefone;
            userInDb.Morada = userInDb.Morada;
            userInDb.Email = userViewModel.Email;
            userInDb.UserName = userViewModel.Email;

            //_context.SaveChanges();
            await userManager.UpdateAsync(userInDb);

            if (User.IsInRole(RoleNames.Admin))
            {
                return RedirectToAction("GerirUtilizadores");
            }

            return RedirectToAction("Index", "Manage", User.Identity.GetUserId());
        }
    }
}