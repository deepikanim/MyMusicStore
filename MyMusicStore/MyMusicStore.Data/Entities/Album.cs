using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicStore.Data.Entities
{
    public class Album
    {
        public Album()
        {
            Songs = new List<Song>();
        }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
