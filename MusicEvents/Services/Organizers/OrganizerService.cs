using MusicEvents.Data;

namespace MusicEvents.Services.Organizers
{
    public class OrganizerService:IOrganizerService
    {
       
            private readonly MusicEventsDbContext data;
           
            public OrganizerService(MusicEventsDbContext data)
            => this.data = data;
            
            public bool IsOrganizer(string userId)
            => this.data
            .Organizers
            .Any(d => d.UserId == userId);
                
    }
}
