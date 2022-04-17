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


            List<Country> countries = countriesAndCities
                .Select(c => new Country
                {
                    CountryName = c.Name,
                    Cities = c.Cities.Select(city=> new City
                    {
                        CityName=city,
                        
                    }).ToList(),
                })
                .ToList();
          
            data.AddRange(countries);
            data.SaveChanges();
            //data.Countries.AddRange(countries);

            List<City> cities = new List<City>();
            foreach (var country in data.Countries)
            { 
                cities.Clear(); 
                foreach (var city in country.Cities)
                {
                    city.CountryId = country.Id;
                    
                    cities.Add(city);
                }
            }


            data.Cities.AddRange(cities);
            data.SaveChanges();
        }
    }
}
