using MyMusicStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicStore.Data
{
    public interface IMusicStoreRepository
    {
        Album GetAlbum(int albumId);
        IQueryable<Album> GetAllAlbums();
        Album GetAlbumWithSongs(string name);
        IQueryable<Album> GetAllAlbumsWithSongs();

        Song GetSong(int albumId, int id);
        IQueryable<Song> GetSongsForAlbum(int albumId);
        Song AddSong(Song song, out Status status);
        int GetMaxSongId(int albumId);
        Song UpdateSong(Song song, out Status status);
    }
}
