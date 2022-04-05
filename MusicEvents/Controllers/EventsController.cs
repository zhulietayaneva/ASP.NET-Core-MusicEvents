using Microsoft.AspNetCore.Mvc;

namespace MusicEvents.Controllers
{
    public class EventsController : Controller
    {
        
        public IActionResult Add() => View();


        //[HttpPost]
        //public IActionResult Add()
        //{
        //    return View();
        //}

    }
}
