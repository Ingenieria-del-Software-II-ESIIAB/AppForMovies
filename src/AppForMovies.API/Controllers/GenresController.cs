namespace AppForMovies.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase {
        private readonly ApplicationDbContext _context;
        private ILogger<GenresController> _logger;

        public GenresController(ApplicationDbContext context, ILogger<GenresController> logger) {
            _context = context;
            _logger = logger;
        }

        // GET: api/Movies/GetMoviesForPurchase
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetGenres(string? genreName) {

            IList<string> genres = await _context.Genres
                .Where(genre => (genreName == null || genre.Name.Contains(genreName))) // where clause             
                .OrderBy(genre => genre.Name)
                .Select(genre => genre.Name)
                .ToListAsync();

            return Ok(genres);
        }
    }
}
