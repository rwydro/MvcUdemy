using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;
using String = System.String;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private MyDbContext context;
        //private List<Movie> MoviesList = new List<Movie>()
        //{
        //    new Movie(){Name = "Shrek"},
        //    new Movie(){Name = "Ice Age"}
        //};

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
            return Content("id=" + id);
        }

        [Route("movies/released/{year}/{month:regex(^\\d{2}$):range(1,12)}")]
        public ActionResult ByReleasedYear(int year, int month)
        {
            var movie = new Movie() { Name = "Shrek!" };
            return Content($"{movie.Name} was released {month}-{year}");
        }

        [Route("movies/{name}")]
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
    }
}