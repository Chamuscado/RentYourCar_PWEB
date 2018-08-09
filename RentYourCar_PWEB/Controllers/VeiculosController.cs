using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using RentYourCar_PWEB.Models;
using RentYourCar_PWEB.Models.VeiculosView;

namespace RentYourCar_PWEB.Controllers
{
    public class VeiculosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Veiculos
        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
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
                    Veiculo = veiculo,
                    Proprietario = db.Users.Single(u => string.Compare(userId, u.Id, StringComparison.Ordinal) == 0)
                        .Nome
                });
            }


            return View(detailsVeiculos);
        }

        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult GerirVeiculos()
        {
            var veiculos = new List<Veiculo>(db.Veiculos.ToList());

            var lista = new List<DetailsVeiculoViewModel>();

            foreach (var veiculo in veiculos)
            {
                var proprietario = db.Users.SingleOrDefault(u =>
                    string.Compare(veiculo.UserId, u.Id, StringComparison.Ordinal) == 0);
                var nomeProprietario = proprietario == null ? "(sem proprietário)" : proprietario.Nome;

                lista.Add(new DetailsVeiculoViewModel
                {
                    Categoria = db.Categorias.First(c => c.Id == veiculo.Categoria_id).Nome,
                    Combustivel = db.Combustiveis.First(c => c.Id == veiculo.Combustivel_id).Nome,
                    Veiculo = veiculo,
                    Proprietario = nomeProprietario
                });
            }

            return View(lista);
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
            if (
                veiculo == null /*|| string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0*/
            )
            {
                return HttpNotFound();
            }

            var proprietario = db.Users.SingleOrDefault(u =>
                string.Compare(veiculo.UserId, u.Id, StringComparison.Ordinal) == 0);
            var nomeProprietario = proprietario == null ? "(sem proprietário)" : proprietario.Nome;

            var detailsVeiculo = new DetailsVeiculoViewModel()
            {
                Categoria = db.Categorias.First(c => c.Id == veiculo.Categoria_id).Nome,
                Combustivel = db.Combustiveis.First(c => c.Id == veiculo.Combustivel_id).Nome,
                Veiculo = veiculo,
                Proprietario = nomeProprietario
            };


            return View(detailsVeiculo);
        }

        // GET: Veiculos/Create
        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
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
        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
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
                (string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0 &&
                 !User.IsInRole(RoleNames.Admin)))
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
            if (ModelState.IsValid &&
                (User.IsInRole(RoleNames.Admin) ||
                 string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) == 0))
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

                if (User.IsInRole(RoleNames.Admin))
                {
                    return RedirectToAction("GerirVeiculos");
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


        public PartialViewResult UploadFiles()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult UploadFiles(HttpPostedFileBase[] files)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {
                //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }
                }
            }

            return PartialView();
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
        } // POST: Veiculos/Delete/5


        public JsonResult ImageUpload(ProductViewModel model)
        {
            var file = model.ImageFile;

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);
                var relativeDir = $"/UploadedFiles/{User.Identity.GetUserId()}/Temp/";
                relativeDir = Server.MapPath(relativeDir);
                if (Directory.Exists(relativeDir))
                    relativeDir += Directory.GetFiles(relativeDir, "*").Length;
                else
                {
                    Directory.CreateDirectory(relativeDir);
                    relativeDir += "0";
                }

                relativeDir += "-" + fileName;
                file.SaveAs(relativeDir);
            }

            return Json(file.FileName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteImage(string fileName)
        {
            if (fileName != null && !fileName.IsEmpty())
            {
                var extention = Path.GetExtension(fileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(fileName);
                var relativeDir = $"/UploadedFiles/{User.Identity.GetUserId()}/Temp/";
                relativeDir = Server.MapPath(relativeDir);

                if (Directory.Exists(relativeDir))
                {
                    var fileList = Directory.GetFiles(relativeDir, "*");
                    foreach (var file in fileList)
                    {
                        var localFileName = Path.GetFileNameWithoutExtension(file);
                        var localfilenamewithoutextension = localFileName.Substring(localFileName.IndexOf('-') + 1);
                        if (string.Compare(filenamewithoutextension, localfilenamewithoutextension,
                                StringComparison.Ordinal) == 0)
                            System.IO.File.Delete(file);
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}