using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Validation;
using String = System.String;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private MyDbContext context;

        public MoviesController()
        {
            context = new MyDbContext();
        }
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() {Name = "Shrek!"};
            var customers = new List<Customer>();
            {
                new Customer() {Name = "Customer 1"};
                new Customer() {Name = "Customer 2"};
            }

            var randomMovieViewModel = new RandomMovieViewModel()
            {
                Customers = customers,
                Movie = movie
            };
            return View(randomMovieViewModel);
        }

        [Route("movies/show-movies")]
        public ActionResult ShowMovies()
        {
            return View(context.Movies.Include(c => c.Genre).ToList());
        }

        public ActionResult Edit(int id)
        {
            var movie = this.context.Movies.Include(c => c.Genre).FirstOrDefault(s => s.Id == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            var movieViewModel = new MovieViewModel()
            {
                Movie = movie,
                GenreList = this.context.GenreType.ToList()
            };
            return View("MovieForm",movieViewModel);
        }

        [Route("movies/released/{year}/{month:regex(^\\d{2}$):range(1,12)}")]
        public ActionResult ByReleasedYear(int year, int month)
        {
            var movie = new Movie() { Name = "Shrek!" };
            return Content($"{movie.Name} was released {month}-{year}");
        }

        public ActionResult MovieByName(string name)
        {
            return View(context.Movies.Include(c=>c.Genre).FirstOrDefault(s => s.Name == name));
        }

        [Route("movies/Details/{id}")]
        public ActionResult Details(int id)
        {
           return View(context.Movies.Include(c => c.Genre).FirstOrDefault(c => c.Id == id)); 
        }


        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (String.IsNullOrEmpty(sortBy))
                sortBy = "Name";
            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        public ActionResult New()
        {
            var movieMiewModel = new MovieViewModel()
            {
                Movie = new Movie(),
                GenreList = this.context.GenreType
            };
            return View("MovieForm", movieMiewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MovieViewModel viewModel)
        {
            if (!ModelState.IsValid) 
            {
                var newViewModel = new MovieViewModel()
                {
                    Movie = new Movie(),
                    GenreList = this.context.GenreType.ToList()
                };
                return View("MovieForm", newViewModel);
            }

            if (viewModel.Movie.Id == 0)
            {
                viewModel.Movie.AddedDate = DateTime.Now;
                this.context.Movies.Add(viewModel.Movie);
            }
            else
            {
                var movie = this.context.Movies.Single(s => s.Id == viewModel.Movie.Id);
                movie.Name = viewModel.Movie.Name;
                movie.ReleaseDate = viewModel.Movie.ReleaseDate;
                movie.GenreId = viewModel.Movie.GenreId;
                movie.NumberInStock = viewModel.Movie.NumberInStock;
            }

            this.context.SaveChanges();
            

            return RedirectToAction("ShowMovies", "Movies");
        }

        public ActionResult Delete(int id)
        {
            var deletedCustomer = context.Movies.FirstOrDefault(s => s.Id == id);
            if (deletedCustomer != null) this.context.Movies.Remove(deletedCustomer);
            context.SaveChanges();
            return RedirectToAction("ShowMovies", "Movies");
        }
    }
}