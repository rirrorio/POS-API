namespace POS_API.DTOs
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO 
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}
