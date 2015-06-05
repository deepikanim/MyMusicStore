using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicStore.Data.Entities
{
    public class Song
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Length { get; set; }
        public int? TrackNumber { get; set; }
        public string Genre { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
