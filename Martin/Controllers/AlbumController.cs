using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using Martin.ViewModel;

namespace Martin.Controllers
{
    [Authorize(Users = "Black-Jin")]
    public class AlbumController : Controller
    {
        public IAlbumRepository AlbumRepository { get; set; }
        public ISongRepository SongRepository { get; set; }
        public IStaticContentRepository StaticContentRepository { get; set; }

        private static object locker = new object();

        public AlbumController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
                SongRepository = scope.Resolve<ISongRepository>();
                StaticContentRepository = scope.Resolve<IStaticContentRepository>();
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

        public ActionResult Index()
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

            var coverPath = SaveAttach("cover", Request.Files["Cover"]);
            var bgForAlbumPath = SaveAttach("cover", Request.Files["BgForAlbum"]);
            if (!string.IsNullOrEmpty(coverPath))
                album.CoverFileName = coverPath;
            if (!string.IsNullOrEmpty(bgForAlbumPath))
                album.BackgroundFileName = bgForAlbumPath;

            var order = AlbumRepository.Count() + 1;
            album.Order = order;

            AlbumRepository.Save(album);

            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/song/"), album.Name));

            return RedirectToAction("Index");
        }

        public ActionResult Edit(long id)
        {
            var model = AlbumRepository.Get(id);
            return View("Add", model);
        }

        public ActionResult Delete(long id)
        {
            AlbumRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult OrderUp(long id)
        {
            AlbumRepository.OrderUp(id);
            return RedirectToAction("Index");
        }

        public ActionResult OrderDown(long id)
        {
            AlbumRepository.OrderDown(id);
            return RedirectToAction("Index");
        }

        // -------------------- Song Region --------------------

        public ActionResult AddSong()
        {
            var model = new SongViewModel();
            model.AlbumsList = AlbumRepository.GetAll().Select(x => new AlbumForListBox(x)).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSong(SongViewModel songViewModel)
        {
            var song = songViewModel.CreateSongModel();
            var album = AlbumRepository.Get(song.Album.Id);

            var folder = Path.Combine("song", album.Name);
            var songName = SaveAttach(folder, Request.Files["Song"]);

            //var path = Path.Combine(Server.MapPath("~/Content/"), folder, songName);
            //var tagFile = TagLib.File.Create(path);
            //tagFile.Tag.Album = album.Name;
            //tagFile.Tag.Genres = new[] { "Indie" };
            //tagFile.Tag.AlbumArtists = new[] { "Martin S." };
            //tagFile.Tag.Title = songName;
            //tagFile.Tag.Lyrics = song.Lyrics;
            //IPicture a = new AttachedPictureFrame();
            //a.Data = new ByteVector();
            //tagFile.Tag.Pictures = new IPicture[];

            song.Mp3FileName = songName;
            SongRepository.Save(song);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteSong(long id)
        {
            SongRepository.Delete(id);
            return RedirectToAction("Index");
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

        // -------------------- Login --------------------

        public ActionResult Exit()
        {
            FormsAuthentication.SetAuthCookie("Fake", false);
            return RedirectToAction("Index", "Home");
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
            
            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
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
