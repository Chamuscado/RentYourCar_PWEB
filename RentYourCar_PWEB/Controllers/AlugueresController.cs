using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;

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
        
    }
}