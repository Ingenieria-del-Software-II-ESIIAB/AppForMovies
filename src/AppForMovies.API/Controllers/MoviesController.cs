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
        public async Task<ActionResult> GetMoviesForRental(string? title, string? genre, DateTime from, DateTime to)
        {
            var movies = await _context.Movies
                .Include(m=>m.Genre)
                .Include(m=>m.RentalItems)
                    .ThenInclude(ri=>ri.Rent)
                .Where(m=>((m.Title.Contains(title)) || (title==null))
                    && ((m.Genre.Name.Equals(genre)) || (genre==null)) 
                    && (m.RentalItems.Count(ri=>ri.Rent.RentalDateFrom<=to
                        && ri.Rent.RentalDateTo>=from) < m.QuantityForRenting))
                .OrderBy(m=>m.Title)
                    .ThenBy(m=>m.PriceForRenting)
                .Select(m=>new MovieForRentalDTO(m.Id, m.Title, m.Genre.Name,
                            m.ReleaseDate, m.PriceForPurchase,
                            m.RentalItems.Max(ri=>ri.Rent.RentalDate)))
                .ToListAsync();
            return Ok(movies);
        }

    }
}
