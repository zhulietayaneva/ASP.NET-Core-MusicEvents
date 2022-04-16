using Microsoft.EntityFrameworkCore;
using MusicEvents.Data;
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

            
            data.Database.Migrate();

            return app;
        }

        private static void SeedCountriesAndCities(MusicEventsDbContext data)
        {
            string inputJson = File.ReadAllText("../../../Data/countires.json");

            var countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(inputJson);


        }
    }
}
