using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Data.Models;

namespace MusicEvents.Data
{
    public class MusicEventsDbContext : IdentityDbContext
    {
       
        public MusicEventsDbContext(DbContextOptions<MusicEventsDbContext> options)
            : base(options)
        {
        }



        public DbSet<Artist> Artists { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Organizer> Organizers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Artist>()
                                    .HasMany(a => a.Songs)
                                    .WithMany(s=>s.Artists);

            builder.Entity<Artist>()
                                    .HasOne(a=>a.Genre)
                                    .WithMany(g=>g.Artists)
                                    .HasForeignKey(a=>a.GenreId)
                                    .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Artist>()
                                   .HasOne(a => a.Country)
                                   .WithMany(c => c.Artists)
                                   .HasForeignKey(a => a.CountryId)
                                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Artist>()    
                                   .HasMany(a => a.Events)
                                   .WithMany(e => e.Artists);




            //---------------------------------------------------
            //---------------------------------------------------

            builder.Entity<Event>()
                                    .HasMany(e=>e.Artists)
                                    .WithMany(a=>a.Events);


            builder.Entity<Event>()
                                    .HasOne(e => e.Country)
                                    .WithMany(c => c.Events)
                                    .HasForeignKey(e => e.CountryId)
                                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                                   .HasOne(e => e.City)
                                   .WithMany(c => c.Events)
                                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                                .HasOne(e => e.Organizer)
                                .WithMany(o => o.Events)
                                .OnDelete(DeleteBehavior.Restrict);


            //---------------------------------------------------
            //---------------------------------------------------

            builder.Entity<Genre>()
                                    .HasMany(g=>g.Artists)
                                    .WithOne(a=>a.Genre)
                                    .OnDelete(DeleteBehavior.Restrict);







            //---------------------------------------------------
            //---------------------------------------------------

            builder.Entity<Country>()
                                     .HasMany(c=>c.Artists)
                                     .WithOne(a=>a.Country)
                                     .HasForeignKey(a=>a.CountryId)
                                     .OnDelete(DeleteBehavior.Restrict);
            

            builder.Entity<Country>()
                                     .HasMany(c=>c.Events)
                                     .WithOne(e=>e.Country)
                                     .HasForeignKey(a=>a.CountryId)
                                     .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<Country>()
                                     .HasMany(c => c.Cities)
                                     .WithOne(c=>c.Country)
                                     .OnDelete(DeleteBehavior.Restrict);





            //---------------------------------------------------
            //---------------------------------------------------

            builder.Entity<Song>()
                                    .HasMany(s => s.Artists)
                                    .WithMany(a => a.Songs);

            builder.Entity<Song>()
                                    .HasOne(s => s.Genre)
                                    .WithMany(g => g.Songs);



            //---------------------------------------------------
            //---------------------------------------------------

            builder.Entity<City>()
                                  .HasMany(c => c.Events)
                                  .WithOne(c => c.City)
                                  .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<City>()
                                  .HasOne(c => c.Country)
                                  .WithMany(c => c.Cities)
                                  .OnDelete(DeleteBehavior.Restrict);


            //---------------------------------------------------
            //---------------------------------------------------

            builder
                    .Entity<Organizer>()
                    .HasOne<IdentityUser>()
                    .WithOne()
                    .HasForeignKey<Organizer>(o =>  o.UserId);
                    

            base.OnModelCreating(builder);
        }
    }
}