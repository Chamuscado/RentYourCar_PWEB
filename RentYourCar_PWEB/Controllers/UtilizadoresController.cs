using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentYourCar_PWEB.Models;

namespace RentYourCar_PWEB.Controllers
{
    public class UtilizadoresController : Controller
    {
        private ApplicationDbContext _context;

        public UtilizadoresController()
        {
            _context = new ApplicationDbContext();
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
    }
}