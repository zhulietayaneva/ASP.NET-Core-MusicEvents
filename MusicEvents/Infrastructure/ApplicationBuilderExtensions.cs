using Microsoft.EntityFrameworkCore;
using MusicEvents.Data;
using MusicEvents.Data.DTO;
using MusicEvents.Data.Models;
using Newtonsoft.Json;

namespace MusicEvents.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase( this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<MusicEventsDbContext>();

            SeedCountriesAndCities(data);
            SeedGenres(data);
            SeedArtistsAndSongs(data);
            SeedSongs(data);
            data.Database.Migrate();
            
            return app;
        }

        private static void SeedCountriesAndCities(MusicEventsDbContext data)
        {

            if (data.Countries.Any())
            {
                return;
            }

            string inputJson = File.ReadAllText(@".\Data\countries.json");
            dynamic itemJson = JsonConvert.DeserializeObject(inputJson);
            var rows = itemJson;

            List<CountriesDTO> countriesAndCities = new List<CountriesDTO>();
            foreach (dynamic row in rows)
            {
                var curr = new CountriesDTO();

                //Console.WriteLine("Country name " + row.Name);
                curr.Name=row.Name;


                foreach (dynamic rowitem in row)
                {

                    foreach (dynamic city in rowitem)
                    {
                       // Console.WriteLine("City name "+city);
                        curr.Cities.Add(city.ToString());
                    }

                }

                countriesAndCities.Add(curr);

            }

            //List<Country> countries = countriesAndCities
            //    .Select(c => new Country
            //    {
            //        CountryName = c.Name,
            //        Cities = c.Cities.Select(city=> new City
            //        {
            //            CityName=city,
                        
                        
            //        }).ToList(),
            //    })
            //    .ToList();

            foreach (var country in countriesAndCities)
            {

                var ctry = new Country() { CountryName=country.Name};
                data.Countries.Add(ctry);
                data.SaveChanges();
                var cts = new List<City>();
                foreach (var city in country.Cities)
                {
                    cts.Add(new City { CityName=city, CountryId=ctry.Id});
                    data.Countries.First(c=>c.Id==ctry.Id).Cities.Add(new City { CityName = city, CountryId = ctry.Id });

                }
                //data.Countries.Find(ctry).Cities.AddRange(cts);
                data.Cities.AddRange(cts);
                data.SaveChanges();
            }
          
            //data.Countries.AddRange(countries);
            //data.SaveChanges();
            

            //if (data.Cities.Any())
            //{
            //    return;
            //}
            //List<City> cities = new List<City>();
            //foreach (var country in data.Countries)
            //{ 
            //    cities.Clear(); 
            //    foreach (var city in country.Cities)
            //    {
                    
                    
            //        cities.Add(city);
            //    }
            //}


            //data.Cities.AddRange(cities);
            //data.SaveChanges();
        }
        private static void SeedGenres(MusicEventsDbContext data)
        {
            if (data.Genres.Any())
            {
                return;
            }
            var genres = new List<Genre>() 
            {
                new Genre(){ GenreName="Pop-Folk"},
                new Genre(){ GenreName="Rock"},
                new Genre(){ GenreName="Metal"},
                new Genre(){ GenreName="Pop"},
                new Genre(){ GenreName="Rap"},
                new Genre(){ GenreName="Techno"},
                new Genre(){ GenreName="Jazz"}
            };
            data.Genres.AddRange(genres);
            data.SaveChanges();

        }
        private static void SeedArtistsAndSongs(MusicEventsDbContext data)
        {
            if (data.Artists.Any())
            {
                return;
            }


            var artists = new List<Artist>() 
            { 
                new Artist(){ArtistName="Azis", GenreId=1,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Galena", GenreId=1,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Sofi Marinova", GenreId=1,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Black Sabbath", GenreId=2,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Secta", GenreId=5,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="MBT", GenreId=5,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Atanas Kolev", GenreId=5,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Boro Purvi", GenreId=5,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="MurdaBoyz", GenreId=5,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="BNR", GenreId=5,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Mihaela Fileva", GenreId=4,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Mihaela Marinova", GenreId=4,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Dara", GenreId=4,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Geri-Nikol", GenreId=4,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Oscar Jerome", GenreId=7,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
                new Artist(){ArtistName="Preslava", GenreId=1,BirthDate=new DateTime(),ImageURL=@"https://img-s2.onedio.com/id-5d12321b352f4df72b2ae76d/rev-0/w-635/listing/f-jpg-webp/s-26ac11675c6dbb789c1e202cac0428be2487c4a9.webp",CountryId=1},
            
            };

            data.Artists.AddRange(artists);
            data.SaveChanges();
        }
        private static void SeedSongs(MusicEventsDbContext data) 
        {
            if (data.Songs.Any())
            {
                return;
            }
            var songs = new List<Song>() 
            {
                new Song(){ SongName = "Tova sum az", GenreId=5,
                    SongURL=@"https://www.youtube.com/embed/V4GvNAVxRJo",
                    Artists =  new List<Artist>() { data.Artists.Where(a => a.ArtistName == "Atanas Kolev").FirstOrDefault() }
                },
                new Song(){ SongName = "Megz", GenreId=5,
                    SongURL=@"https://www.youtube.com/embed/k4hB9y5ieFE",
                    Artists =  new List<Artist>(data.Artists.Where(a => a.ArtistName == "MBT" || a.ArtistName == "Secta").ToList())
                },
                new Song(){ SongName = "Garmish", GenreId=1,
                    SongURL=@"https://www.youtube.com/embed/tshokdMQsvk",
                    Artists =  new List<Artist>(){ data.Artists.Where(a => a.ArtistName == "Sofi Marinova").FirstOrDefault() }
                }


            };

            data.Songs.AddRange(songs);
            data.SaveChanges();


        }
    }
}
