using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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

        public AlbumController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
                SongRepository = scope.Resolve<ISongRepository>();
                StaticContentRepository = scope.Resolve<IStaticContentRepository>();
            }
        }

        public ActionResult Index()
        {
            var model = AlbumRepository.GetAll();
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new Album();
            return View(model);
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

            AlbumRepository.Save(album);

            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/song/"), album.Name));

            return RedirectToAction("Index");
        }

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

        //public ActionResult StaticContent()
        //{
        //    return View();
        //}

        public ActionResult AddStaticContent()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddStaticContent(StaticContent staticContent)
        {
            StaticContentRepository.Save(staticContent);

            return View();
        }

        [HttpPost]
        public ActionResult Exit()
        {
            FormsAuthentication.SetAuthCookie("Fake", false);
            return RedirectToAction("Index", "Home");
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
