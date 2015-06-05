using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusicStore.Models
{
    public class AlbumModel
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }

        public virtual IEnumerable<SongModel> Songs { get; set; }
    }
}