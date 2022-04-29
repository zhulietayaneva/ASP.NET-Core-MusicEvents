using MusicEvents.Data;

namespace MusicEvents.Services.Organizers
{
    public class OrganizerService : IOrganizerService
    {

        private readonly MusicEventsDbContext data;

        public OrganizerService(MusicEventsDbContext data)
        => this.data = data;

        public bool IsOrganizer(string userId)
        => this.data
        .Organizers
        .Any(d => d.UserId == userId);

        public int GetOrganizerId(string userId)
        {
            return data.Organizers.Where(o => o.UserId == userId).FirstOrDefault().Id;
        }

        public bool IsEvOrganizer(int evId, string userId)
        {
            if (userId==null)
            {
                return false;
            }
            if (!this.IsOrganizer(userId))
            {
                return false;
            }


            var orgId = GetOrganizerId(userId);


            var ev = data.Events.Where(e=>e.Id==evId).FirstOrDefault();
            if (ev.OrganizerId != orgId)
            {
                return false;
            }

            return true;

        }

    }
}
