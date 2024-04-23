using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using cis237_inclass_6.Models;

namespace cis237_inclass_6.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly CarContext _context;

        public CarsController(CarContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            // Setup a variable to hold the cars data.
            DbSet<Car> CarsToFilter = _context.Cars;

            // Setup some strings to hold the data that might be
            // in the session. If there is nothing in the seesion
            // we can still use these variables as a default.
            string filterMake = "";
            string filterMin = "";
            string filterMax = "";

            // Define a min and max for the cylinders;
            int min = 0;
            int max = 16;

            // Check to see if there is a value in the session,
            // and if there is, assign it to the variable that
            // we setup to hold the value
            if (!String.IsNullOrWhiteSpace(HttpContext.Session.GetString("session_make")))
            {
                filterMake = HttpContext.Session.GetString("session_make");
            }

            // Check to see if there is a value in the session,
            // and if there is, assign it to the variable that
            // we setup to hold the value
            if (!String.IsNullOrWhiteSpace(HttpContext.Session.GetString("session_min")))
            {
                filterMin = HttpContext.Session.GetString("session_min");

                min = Int32.Parse(filterMin);

            }

            // Check to see if there is a value in the session,
            // and if there is, assign it to the variable that
            // we setup to hold the value
            if (!String.IsNullOrWhiteSpace(HttpContext.Session.GetString("session_max")))
            {
                filterMax = HttpContext.Session.GetString("session_max");

                max = Int32.Parse(filterMax);

            }

            // Do the filter on the CarsToFilter Dataset.
            // Use the Where() method that we used before when doing
            // the last inclass, only this time send in more
            // lambda expressions to narrow it down further.
            // Since we setup the default values for each of the
            // filter parameters, min, max, and filterMake, we
            // can count on this always running with no errors.
            IList<Car> finalFiltered = await CarsToFilter.Where(car => car.Cylinders >= min &&
                                                                       car.Cylinders <= max &&
                                                                       car.Make.Contains(filterMake)
                                                               ).ToListAsync();

            // Place the string representation of the values
            // that are in the sesssion into the viewdata so
            // that they can be retrieved and displayed on the view.
            ViewData["filterMake"] = filterMake;
            ViewData["filterMin"] = filterMin;
            ViewData["filterMax"] = filterMax;
            
            return View(finalFiltered);

            // This was the original return statement
            //return View(await _context.Cars.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,Make,Model,Type,Horsepower,Cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Year,Make,Model,Type,Horsepower,Cylinders")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'CarContext.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter()
        {
            // Get the form data that we sent out of the request object.
            // The string that is used as a key to get the data matches
            // the name propperty of the form control
            string make = HttpContext.Request.Form["make"];
            string min = HttpContext.Request.Form["min"];
            string max = HttpContext.Request.Form["max"];

            // Now that we have the data pulled out from the request object,
            // let's put it into the session so that other methods can have access to it.
            HttpContext.Session.SetString("session_make", make);
            HttpContext.Session.SetString("session_min", min);
            HttpContext.Session.SetString("session_max", max);

            //return Content("foobar");
            // Redirect to the index page
            return RedirectToAction(nameof(Index));
        }

        public IActionResult JsonApi()
        {
            return Json(_context.Cars.ToList());
        }

        private bool CarExists(string id)
        {
          return _context.Cars.Any(e => e.Id == id);
        }
    }
}
