using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using Martin.Utility;
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
            var mp3Paths = album.Songs.Select(x => FileHelper.PathToSong(album.Name, x.Mp3FileName));
            var zipPath = Path.Combine(Server.MapPath("~/Content/Song"), album.Name + ".zip");
            if (FileStandard.Exists(zipPath))
            {
                FileStandard.Delete(zipPath);
            }

            var zipFile = ZipFile.Create(zipPath);
            zipFile.BeginUpdate();
            foreach (var mp3PathLocal in mp3Paths)
            {
                var globalMp3Path = Server.MapPath("~/" + mp3PathLocal);
                var dataSource = new StaticDiskDataSource(globalMp3Path);
                zipFile.Add(dataSource, Path.GetFileName(globalMp3Path));
            }
            zipFile.CommitUpdate();
            zipFile.Close();

            var fileBytes = FileStandard.ReadAllBytes(zipPath);
            var fileName = Path.GetFileName(zipPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
        }
    }
}
