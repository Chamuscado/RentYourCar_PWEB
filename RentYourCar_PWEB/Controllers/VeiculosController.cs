using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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

            return View("ListaTodosVeiculos", lista);
        }

        [AllowAnonymous]
        public ActionResult ListaVeiculos()
        {
            if (User.IsInRole(RoleNames.Admin))
            {
                return RedirectToAction("GerirVeiculos");
            }

            var veiculos = new List<Veiculo>(db.Veiculos.ToList());

            var lista = new List<DetailsVeiculoViewModel>();

            foreach (var veiculo in veiculos)
            {
                if (!User.Identity.IsAuthenticated ||
                    string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0)
                {
                    if (veiculo.Aprovado)
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
                }
            }

            return View("ListaTodosVeiculosReadOnly", lista);
        }

        // GET: Veiculos/Details/5
        [AllowAnonymous]
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
                Proprietario = nomeProprietario,
                imagesPaths = LoadImagesDetails(veiculo.UserId, veiculo.Id)
            };


            return View(detailsVeiculo);
        }

        // GET: Veiculos/Create
        [Authorize(Roles = RoleNames.Particular + ", " + RoleNames.Profissional)]
        public ActionResult Create()
        {
            ClearDir();
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

                var source = Server.MapPath($"/UploadedFiles/{User.Identity.GetUserId()}/Temp/");
                var dest = Server.MapPath($"/UploadedFiles/{User.Identity.GetUserId()}/{veiculo.Id}/");
                CopyFiles(source, dest);


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
            ClearDir();
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

            var dest = Server.MapPath($"/UploadedFiles/{User.Identity.GetUserId()}/Temp/");
            var source = Server.MapPath($"/UploadedFiles/{User.Identity.GetUserId()}/{veiculo.Id}/");
            CopyFiles(source, dest, false);

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

                    var source = Server.MapPath($"/UploadedFiles/{User.Identity.GetUserId()}/Temp/");
                    var dest = Server.MapPath($"/UploadedFiles/{User.Identity.GetUserId()}/{veiculo.Id}/");
                    ClearDir(dest);
                    MoveFiles(source, dest);
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

        // GET: Veiculos/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }

            if (!User.IsInRole(RoleNames.Admin) &&
                string.Compare(veiculo.UserId, User.Identity.GetUserId(), StringComparison.Ordinal) != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            //return View(veiculo);
            db.Veiculos.Remove(veiculo);
            db.SaveChanges();

            if (User.IsInRole(RoleNames.Admin))
            {
                return RedirectToAction("GerirVeiculos");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public ActionResult Aprovar(int id)
        {
            var veiculo = db.Veiculos.SingleOrDefault(u => u.Id == id);

            if (veiculo == null)
            {
                return HttpNotFound();
            }

            veiculo.Aprovado = true;
            db.SaveChanges();

            return RedirectToAction("GerirVeiculos");
        }

        // POST: Veiculos/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Veiculo veiculo = db.Veiculos.Find(id);
        //    db.Veiculos.Remove(veiculo);
        //    db.SaveChanges();

        //    if (User.IsInRole(RoleNames.Admin))
        //    {
        //        return RedirectToAction("GerirVeiculos");
        //    }

        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        #region JsonResult

        public JsonResult ImageUpload(ImageViewModel model)
        {
            var file = model.ImageFile;
            var fileNameToReturn = "";
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
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
                fileNameToReturn = relativeDir;


                file.SaveAs(relativeDir);
            }

            return Json(fileNameToReturn, JsonRequestBehavior.AllowGet);
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
                        var localfilenamewithoutextension = Path.GetFileNameWithoutExtension(file);
                        if (string.Compare(filenamewithoutextension, localfilenamewithoutextension,
                                StringComparison.Ordinal) == 0)
                            System.IO.File.Delete(file);
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ReloadImages()
        {
            var imageList = new List<string>();

            var relativeDir = $"/UploadedFiles/{User.Identity.GetUserId()}/Temp/";

            var absuluteDir = Server.MapPath(relativeDir);
            absuluteDir = absuluteDir.Replace("\\", "/");
            var intialParh = absuluteDir.Replace(relativeDir, "");

            if (Directory.Exists(absuluteDir))
            {
                var fileList = Directory.GetFiles(absuluteDir, "*");
                foreach (var file in fileList)
                {
                    imageList.Add(file.Replace(intialParh, ""));
                }
            }


            return Json(imageList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region privateMethods

        private List<string> LoadImagesDetails(string userid, int veiculoId)
        {
            var imageList = new List<string>();

            var relativeDir = $"/UploadedFiles/{userid}/{veiculoId}/";

            var absuluteDir = Server.MapPath(relativeDir);
            absuluteDir = absuluteDir.Replace("\\", "/");
            var intialParh = absuluteDir.Replace(relativeDir, "");

            if (Directory.Exists(absuluteDir))
            {
                var fileList = Directory.GetFiles(absuluteDir, "*");
                foreach (var file in fileList)
                {
                    imageList.Add(file.Replace(intialParh, ""));
                }
            }


            return imageList;
        }


        private void CopyFiles(string source, string desti, bool rename = true)
        {
            if (!Directory.Exists(desti))
                Directory.CreateDirectory(desti);

            if (Directory.Exists(source))
            {
                var fileListSource = Directory.GetFiles(source, "*");
                var id = 0;
                foreach (var file in fileListSource)
                {
                    var destpath = "";
                    if (rename)
                        destpath = desti + $"/{id++}" + Path.GetExtension(file);
                    else
                        destpath = desti + "/" + Path.GetFileName(file);
                    System.IO.File.Copy(file, destpath);
                }
            }
        }

        private void MoveFiles(string source, string desti, bool rename = true)
        {
            if (!Directory.Exists(desti))
                Directory.CreateDirectory(desti);

            if (Directory.Exists(source))
            {
                var fileListSource = Directory.GetFiles(source, "*");
                var id = 0;
                foreach (var file in fileListSource)
                {
                    var destpath = "";
                    if (rename)
                        destpath = desti + $"/{id++}" + Path.GetExtension(file);
                    else
                        destpath = desti + "/" + Path.GetFileName(file);
                    System.IO.File.Move(file, destpath);
                }
            }
        }

        private void ClearDir(string dir = "")
        {
            if (dir.IsEmpty())
            {
                dir = $"/UploadedFiles/{User.Identity.GetUserId()}/Temp/";
                dir = Server.MapPath(dir);
            }

            if (Directory.Exists(dir))
            {
                var di = new DirectoryInfo(dir);
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        #endregion
    }
}