using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.Shared.MovieDTOs
{
    public class MovieForRentalDTO
    {
        public int Id { get; set; }

        [StringLength(50,ErrorMessage = "Title must have a maximun length of 50 characters")]
        public string Title { get; set; }
    }
}
