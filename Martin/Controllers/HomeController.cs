﻿using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using ICSharpCode.SharpZipLib.Zip;
using Martin.Utility;
using Martin.ViewModel;
using FileStandard = System.IO.File;

namespace Martin.Controllers
{
    public class HomeController : Controller
    {
        public IAlbumRepository AlbumRepository { get; set; }
        public ISongRepository SongRepository{ get; set; }
        public IArtistRepository ArtistRepository { get; set; }
        public IStaticContentRepository StaticContentRepository { get; set; }

        public HomeController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
                SongRepository = scope.Resolve<ISongRepository>();
                StaticContentRepository = scope.Resolve<IStaticContentRepository>();
                ArtistRepository = scope.Resolve<IArtistRepository>();
            }
        }

        // -------------------- Album  --------------------
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
            return View("Index", model);
        }

        public ActionResult GetSlideByName(string albumName, long albumId)
        {
            return Index(albumId);
        }

        public JsonResult GetOneSlide(long albumId)
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

        // -------------------- Static  --------------------
        public ActionResult About()
        {
            var model = StaticContentRepository.Get(TypeStaticContent.About);
            return View(model);
        }

        public ActionResult Donate()
        {
            var model = StaticContentRepository.Get(TypeStaticContent.Donate);
            return View("About", model);
        }

        // -------------------- Login --------------------
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Artist artist)
        {
            artist = ArtistRepository.CheckPassword(artist);
            if (artist == null) return View();
            
            FormsAuthentication.SetAuthCookie(artist.Id.ToString(), false);
            return RedirectToAction("Albums", "Album");
        }

        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // -------------------- Download --------------------
        public FileResult GetAlbumInArchive(long idAlbum)
        {
            var album = AlbumRepository.Get(idAlbum);
            var zipName = string.Format("Martin S. - {0}.zip", album.Name);
            var zipPath = Path.Combine(Server.MapPath("~/Content/Song"), zipName);
            if (FileStandard.Exists(zipPath))
            {
                FileStandard.Delete(zipPath);
            }

            var zipFile = ZipFile.Create(zipPath);
            zipFile.BeginUpdate();

            var count = 1;
            // Add song in archive
            foreach (var song in album.Songs)
            {
                var globalMp3Path = Server.MapPath("~/" + FileHelper.PathToSong(album.Name, song.Mp3FileName));
                var dataSource = new StaticDiskDataSource(globalMp3Path);
                var cyrillicName = song.Name;
                var romanName = cyrillicName.FromCyrillicToRomanAlphabet();
                var songFileName = string.Format("{0}. Martin S. - {1}.mp3", count++, romanName);
                zipFile.Add(dataSource, songFileName);
            }

            // Add readme in archive
            var readmePath = Server.MapPath("~/" + FileHelper.PathToSong(album.Name, "readme.txt"));
            FileStandard.WriteAllText(readmePath, album.ReadmeInArchive);
            var readmeDataSource = new StaticDiskDataSource(readmePath);
            zipFile.Add(readmeDataSource, Path.GetFileName(readmePath));

            // Add cover in archive
            var coverPath = Server.MapPath("~/" + FileHelper.PathToCoverForAlbum(album.CoverFileName));
            var coverDataSource = new StaticDiskDataSource(coverPath);
            zipFile.Add(coverDataSource, Path.GetFileName(coverPath).FromCyrillicToRomanAlphabet());

            zipFile.CommitUpdate();
            zipFile.Close();

            var fileBytes = FileStandard.ReadAllBytes(zipPath);
            var fileName = Path.GetFileName(zipPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
        }

        public FileResult GetSongMp3(long songId)
        {
            var song = SongRepository.Get(songId);
            var localPath = FileHelper.PathToSong(song.Album.Name, song.Mp3FileName);
            var path = Server.MapPath("~/" + localPath);

            var fileBytes = FileStandard.ReadAllBytes(path);
            var fileName = Path.GetFileName(path);
            return File(fileBytes, "audio/mp3", fileName);
        }

        // -------------------- Util --------------------
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
