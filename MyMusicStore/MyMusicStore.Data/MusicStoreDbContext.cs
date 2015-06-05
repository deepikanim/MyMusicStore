using MyMusicStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicStore.Data
{
    public class MusicStoreDbContext:DbContext
    {
        public MusicStoreDbContext()
            : base("name=MusicStoreDbContext")
        {

        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Album)
                .HasForeignKey(e => e.AlbumId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song>()
                .HasKey(p => new { p.SongId, p.AlbumId });
        }
    }
}
