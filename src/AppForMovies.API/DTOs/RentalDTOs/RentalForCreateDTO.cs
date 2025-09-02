namespace AppForMovies.API.DTOs.RentalDTOs
{
    public class RentalForCreateDTO
    {
        public RentalForCreateDTO(string customerUserName, string customerNameSurname, string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalItemDTO> rentalItems)
        {
            CustomerUserName = customerUserName ?? throw new ArgumentNullException(nameof(customerUserName));
            CustomerNameSurname = customerNameSurname ?? throw new ArgumentNullException(nameof(customerNameSurname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            RentalItems = rentalItems ?? throw new ArgumentNullException(nameof(rentalItems));
        }

        public RentalForCreateDTO()
        {
            RentalItems = new List<RentalItemDTO>();
        }

        public DateTime RentalDateFrom { get; set; }

        public DateTime RentalDateTo { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [EmailAddress]
        [Required]
        public string CustomerUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string CustomerNameSurname { get; set; }

        public IList<RentalItemDTO> RentalItems { get; set; }
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        private int NumberOfDays
        {
            get
            {
                return (RentalDateTo - RentalDateFrom).Days;
            }
        }

        [Display(Name = "Total Price")]
        [JsonPropertyName("TotalPrice")]
        public double TotalPrice
        {
            get
            {
                return RentalItems.Sum(ri => ri.PriceForRenting * NumberOfDays);
            }
        }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is RentalForCreateDTO dTO &&
                   RentalDateFrom == dTO.RentalDateFrom &&
                   RentalDateTo == dTO.RentalDateTo &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   CustomerUserName == dTO.CustomerUserName &&
                   CustomerNameSurname == dTO.CustomerNameSurname &&
                   EqualityComparer<IList<RentalItemDTO>>.Default.Equals(RentalItems, dTO.RentalItems) &&
                   PaymentMethod == dTO.PaymentMethod &&
                   NumberOfDays == dTO.NumberOfDays &&
                   TotalPrice == dTO.TotalPrice;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(RentalDateFrom);
            hash.Add(RentalDateTo);
            hash.Add(DeliveryAddress);
            hash.Add(CustomerUserName);
            hash.Add(CustomerNameSurname);
            hash.Add(RentalItems);
            hash.Add(PaymentMethod);
            hash.Add(NumberOfDays);
            hash.Add(TotalPrice);
            return hash.ToHashCode();
        }
    }
}
