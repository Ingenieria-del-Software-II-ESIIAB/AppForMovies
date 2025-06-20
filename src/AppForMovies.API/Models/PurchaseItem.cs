namespace AppForMovies.API.Models
{
    [PrimaryKey(nameof(MovieId), nameof(PurchaseId))]
    public class PurchaseItem
    {
        public PurchaseItem()
        {
        }

        public PurchaseItem(Movie movie, Purchase purchase, int quantity)
        {
            Movie = movie;
            MovieId = movie.Id;
            Purchase = purchase;
            PurchaseId = purchase.Id;
            Price = movie.PriceForPurchase;
            Quantity = quantity;
        }

        public Movie Movie { get; set; }

        public int MovieId { get; set; }

        public Purchase Purchase { get; set; }

        public int PurchaseId { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "You must provide a quantity higher than 1")]
        public int Quantity { get; set; }
    }
}
