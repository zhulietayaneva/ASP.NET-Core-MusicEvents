using Microsoft.EntityFrameworkCore;
using MusicEvents.Data;

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

    }
}
