using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Library.Repository_Interfaces;
using Store.WebApp.ViewModels;

namespace Store.WebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository repository)
        {
            _locationRepository = repository;
        }

        // GET: Location
        public ActionResult Index()
        {
            var locations = _locationRepository.GetAll();
            return View(locations);
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            var location = _locationRepository.Get(id);
            return View(location);
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name")] LocationViewModel viewLocation, IFormCollection collection)
        {
            if(!ModelState.IsValid)
            {
                return View(viewLocation);
            }

            try
            {
                // give the id as 1 just to get it into the system
                // the create repo does not use that id to form a new DB entry
                var location = new Library.Location(viewLocation.Name, 1);
                _locationRepository.Create(location);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "There was a problem Creating the Location");
                return View(viewLocation);
            }
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            // add the id to temp data for security
            TempData["Location"] = id;
            // get the location needed from the db
            var location = _locationRepository.Get(id);
            // convert to view model
            var viewLocation = new LocationViewModel() { Name = location.Name, Id = location.Id };
            return View(viewLocation);
        }

        // POST: Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind ("Name, Id")] LocationViewModel viewLocation, IFormCollection collection)
        {
            // get the current id from tempdata to ensure the user hasn't modified the id that the page put out
            int tdId = (int)TempData.Peek("Location");
            if (tdId != id && tdId != viewLocation.Id)
            {
                // *ER add error message
                return RedirectToAction(nameof(Index));
            }

            // check modelstate
            if(!ModelState.IsValid)
            {
                return View(viewLocation);
            }

            try
            {
                var location = new Library.Location(viewLocation.Name, viewLocation.Id);
                _locationRepository.Update(location);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
