using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Organizers;

namespace MusicEvents.Controllers
{
    public class OrganizersController : Controller
    {
        private readonly MusicEventsDbContext data;


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
            var userIsDealer = this.data
            .Organizers
            .Any(d => d.UserId == this.User.GetId());

            if (userIsDealer)
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
