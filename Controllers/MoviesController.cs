using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string  movieGenre, string searchString, string sortOrder)
        {
             var movies = from m in _context.Movie
                 select m;
            
             ViewData["ReleaseDateSort"] = sortOrder == "Date" ? "date_desc" : "Date";
            // Use LINQ to get list of genres.
            /*IQueryable<string> genreQuery = from m in _context.Movie
                                    orderby m.GenreId
                                    select m.GenreId;
*/
           
            if (!string.IsNullOrEmpty(sortOrder)) 
            {    
            switch (sortOrder)
            {
                case "Date":
                    movies = movies.OrderBy(m => m.ReleaseDate);
                    break;
                case "name_desc":
                    movies = movies.OrderByDescending(m => m.ReleaseDate);
                    break;
                default:
                    movies = movies.OrderBy(m => m.Title);
                    break;
            }
            }

            if (!string.IsNullOrEmpty(searchString))
            {       
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

             if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre.GenreName == movieGenre);
            }


            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(_context.Genre, "GenreId", "GenreName"),               
                Movies = await movies.Include(m => m.Genre).ToListAsync()
            };

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // ViewData["Genres"] = new SelectList(_context.Genre, "GenreId", "GenreName");

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.Include(m => m.Genre)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["Genres"] = new SelectList(_context.Genre, "GenreId", "GenreName");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
             //ViewData["Genres"] = new SelectList(_context.Genre, "GenreId", "GenreName");
             movie.Genre = _context.Genre.FirstOrDefault(g => g.GenreId == movie.Genre.GenreId);
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }         

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

           
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.Genre = _context.Genre.FirstOrDefault(g => g.GenreId == movie.Genre.GenreId);
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

       

    }
}