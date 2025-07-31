using AppForMovies.Shared.MovieDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForMovies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MovieForRentalDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetMoviesForRental(string? movieTitle, string? movieGenre, DateTime? fromDate, DateTime? toDate)
        {
            //    var movies = await _context.Movies
            //        .Include(m=>m.Genre)
            //        .Include(m=>m.RentalItems)
            //            .ThenInclude(ri=>ri.Rent)
            //        .Where(m=>((m.Title.Contains(title)) || (title==null))
            //            && ((m.Genre.Name.Equals(genre)) || (genre==null)) 
            //            && (m.RentalItems.Count(ri=>ri.Rent.RentalDateFrom<=to
            //                && ri.Rent.RentalDateTo>=from) < m.QuantityForRenting))
            //        .OrderBy(m=>m.Title)
            //            .ThenBy(m=>m.PriceForRenting)
            //        .Select(m=>new MovieForRentalDTO(m.Id, m.Title, m.Genre.Name,
            //                    m.ReleaseDate, m.PriceForPurchase,
            //                    m.RentalItems.Max(ri=>ri.Rent.RentalDate)))
            //        .ToListAsync();
            //    return Ok(movies);
            //}
            if (fromDate != null && toDate != null && fromDate > toDate)
            {
                //return BadRequest( Problem("fromDate must be earlier than toDate", 
                //    $"fromDate ({fromDate}) toDate({toDate})", 400,"Bad Request", 
                //    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"));
                ModelState.AddModelError("fromDate&toDate", "fromDate must be earlier than toDate");
                _logger.LogError($"{DateTime.Now} Error: fromDate must be earlier than toDate");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //if not renting dates are provided a value by default is assigned
            fromDate = fromDate == null ? DateTime.Today.AddDays(1) : fromDate;
            toDate = toDate == null ? DateTime.Today.AddDays(2) : toDate;

            IList<MovieForRentalDTO> selectMovies = await _context.Movies

                //join table Movie and table Genre
                .Include(m => m.Genre)

                //join tables Movie and PurchaseItem and then Purchase
                //more info about lazy loading and eager loading https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
                .Include(m => m.RentalItems).ThenInclude(ri => ri.Rent)

                .Where(m => // where clause
                   (movieTitle == null || m.Title.Contains(movieTitle)) //in case user has provided a title
                    && (movieGenre == null || m.Genre.Name.Equals(movieGenre))
                    //we check that it has not 
                    && (m.RentalItems.Where(ri => ri.Rent.RentalDateFrom <= toDate
                                            && ri.Rent.RentalDateTo >= fromDate).Count() < m.QuantityForRenting)

                    )

                .OrderBy(m => m.Title)

                .Select(m => new MovieForRentalDTO(m.Id, m.Title, m.Genre.Name, m.ReleaseDate, m.PriceForRenting))
                .ToListAsync();

            return Ok(selectMovies);
        }

    }
}
