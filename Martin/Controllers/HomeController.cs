using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using FileStandard = System.IO.File;

namespace Martin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public IAlbumRepository AlbumRepository { get; set; }

        public HomeController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
            }
        }

        public ActionResult Index()
        {
            var model = AlbumRepository.GetAll();
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public FileResult GetAlbumInArchive(long idAlbum)
        {
            var album = AlbumRepository.Get(idAlbum);

            var path = Path.Combine(Server.MapPath("~/Content/Song"), album.Name);

            var zipPath = Path.Combine(Server.MapPath("~/Content/Song"), album.Name + ".zip");
            if (FileStandard.Exists(zipPath))
            {
                FileStandard.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(path, zipPath);

            var fileBytes = FileStandard.ReadAllBytes(zipPath);
            var fileName = Path.GetFileName(zipPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
        }
    }
}
