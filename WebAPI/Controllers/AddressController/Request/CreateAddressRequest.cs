namespace WebAPI.Controllers.AddressController.Request
{
    public class CreateAddressRequest
    {
        public int AddressId { get; set; }
        public int CityId { get; set; }
        public string StreetAdress { get; set; }
        public string ZipCode { get; set; }
    }
}
