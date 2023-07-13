namespace WebAPI.Controllers.PersonController.Request
{
    public class CreatePersonRequest
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int AddressId { get; set; }
    }
}
