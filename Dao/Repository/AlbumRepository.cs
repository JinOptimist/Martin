﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MartinContext _db = new MartinContext();

        public Album GetRandom()
        {
            return _db.Album.OrderByDescending(x => x.Order).First();
        }

        public Album Get(long id)
        {
            return _db.Album.SingleOrDefault(x => x.Id == id);
        }

        public Album GetByOrder(int order)
        {
            return _db.Album.SingleOrDefault(x => x.Order == order);
        }

        public void Order(long id, bool up)
        {
            NewOrder(id, up);
        }

        public void Delete(long id)
        {
            _db.Album.Remove(Get(id));
            _db.SaveChanges();
        }

        public List<Album> GetAll()
        {
            return _db.Album.OrderByDescending(x => x.Order).ToList();
        }

        public void Save(Album album)
        {
            if (album.Id > 0)
            {
                _db.Album.Attach(album);
                _db.Entry(album).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            _db.Album.Add(album);
            _db.SaveChanges();
        }

        public int Count()
        {
            return _db.Album.Count();
        }

        public void Reorder()
        {
            var i = 1;
            var albums = _db.Album.OrderBy(x => x.Order);
            foreach (var album in albums)
            {
                album.Order = i++;
                _db.Album.Attach(album);
                _db.Entry(album).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        private void NewOrder(long id, bool goUp)
        {
            var album = Get(id);
            var oldOrder = album.Order;
            var newOrder = goUp ? oldOrder + 1 : oldOrder - 1;

            if (newOrder < 1 || newOrder > Count())
            {
                return;
            }

            var albumToRelocate = GetByOrder(newOrder);
            if (albumToRelocate != null)
            {
                albumToRelocate.Order = oldOrder;

                _db.Album.Attach(albumToRelocate);
                _db.Entry(albumToRelocate).State = EntityState.Modified;
            }

            album.Order = newOrder;
            _db.Album.Attach(album);
            _db.Entry(album).State = EntityState.Modified;

            _db.SaveChanges();
        }
    }
}