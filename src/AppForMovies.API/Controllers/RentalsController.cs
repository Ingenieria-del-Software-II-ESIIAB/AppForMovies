using AppForMovies.API.DTOs.RentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForMovies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(ApplicationDbContext context, ILogger<RentalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
            if (_context.Rentals == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }

            var rental = await _context.Rentals
             .Where(r => r.Id == id)
                 .Include(r => r.RentalItems) //join table RentalItems
                    .ThenInclude(ri => ri.Movie) //then join table Movies
                        .ThenInclude(movie => movie.Genre) //then join table Genre
             .Select(r => new RentalDetailDTO(r.Id, r.RentalDate, r.CustomerUserName,
                    r.CustomerNameSurname, r.DeliveryAddress,
                    (PaymentMethodTypes)r.PaymentMethod,
                    r.RentalDateFrom, r.RentalDateTo,
                    r.RentalItems
                        .Select(ri => new RentalItemDTO(ri.Movie.Id,
                                ri.Movie.Title, ri.Movie.Genre.Name,
                                ri.Movie.PriceForRenting, ri.Description)).ToList<RentalItemDTO>()))
             .FirstOrDefaultAsync();


            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        }
    }

}
