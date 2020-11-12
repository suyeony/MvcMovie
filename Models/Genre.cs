using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MvcMovie.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public List<Movie> Movies { get; set; }
        
    }
}