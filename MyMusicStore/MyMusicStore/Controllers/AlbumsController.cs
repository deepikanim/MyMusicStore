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
    public class AlbumsController : ApiController
    {
        private IMusicStoreRepository _repository;
        private ModelFactory _modelFactory;      

        public AlbumsController(IMusicStoreRepository repository)
        {
            _repository = repository;
            _modelFactory = new ModelFactory();
        }

        //get all albums with or without songs
        public IHttpActionResult Get(bool includeSongs=true)
        {
            try
            {
                IQueryable<Album> albums;

                if(includeSongs)
                {
                    albums = _repository.GetAllAlbumsWithSongs();
                }
                else
                {
                    albums = _repository.GetAllAlbums();
                }
                
                return Ok(albums.ToList()
                                .Select(a => _modelFactory.Create(a)));
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        //Get 1 album with songs
        public IHttpActionResult Get(string name)
        {
            try
            {
                Album album = null;

                if(name != null)
                {
                    album = _repository.GetAlbumWithSongs(name);
                }

                if(album == null)
                {
                    return NotFound();
                }

                return Ok(_modelFactory.Create(album));                  
            }
            catch(Exception)
            {
                return InternalServerError();
            }
        }
    }
}
