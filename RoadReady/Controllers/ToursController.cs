using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoadReady;
using RoadReady.Models;
using RoadReady.ViewModels;

namespace RoadReady.Controllers
{
    public class ToursController : Controller
    {
        private readonly RoadReadyDbContext _context;

        public ToursController(RoadReadyDbContext context)
        {
            _context = context;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
              return _context.Tours != null ? 
                          View(await _context.Tours.ToListAsync()) :
                          Problem("Entity set 'RoadReadyDbContext.Tours'  is null.");
        }

        // GET: Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (tour == null)
            {
                return NotFound();
            }
            tour.Stops = tour.Stops.OrderBy(stop => stop.Order).ToList();
            return View(tour);
        }

        // GET: Tours/Create
        public IActionResult Create()
        {
            return View(new TourViewModel());
        }

        // POST: Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TourViewModel model)
        {
            var tour = new Tour
            {
                Name = model.Name,
            };
            _context.Tours.Add(tour);

            var stops = model.Stops;
            foreach(StopViewModel stop in stops)
            {
                var realStop = new Stop
                {
                    Order = stop.Order,
                    Latitude = stop.Latitude,
                    Longitude = stop.Longitude,
                    Name = stop.Name,
                    TourId = tour.Id
                };
                _context.Stops.Add(realStop);
            }
                
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", tour.Id);
        }

        // GET: Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            var stops = new List<StopViewModel>();
            foreach(var stop in tour.Stops.OrderBy(stop => stop.Order).ToList())
            {
                stops.Add(new StopViewModel
                {
                    Order = stop.Order,
                    Latitude = stop.Latitude,
                    Longitude = stop.Longitude,
                    Name = stop.Name
                });
            }

            var tourEdit = new TourEditViewModel
            {
                Id = tour.Id,
                Name = tour.Name,
                Stops = stops
            };


            return View(tourEdit);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] TourEditViewModel tourEdited)
        {
            var tour = _context.Tours.Find(tourEdited.Id);
            if (tour == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tour.Name = tourEdited.Name;
                    foreach(Stop stop in tour.Stops)
                    {
                        _context.Remove(stop);
                    }
                    _context.Update(tour);

                    var stops = tourEdited.Stops;
                    foreach (StopViewModel stop in stops)
                    {
                        var realStop = new Stop
                        {
                            Order = stop.Order,
                            Latitude = stop.Latitude,
                            Longitude = stop.Longitude,
                            Name = stop.Name,
                            TourId = tour.Id
                        };
                        _context.Stops.Add(realStop);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.Id))
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
            return View(tour);
        }

        // GET: Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tours == null)
            {
                return Problem("Entity set 'RoadReadyDbContext.Tours'  is null.");
            }
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
          return (_context.Tours?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
