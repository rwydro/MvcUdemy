using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Genre> GenreList{ get; set; }

        public string Title => this.Movie != null && this.Movie.Id != 0 ? "Edit movie" : "New movie";
    }
}