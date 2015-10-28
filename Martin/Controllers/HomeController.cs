using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Security;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using Martin.Utility;
using Martin.ViewModel;
using FileStandard = System.IO.File;

namespace Martin.Controllers
{
    public class HomeController : Controller
    {
        public IAlbumRepository AlbumRepository { get; set; }

        public IStaticContentRepository StaticContentRepository { get; set; }

        public HomeController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
                StaticContentRepository = scope.Resolve<IStaticContentRepository>();
            }
        }

        public ActionResult Index(long? albumId)
        {
            if (!albumId.HasValue)
            {
                albumId = AlbumRepository.GetRandom().Id;
            }

            var albums = AlbumRepository.GetAll();
            var model = new AlbumsHomeViewModel
            {
                AlbumId = albumId.Value,
                Albums = albums
            };
            return View(model);
        }

        public ActionResult GetOneSlide(long albumId)
        {
            var albums = AlbumRepository.GetAll();
            var model = new AlbumsHomeViewModel
            {
                AlbumId = albumId,
                Albums = albums
            };

            var viewToString = RenderRazorViewToString("OneSlide", model);
            return new JsonResult
            {
                Data = viewToString, 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult About()
        {
            var model = StaticContentRepository.Get(TypeStaticContent.About);
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string result)
        {
            FormsAuthentication.SetAuthCookie(result, false);
            return RedirectToAction("Index", "Album");
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

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
