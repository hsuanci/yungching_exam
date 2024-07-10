using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YungChingExam.Data.DTOs;

namespace YungChingExam.Helpers
{
    public class JWTHelper
    {
        private readonly IConfiguration _configuration;

        public JWTHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(EmployeeDto employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 從配置中讀取過期時間,並轉換為整數
            if (!int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out int expiresInMinutes))
            {
                expiresInMinutes = 30; // 默認 30 分鐘
            }

            var claims = new List<Claim>
        {
            new Claim("sub", employee.Id.ToString()), // 確保 Id 是字符串
            new Claim("role", employee.RoleTitle),       // 確保 Email 是字符串
            new Claim("name", employee.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
