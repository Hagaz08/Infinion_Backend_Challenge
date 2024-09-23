namespace Infinion.Core.DTOs
{
    public class UserRegistrationResponseDto
    {
        public string id { get; set; }= "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public IList<string> Roles { get; set; }
        public string PhoneNumber { get; set; } = "";


    }
}
