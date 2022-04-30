using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Organizers;
using MusicEvents.Services.Organizers;

namespace MusicEvents.Controllers
{
    public class OrganizersController : Controller
    {
        private readonly MusicEventsDbContext data;
        private readonly IOrganizerService organizers;


        public OrganizersController(MusicEventsDbContext data)
        {
            this.data = data;
        }


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateOrganizerFormModel model)
        {

            if (organizers.IsOrganizer(User.GetId()))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var organizer = new Organizer
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                UserId = this.User.GetId()

            };

            this.data.Organizers.Add(organizer);
            this.data.SaveChanges();
            return RedirectToAction("All", "Events");

        }


    }
}
