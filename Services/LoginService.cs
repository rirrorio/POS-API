using Microsoft.IdentityModel.Tokens;
using POS_API.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POS_API.Services
{
    public class LoginService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly string _secretKey;

        public LoginService(ApplicationDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        public LoginResponseDTO Authenticate(LoginRequestDTO request)
        {
            // Validate user
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDTO
            {
                Username = request.Username,
                AccessToken = tokenHandler.WriteToken(token)
            };
        }
    }
}
