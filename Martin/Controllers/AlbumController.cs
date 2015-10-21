using System;
using System.Collections.Generic;
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
    public class AlbumController : Controller
    {
        //
        // GET: /Album/

        public IAlbumRepository AlbumRepository { get; set; }
        public ISongRepository SongRepository { get; set; }

        public AlbumController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                AlbumRepository = scope.Resolve<IAlbumRepository>();
                SongRepository = scope.Resolve<ISongRepository>();
            }
        }

        public ActionResult Index()
        {
            var model = AlbumRepository.GetAll();
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Album album)
        {
            AlbumRepository.Save(album);

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
            SongRepository.Save(songViewModel.Song);
            return RedirectToAction("Index");
        }

        
    }
}
