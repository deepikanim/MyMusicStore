using MyMusicStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicStore.Data
{
    public class MusicStoreDbRepository:IMusicStoreRepository
    {
        MusicStoreDbContext _context;

        public MusicStoreDbRepository(MusicStoreDbContext context)
        {
            _context = context;
            _context.Configuration.LazyLoadingEnabled = false;
        }

        public Album GetAlbum(int id)
        {
            return _context.Albums.Include("Songs").FirstOrDefault(a => a.AlbumId == id);
        }

        public IQueryable<Album> GetAllAlbums()
        {
            return _context.Albums;
        }

        public IQueryable<Album> GetAllAlbumsWithSongs()
        {
            return _context.Albums.Include("Songs");
        }

        public Album GetAlbumWithSongs(string albumName)
        {
            return _context.Albums.Include("Songs").FirstOrDefault(e => e.Title.Equals(albumName));
        }

        public Song GetSong(int albumId, int id)
        {
            return _context.Songs.FirstOrDefault(s => s.SongId == id && s.AlbumId == albumId);
        }

        public IQueryable<Song> GetSongsForAlbum(int albumId)
        {
            var correctAlbum = _context.Albums.FirstOrDefault(s => s.AlbumId == albumId);
            if (correctAlbum != null)
            {
                return _context.Songs.Where(s => s.AlbumId == albumId);
            }
            else
            {
                return null;
            }
        }

        public Song AddSong(Song song, out Status status)
        {
            try
            {
                _context.Songs.Add(song);
                var result = _context.SaveChanges();

                if (result == 1)
                {
                    status = Status.Created;
                }
                else
                {
                    status = Status.NothingModified;
                }
                return song;
            }
            catch(Exception)
            {
                status = Status.Error;
                return song;
            }
            
        }

        public Song UpdateSong(Song song, out Status status)
        {
            try
            {
                _context.Entry(song).State = EntityState.Detached;
                _context.Songs.Attach(song);
                _context.Entry(song).State = EntityState.Modified;

                var result = _context.SaveChanges();
                if (result > 0)
                {
                    status = Status.Updated;
                }
                else
                {
                    status = Status.NothingModified;
                }
                return song;
            }
            catch (Exception)
            {
                status = Status.Error;
                return song;
            }
        }
        public int GetMaxSongId(int albumId)
        {
            var max = (_context.Songs.Where(s => s.AlbumId == albumId)
                                    .OrderByDescending(x => x.SongId).First()).SongId;
            return max;
        }
    }
}
