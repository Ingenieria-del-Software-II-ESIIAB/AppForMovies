using AppForMovies.API.DTOs.MovieDTOs;
using AppForMovies.API.Controllers;

namespace AppForMovies.UT.MoviesController_test
{
    public class GetMoviesForRental_WithClassFixture: IClassFixture<DatabaseFixture4GetOperations>
    {
        private DatabaseFixture4GetOperations Fixture { get; }

        private ApplicationDbContext _context;
        public GetMoviesForRental_WithClassFixture(DatabaseFixture4GetOperations fixture)
        {
            Fixture = fixture;
            _context = Fixture.CreateContext();

        }



        public static IEnumerable<object?[]> TestCasesFor_GetMoviesForRental_OK()
        {

            var movieDTOs = new List<MovieForRentalDTO>() {
                new MovieForRentalDTO(1,"The lord of the rings","Sci - Fi",new DateTime(2011, 10, 20),1),
                new MovieForRentalDTO(2,"The mechanic orange","Sci - Fi" , new DateTime(1988, 02, 23),2),
                new MovieForRentalDTO(3, "The flying castle", "Drama", new DateTime(2007, 04, 04), 3),
            };

            var movieDTOsTC1 = new List<MovieForRentalDTO>() { movieDTOs[1], movieDTOs[2] }
                    //the GetMoviesForPurchase method returns the movies ordered by title
                    .OrderBy(m => m.Title).ToList();


            var movieDTOsTC2 = new List<MovieForRentalDTO>() { movieDTOs[1] };
            var movieDTOsTC3 = new List<MovieForRentalDTO>() { movieDTOs[2] };

            var movieDTOsTC4 = new List<MovieForRentalDTO>() { movieDTOs[0], movieDTOs[1], movieDTOs[2] }
                //the GetMoviesForPurchase method returns the movies ordered by title
                .OrderBy(m => m.Title).ToList();

            var allTests = new List<object?[]>
            {             //filters to apply - expected movies
                                          //by default datefrom=today +1, dateto=today+2, thus movieDTOs[0] cannot be returned
                new object?[] { null, null, null, null, movieDTOsTC1,  },
                new object?[] { "mechanic", null, null, null, movieDTOsTC2, },
                new object?[] { null, "Drama", null, null, movieDTOsTC3, },
                new object?[] { null, null, DateTime.Today.AddDays(6), DateTime.Today.AddDays(8), movieDTOsTC4, },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetMoviesForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMoviesForRental_OK_test(string? filterTitle, string? filterGenre, DateTime? fromDate, DateTime? toDate,
            IList<MovieForRentalDTO> expectedMovies)
        {
            // Arrange
            var mock = new Mock<ILogger<MoviesController>>();
            ILogger<MoviesController> logger = mock.Object;
            var controller = new MoviesController(_context, logger);

            // Act
            var result = await controller.GetMoviesForRental(filterTitle, filterGenre, fromDate, toDate);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var movieDTOsActual = Assert.IsType<List<MovieForRentalDTO>>(okResult.Value);
            Assert.Equal(expectedMovies, movieDTOsActual);

        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetMoviesForRental_badrequest_test()
        {
            // Arrange
            var mock = new Mock<ILogger<MoviesController>>();
            ILogger<MoviesController> logger = mock.Object;
            var controller = new MoviesController(_context, logger);

            // Act
            var result = await controller.GetMoviesForRental(null, null, DateTime.Today.AddDays(5), DateTime.Today.AddDays(1));

            //Assert
            //we check that the response type is OK and obtain the list of movies
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("fromDate must be earlier than toDate", problem);
        }

    }
}