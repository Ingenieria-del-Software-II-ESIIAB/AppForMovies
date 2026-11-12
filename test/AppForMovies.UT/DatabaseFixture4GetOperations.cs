namespace AppForMovies.UT;
public class DatabaseFixture4GetOperations:IDisposable
{
    // string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=AppForMovies-API;Trusted_Connection=True;ConnectRetryCount=0";

    protected readonly DbConnection _connection;
    protected readonly ApplicationDbContext _context;
    protected readonly DbContextOptions<ApplicationDbContext> _contextOptions;

    //private static readonly object _lock = new();
    //private static bool _databaseInitialized;

    public DatabaseFixture4GetOperations()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection).Options;

        // Create the schema and seed some data
        _context = new ApplicationDbContext(_contextOptions);
        if (_context.Database.EnsureCreated())
        {
            using var viewCommand = _context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
                CREATE VIEW AllResources AS
                SELECT Name
                FROM Movies;";
            viewCommand.ExecuteNonQuery();
        }


        var genres = new List<Genre>() {
            new Genre("Sci - Fi"),
            new Genre("Drama"),
            new Genre("Comedy"),
            new Genre("Soap opera")
        };

        var movies = new List<Movie>(){
            new Movie("The lord of the rings", genres[0],new DateTime(2011, 10, 20),10.0m, 5,1.0,1),
            new Movie("The mechanic orange", genres[0],new DateTime(1988, 02, 23),15.0m, 10,2.0,2),
            new Movie("The flying castle", genres[1],new DateTime(2007, 04, 04),20.0m, 15,3.0,10),
            //this movie has quantityforpurchase=0 and quantityforrenting=0 so it shouldn't be returned when 
            //quering for movies for being purchased or rented
            new Movie("The man in the high castle", genres[2],new DateTime(2015, 01, 01),10.0m,0, 4.0,0),
        };


        _context.AddRange(genres);
        _context.AddRange(movies);
        _context.SaveChanges();

        ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
        _context.Add(user);
        _context.SaveChanges();

        var purchase = new Purchase("elena@uclm.es", "Elena Navarro", user, "Avda. España s/n, Albacete",
                            DateTime.Today, new List<PurchaseItem>(), PaymentMethodTypes.CreditCard);
        var purchaseItem = new PurchaseItem(movies[movies.Count - 1], 1, purchase);
        purchase.PurchaseItems.Add(purchaseItem);

        _context.Add(purchase);
        _context.SaveChanges();

        var rental = new Rental("elena@uclm.es", "Elena Navarro",
                user, "Avda. España s/n, Albacete 02071",
                DateTime.Now, PaymentMethodTypes.CreditCard, DateTime.Today.AddDays(2), DateTime.Today.AddDays(4),
                new List<RentalItem>());
        rental.RentalItems.Add(new RentalItem(movies[0], rental, "My favourite movie"));

        _context.SaveChanges();

        _context.Add(rental);
        _context.SaveChanges();

                


    }

    //public ApplicationDbContext CreateContext()
    //    => new ApplicationDbContext(
    //        new DbContextOptionsBuilder<ApplicationDbContext>()
    //            .UseSqlServer(ConnectionString)
    //            .Options);

    public ApplicationDbContext CreateContext() => new(_contextOptions);

    public void Dispose()
    {
        
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture4GetOperations>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}