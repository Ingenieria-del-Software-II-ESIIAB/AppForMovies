using AppForMovies.API.DTOs.MovieDTOs;
using AppForMovies.API.Controllers;

namespace AppForMovies.UT.MoviesController_test
{
    public class GetMoviesForPurchase_test_WithClassFixture: IClassFixture<DatabaseFixture4GetOperations>
    {
        private DatabaseFixture4GetOperations Fixture { get; }

        private ApplicationDbContext _context;
        public GetMoviesForPurchase_test_WithClassFixture(DatabaseFixture4GetOperations fixture)
        {
            Fixture = fixture;
            _context = Fixture.CreateContext();

        }



    //Test case: GetMoviesForPurchase with null title and null genre
    //Expected: returns all movies ordered by title
    //We use the ClassFixture to share the same database context across tests, so we can use the same data for all tests
    

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetMoviesForPurchase_NULL_TITLE_GENRE() {
       // Arrange
       var expectedMovies = new List<MovieForPurchaseDTO>() {
           new MovieForPurchaseDTO(1,"The lord of the rings","Sci - Fi",new DateTime(2011, 10, 20),10.0m),
           new MovieForPurchaseDTO(2,"The mechanic orange","Sci - Fi" , new DateTime(1988, 02, 23),15.0m),
           new MovieForPurchaseDTO(3,"The flying castle","Drama" , new DateTime(2007, 04, 04),20.0m),
       }.OrderBy(m => m.Title).ToList();
       var mock = new Mock<ILogger<MoviesController>>();
       ILogger<MoviesController> logger = mock.Object;

       var controller = new MoviesController(_context, logger);

       // Act
       var result = await controller.GetMoviesForPurchase(null, null);

       //Assert
       //we check that the response type is OK 
       var okResult = Assert.IsType<OkObjectResult>(result);
       //and obtain the list of movies
       var movieDTOsActual = Assert.IsType<List<MovieForPurchaseDTO>>(okResult.Value);
       Assert.Equal(expectedMovies, movieDTOsActual);

    }

    public static IEnumerable<object?[]> TestCasesFor_GetMoviesForPurchase_OK() {

        var movieDTOs = new List<MovieForPurchaseDTO>() {
            new MovieForPurchaseDTO(1,"The lord of the rings","Sci - Fi",new DateTime(2011, 10, 20),10.0m),
            new MovieForPurchaseDTO(2,"The mechanic orange","Sci - Fi" , new DateTime(1988, 02, 23),15.0m),
           new MovieForPurchaseDTO(3,"The flying castle","Drama" , new DateTime(2007, 04, 04),20.0m),
       };

        var movieDTOsTC1 = movieDTOs.OrderBy(m => m.Title).ToList();
                //the GetMoviesForPurchase method returns the movies ordered by title



        var movieDTOsTC2 = new List<MovieForPurchaseDTO>() { movieDTOs[0] };
        var movieDTOsTC3 = new List<MovieForPurchaseDTO>() { movieDTOs[2] };



        var allTests = new List<object?[]>
        {             //filters to apply - expected movies
            new object?[] { null, null, movieDTOsTC1,  },
            new object?[] { "lord", null, movieDTOsTC2, },
            new object?[] { null, "Drama", movieDTOsTC3, },

        };

        return allTests;
    }

    [Theory]
    [Trait("LevelTesting", "Unit Testing")]
    [MemberData(nameof(TestCasesFor_GetMoviesForPurchase_OK))]
    public async Task GetMoviesForPurchase_filter_test(string? movieTitle, string? movieGenre, List<MovieForPurchaseDTO> expectedMovies) {
        // Arrange
        var mock = new Mock<ILogger<MoviesController>>();
        ILogger<MoviesController> logger = mock.Object;
        var controller = new MoviesController(_context, logger);

        // Act
        var result = await controller.GetMoviesForPurchase(movieTitle, movieGenre);

        //Assert
        //we check that the response type is OK 
        var okResult = Assert.IsType<OkObjectResult>(result);
        //and obtain the list of movies
        var movieDTOsActual = Assert.IsType<List<MovieForPurchaseDTO>>(okResult.Value);
        Assert.Equal(expectedMovies, movieDTOsActual);

    }

    }
}