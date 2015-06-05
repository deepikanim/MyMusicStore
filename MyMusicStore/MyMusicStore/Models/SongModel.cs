using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyMusicStore.Models
{
    public class SongModel
    {
        public int SongId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [RegularExpression("([0-5]?[0-9]):([0-5]?[0-9])", ErrorMessage = "Length should be in format (mm:ss)")]
        public string Length { get; set; }
        public int? TrackNumber { get; set; }
        public string Genre { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public int AlbumId { get; set; }
    }
}