using MyMusicStore.Data;
using MyMusicStore.Data.Entities;
using MyMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyMusicStore.Controllers
{
    public class SongsController : ApiController
    {
        private IMusicStoreRepository _repository { get; set; }
        private ModelFactory _modelFactory;

        public SongsController(IMusicStoreRepository repository)
        {
            _repository = repository;
            _modelFactory = new ModelFactory();
        }

        public IHttpActionResult Get(int albumId)
        {
            try
            {
                var songs = _repository.GetSongsForAlbum(albumId);

                if (songs == null)
                {
                    return NotFound();
                }

                return Ok(songs.ToList()
                               .Select(a => _modelFactory.Create(a)));
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(int albumId, int id)
        {
            try 
            {
                var song = _repository.GetSong(albumId, id);

                if (song == null)
                    return NotFound();

                return Ok(_modelFactory.Create(song));               
            }
            catch(Exception)
            {
                return InternalServerError();
            }
        }
       
        
        [HttpPost]
        public IHttpActionResult Post(int albumId, [FromBody]SongModel songModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (songModel == null)
                    {
                        return BadRequest();
                    }

                    var album = _repository.GetAlbum(albumId);
                    if (album == null)
                    {
                        return BadRequest();
                    }

                    //it is a duplicate
                    if (album.Songs.Any(s => s.Title == songModel.Title))
                    {
                        return BadRequest();
                    }

                    songModel.AlbumId = albumId;
                    songModel.DateAdded = DateTime.Now;
                    songModel.DateModified = DateTime.Now;
                    int maxSongId = _repository.GetMaxSongId(albumId);
                    songModel.SongId = maxSongId + 1;
                    songModel.TrackNumber = songModel.SongId;
                    
                    var song = _modelFactory.Create(songModel);
                    var status = Status.NothingModified;
                    var songReturned = _repository.AddSong(song, out status);

                    if (status == Status.Created)
                    {
                        // convert entity to model
                        var songAdded = _modelFactory.Create(songReturned);
                        return Created<SongModel>(Request.RequestUri + "/" + songAdded.SongId.ToString(), songAdded);
                    }
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPatch]
        [HttpPut]
        public IHttpActionResult Patch(int albumId, int id, [FromBody] SongModel songModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (songModel == null)
                    {
                        return BadRequest();
                    }

                    var album = _repository.GetAlbum(albumId);
                    if (album == null)
                    {
                        return BadRequest();
                    }

                    var song = _repository.GetSong(albumId, id);
                    if (song == null)
                    {
                        return NotFound();
                    }

                    var songEntity = _modelFactory.Create(songModel);

                    if (songModel.Title != null) song.Title = songEntity.Title;
                    if (songModel.Length != null) song.Length = songEntity.Length;
                    if (songModel.Genre != null) song.Genre = songEntity.Genre;
                    song.DateModified = DateTime.Now;

                    var status = Status.NothingModified;
                    var result = _repository.UpdateSong(song, out status);
                    if (status == Status.Updated)
                    {
                        var updatedSong = _modelFactory.Create(result);
                        return Ok(updatedSong);
                    }
                }
                return BadRequest();
            }
            catch(Exception)
            {
                return InternalServerError();
            }
        }   
     
        public void UpdateAttribute<T>(T attribute, T value)
        {
            attribute = value;
        }
    }
}