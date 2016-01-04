using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using Dao.Repository;
using Martin.Utility;
using Martin.ViewModel;

namespace Martin.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        public IAlbumRepository AlbumRepository { get; set; }
        public ISongRepository SongRepository { get; set; }
        public IArtistRepository ArtistRepository { get; set; }
        public IStaticContentRepository StaticContentRepository { get; set; }

        private static object locker = new object();

        public AlbumController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
                SongRepository = scope.Resolve<ISongRepository>();
                StaticContentRepository = scope.Resolve<IStaticContentRepository>();
                ArtistRepository = scope.Resolve<IArtistRepository>();
            }
        }

        private string DefaultFontPath
        {
            get
            {
                return Server != null ? Server.MapPath("~/Content/font/defaultFont.ttf") : string.Empty;
            }
        }

        private string SuperPuperFontPath
        {
            get
            {
                return Server != null ? Server.MapPath("~/Content/font/superPuperFont.ttf") : string.Empty;
            }
        }

        public ActionResult Albums()
        {
            var model = AlbumRepository.GetAll();
            return View(model);
        }

        // -------------------- Album Region --------------------

        public ActionResult Add()
        {
            var model = new Album();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Album album)
        {
            if (!ModelState.IsValid)
            {
                return View(album);
            }

            album.Name = album.Name.Replace(".", string.Empty);

            var coverPath = SaveAttach("cover", Request.Files["Cover"]);
            var bgForAlbumPath = SaveAttach("cover", Request.Files["BgForAlbum"]);
            if (!string.IsNullOrEmpty(coverPath))
                album.CoverFileName = coverPath;
            if (!string.IsNullOrEmpty(bgForAlbumPath))
                album.BackgroundFileName = bgForAlbumPath;

            if (album.Id <= 0)
            {
                var order = AlbumRepository.Count() + 1;
                album.Order = order;
            }

            AlbumRepository.Save(album);

            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/song/"), album.Name));

            return RedirectToAction("Albums");
        }

        public ActionResult Edit(long id)
        {
            var model = AlbumRepository.Get(id);
            return View("Add", model);
        }

        public ActionResult Delete(long id)
        {
            var album = AlbumRepository.Get(id);
            var zipPath = Path.Combine(Server.MapPath("~/Content/Song"), album.Name + ".zip");
            if (System.IO.File.Exists(zipPath))
            {
                System.IO.File.Delete(zipPath);
            }

            var albumPath = Path.Combine(Server.MapPath("~/Content/Song"), album.Name);
            if (System.IO.File.Exists(albumPath))
            {
                System.IO.File.Delete(albumPath);
            }

            AlbumRepository.Delete(id);
            AlbumRepository.Reorder();
            
            return RedirectToAction("Albums");
        }

        public ActionResult AlbumOrder(long id, bool up)
        {
            AlbumRepository.Order(id, up);
            return RedirectToAction("Albums");
        }

        // -------------------- Song Region --------------------

        public ActionResult AddSong(long albumId, long? songId)
        {
            var model = new Song();
            if (songId.HasValue)
            {
                model = SongRepository.Get(songId.Value);
            }
            else
            {
                model.Album = AlbumRepository.Get(albumId);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddSong(Song model)
        {
            var album = AlbumRepository.Get(model.Album.Id);
            var folder = Path.Combine("song", album.Name);
            var songName = SaveAttach(folder, Request.Files["Song"]);

            if (model.Id <= 0)
            {
                var order = SongRepository.Count(model.Album.Id) + 1;
                model.Order = order;
            }

            if (model.Id <= 0 || !string.IsNullOrEmpty(songName))
            {
                model.Mp3FileName = songName;
            }
            
            SongRepository.Save(model);
            return RedirectToAction("Albums");
        }

        public ActionResult DeleteSong(long id)
        {
            SongRepository.Delete(id);
            return RedirectToAction("Albums");
        }

        public ActionResult SongOrder(long id, bool up)
        {
            SongRepository.Order(id, up);
            return RedirectToAction("Albums");
        }

        // -------------------- Static Region --------------------

        public ActionResult AddStaticContent()
        {
            return View();
        }

        public JsonResult GetStaticContent(long type)
        {
            var staticContent = StaticContentRepository.Get((TypeStaticContent)type);
            if (staticContent == null)
                return null;

            return new JsonResult
            {
                Data = new
                {
                    body = staticContent.Body,
                    title = staticContent.Title
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddStaticContent(StaticContent staticContent)
        {
            StaticContentRepository.Save(staticContent);

            return View();
        }

        // -------------------- Font Region --------------------

        public ActionResult ResetFont()
        {
            lock (locker)
            {
                if (System.IO.File.Exists(SuperPuperFontPath))
                {
                    System.IO.File.Delete(SuperPuperFontPath);
                }

                System.IO.File.Copy(DefaultFontPath, SuperPuperFontPath);
            }

            return RedirectToAction("Albums");
        }

        public ActionResult SaveNewFont()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveNewFont(string fake)
        {
            var fontFile = Request.Files["font"];
            var newFileName = SaveAttach("font", fontFile);
            var newFilePath = Server.MapPath("~/Content/font/" + newFileName);

            SaveNewFontFile(newFilePath);

            return RedirectToAction("Albums");
        }

        // -------------------- Artist Region --------------------

        public ActionResult AddArtist(long? id)
        {
            var model = new Artist();
            if (id.HasValue)
            {
                model = ArtistRepository.Get(id.Value);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddArtist(Artist artist)
        {
            ArtistRepository.Save(artist);
            return RedirectToAction("Artists");
        }

        public ActionResult Artists()
        {
            var artists = ArtistRepository.GetAll().Where(x => !x.MegaAdmin).ToList();
            return View(artists);
        }

        public ActionResult DeleteArtist(long id)
        {
            ArtistRepository.Delete(id);
            return RedirectToAction("Artists");
        }

        private void SaveNewFontFile(string newFilePath)
        {
            lock (locker)
            {
                if (System.IO.File.Exists(SuperPuperFontPath))
                {
                    System.IO.File.Delete(SuperPuperFontPath);
                }
                System.IO.File.Move(newFilePath, SuperPuperFontPath);
            }
        }

        private string SaveAttach(string folder, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
                return string.Empty;

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/"), folder, fileName);
            file.SaveAs(path);

            return fileName;
        }
    }
}
