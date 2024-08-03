using Authentication.Input;
using Authentication.Repository;
using Core.Entity;
using Core.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Authentication.Services
{
    public class TokenService : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;

        public TokenService(IConfiguration configuration, ICustomerRepository customerRepository)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
        }
        public string GetToken(CustomerLoginInput customerInput)
        {
            var customer = _customerRepository.ObterTodos()
                .FirstOrDefault(c => c.Email == customerInput.Email && c.Password == customerInput.Password);

            if (customer != null)
            {
                var token = GenerateTokenJWT(customer);
                return token;
            }

            return string.Empty;
        }

        public string GenerateTokenJWT(Customer customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretJWT = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.Role, customer.PermissionType.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretJWT), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
