using MyMusicStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusicStore.Models
{
    public class ModelFactory
    {
        public AlbumModel Create(Album album)
        {
            return new AlbumModel()
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ArtistName = album.ArtistName,
                Songs = album.Songs.Select(s => Create(s))
            };
        }

        public SongModel Create(Song s)
        {
            return new SongModel()
            {
                SongId = s.SongId,
                Title = s.Title,
                Length = s.Length,
                TrackNumber = s.TrackNumber,
                Genre = s.Genre,
                DateAdded = s.DateAdded,
                DateModified = s.DateModified,
                AlbumId = s.AlbumId
            };
        }

        private string ConvertSecondsToMMSS(int p)
        {
            return String.Format("{0}:{1:00}", p / 60, p % 60);
        }

        public Song Create(SongModel s)
        {
            return new Song()
            {
                SongId = s.SongId,
                Title = s.Title,
                Length = s.Length,
                TrackNumber = s.TrackNumber,
                Genre = s.Genre,
                DateAdded = s.DateAdded,
                DateModified = s.DateModified,
                AlbumId = s.AlbumId
            };
        }

        private int ConvertMMSSToSeconds(string p)
        {
            string[] elements = p.Split(':');
            return Int32.Parse(elements[0]) * 60 + Int32.Parse(elements[1]);
        }
    }
}