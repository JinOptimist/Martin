using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using Dao.Repository;
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
            if (!ModelState.IsValid)
            {
                return View(songViewModel);
            }

            var song = songViewModel.CreateSongModel();
            var album = AlbumRepository.Get(song.Album.Id);
            var songName = SaveAttach("song/" + album.Name, Request.Files["Song"]);
            song.Mp3FileName = songName;
            SongRepository.Save(song);
            return RedirectToAction("Index");
        }

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
