using MyMusicStore.Data.Entities;
using MyMusicStore.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MyMusicStore.Data
{
    public class MusicStoreXmlRepository : IMusicStoreRepository
    {
        public XDocument _xDoc { get; set; }
        private string _path { get; set; }
        private MemoryCacher _memCacher { get; set;}

        public MusicStoreXmlRepository()
        {
            _memCacher = new MemoryCacher();
            LoadXml();           
        }

        public void LoadXml()
        {
            _xDoc = (XDocument)_memCacher.GetValue("xmlFile");
            if (_xDoc == null)
            {
                _path = HttpContext.Current.Server.MapPath(@"~/App_Data/Songs.xml");
                _xDoc = XDocument.Load(_path);

                _memCacher.Add("xmlFile", _xDoc, DateTimeOffset.UtcNow.AddHours(5));
            }
        }
        public Entities.Album GetAlbum(int albumId)
        {
            return (from q in _xDoc.Descendants("album")
                    where Int32.Parse(q.Attribute("Id").Value) == albumId
                    select new Album
                    {
                        AlbumId = (int)q.Attribute("Id"),
                        Title = (string)q.Attribute("title"),
                        ArtistName = (string)q.Parent.Attribute("name"),
                        Songs =
                        (
                            from s in q.Elements("song")
                            select new Song
                            {
                                SongId = (int)s.Attribute("SongId"),
                                Title = (string)s.Attribute("title"),
                                Length = (string)s.Attribute("length"),
                                AlbumId = (int)q.Attribute("Id")
                            }
                         ).ToList()
                    }).FirstOrDefault<Album>();  
        }

        public IQueryable<Entities.Album> GetAllAlbums()
        {
            return (from q in _xDoc.Descendants("album")
                    select new Album
                    {
                        AlbumId = (int)q.Attribute("Id"),
                        Title = (string)q.Attribute("title"),
                        ArtistName = (string)q.Parent.Attribute("name")
                    }).AsQueryable<Album>();
        }

        public Entities.Album GetAlbumWithSongs(string name)
        {
            return (from q in _xDoc.Descendants("album")
                    where q.Attribute("title").Value == name
                    select new Album
                    {
                        AlbumId = (int)q.Attribute("Id"),
                        Title = (string)q.Attribute("title"),
                        ArtistName = (string)q.Parent.Attribute("name"),
                        Songs =
                        (
                            from s in q.Elements("song")
                            select new Song
                            {
                                SongId = (int)s.Attribute("SongId"),
                                Title = (string)s.Attribute("title"),
                                Length = (string)s.Attribute("length"),
                                AlbumId = (int)q.Attribute("Id")
                            }
                         ).ToList()
                    }).FirstOrDefault<Album>();  
        }

        public IQueryable<Album> GetAllAlbumsWithSongs()
        {
            return (from q in _xDoc.Descendants("album")
                    select new Album
                    {
                        AlbumId = (int)q.Attribute("Id"),
                        Title = (string)q.Attribute("title"),
                        ArtistName = (string)q.Parent.Attribute("name"),
                        Songs =
                        (
                            from s in q.Elements("song")
                            select new Song
                            {
                                SongId = (int)s.Attribute("SongId"),
                                Title = (string)s.Attribute("title"),
                                Length = (string)s.Attribute("length"),
                                AlbumId = (int)q.Attribute("Id")
                            }
                         ).ToList()
                    }).AsQueryable<Album>();     
        }

        public Entities.Song GetSong(int albumId, int id)
        {
            XElement song = _xDoc.Descendants("album")
                                .Where(a => Int32.Parse(a.Attribute("Id").Value) == albumId)
                                .Elements("song")
                                .Where(b => Int32.Parse(b.Attribute("SongId").Value) == id)
                                .FirstOrDefault();

            if(song == null)
            {
                return null;
            }
            return new Song
            {
                SongId = (int)song.Attribute("SongId"),
                Title = (string)song.Attribute("title"),
                Length = (string)song.Attribute("length"),
                AlbumId = albumId
            };
        }

        public IQueryable<Entities.Song> GetSongsForAlbum(int albumId)
        {
            IEnumerable<XElement> songs = _xDoc.Descendants("album")
                                                .Where(a => Int32.Parse(a.Attribute("Id").Value) == albumId)
                                                .Elements("song");
            if (songs.Count() > 0)
            {
                return (from s in songs
                        select new Song
                        {
                            SongId = (int)s.Attribute("SongId"),
                            Title = (string)s.Attribute("title"),
                            Length = (string)s.Attribute("length"),
                            AlbumId = albumId
                        }).AsQueryable<Song>();
            }
            else
            {
                return null;
            }
        }

        public Entities.Song AddSong(Entities.Song song, out Status status)
        {
            try
            {
                //delete xml from cache and load xml from xml file
                _memCacher.Delete("xmlFile");
                LoadXml();

                _xDoc.Descendants("album")
                 .First(c => (int)c.Attribute("Id") == song.AlbumId)
                 .Add
                 (
                     new XElement
                         (
                             "song",
                             new XAttribute("title", song.Title),
                             new XAttribute("length", song.Length),
                             new XAttribute("SongId", song.SongId)
                         )
                  );

                _xDoc.Save(_path);
                status = Status.Created;

                song.TrackNumber = null;
                song.Genre = null;
                song.DateAdded = null;
                song.DateModified = null;
                
                return song;
            }
            catch(Exception)
            {
                status = Status.Error;
                return song;
            }
        }

        public int GetMaxSongId(int albumId)
        {
            int maxSongId = Int32.Parse(_xDoc.Descendants("album")
                                 .Where(a => Int32.Parse(a.Attribute("Id").Value) == albumId)
                                 .Elements("song")
                                 .OrderBy(s => s.Attribute("songId"))
                                 .Last()
                                 .Attribute("SongId")
                                 .Value);
            return maxSongId;                 
        }

        public Entities.Song UpdateSong(Entities.Song song, out Status status)
        {
            try
            {
                //delete xml from cache and load xml from xml file
                _memCacher.Delete("xmlFile");
                LoadXml();

                XElement songToBeUpdated = _xDoc.Descendants("album")
                                               .FirstOrDefault(c => (int)c.Attribute("Id") == song.AlbumId)
                                               .Elements("song")
                                               .FirstOrDefault(d => (int)d.Attribute("SongId") == song.SongId);
              
               songToBeUpdated.SetAttributeValue("title", song.Title);
               songToBeUpdated.SetAttributeValue("length", song.Length);

                _xDoc.Save(_path);
                status = Status.Updated;

                song.TrackNumber = null;
                song.Genre = null;
                song.DateAdded = null;
                song.DateModified = null;

                return song;
            }
            catch (Exception)
            {
                status = Status.Error;
                return song;
            }
        }
    }
}
