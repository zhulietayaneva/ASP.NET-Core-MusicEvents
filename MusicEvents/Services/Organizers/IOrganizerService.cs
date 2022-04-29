namespace MusicEvents.Services.Organizers
{
    public interface IOrganizerService
    {
        public int GetOrganizerId(string userId);

        public bool IsOrganizer(string userId);

        public bool IsEvOrganizer(int evId, string userId);
    }
}
