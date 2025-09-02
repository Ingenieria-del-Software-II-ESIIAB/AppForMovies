
namespace AppForMovies.API.DTOs.RentalDTOs
{
    public class RentalDetailDTO : RentalForCreateDTO
    {
        public RentalDetailDTO(int id, DateTime rentalDate, string customerUserName, string customerNameSurname,
            string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime rentalDateFrom,
            DateTime rentalDateTo, IList<RentalItemDTO> rentalItems)
            : base(customerUserName,
                   customerNameSurname,
                   deliveryAddress,
                   paymentMethod,
                   rentalDateFrom,
                   rentalDateTo,
                   rentalItems)
        {
            Id = id;
            RentalDate = rentalDate;
        }
        public int Id { get; set; }

        public DateTime RentalDate { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RentalDetailDTO dTO &&
                   base.Equals(obj) &&
                   RentalDateFrom == dTO.RentalDateFrom &&
                   RentalDateTo == dTO.RentalDateTo &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   CustomerUserName == dTO.CustomerUserName &&
                   CustomerNameSurname == dTO.CustomerNameSurname &&
                   EqualityComparer<IList<RentalItemDTO>>.Default.Equals(RentalItems, dTO.RentalItems) &&
                   PaymentMethod == dTO.PaymentMethod &&
                   TotalPrice == dTO.TotalPrice &&
                   Id == dTO.Id &&
                   RentalDate == dTO.RentalDate;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(RentalDateFrom);
            hash.Add(RentalDateTo);
            hash.Add(DeliveryAddress);
            hash.Add(CustomerUserName);
            hash.Add(CustomerNameSurname);
            hash.Add(RentalItems);
            hash.Add(PaymentMethod);
            hash.Add(TotalPrice);
            hash.Add(Id);
            hash.Add(RentalDate);
            return hash.ToHashCode();
        }
    }
}
