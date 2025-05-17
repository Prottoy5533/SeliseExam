using EmployeeManagement.Application.Common.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Feature.Authentication
{
    public static class TokenHelper
    {
        private static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == AppConstants.Environment.Development;
        public static string GenerateJwtToken(IConfiguration configuration, string email, string roles)
        {

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(ConfigurationHelper.GetConfigValue(configuration, "Jwt:Key", IsDevelopment)));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, roles)
                };

            var token = new JwtSecurityToken(
                issuer: ConfigurationHelper.GetConfigValue(configuration, "Jwt:Issuer", IsDevelopment),
                audience: ConfigurationHelper.GetConfigValue(configuration, "Jwt:Issuer", IsDevelopment),
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(ConfigurationHelper.GetConfigValue(configuration, "Jwt:ExpiryMinutes", true))),
                signingCredentials: credentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
        public static string GenerateJwtToken(
            string key,
            string issuer,
            string audience,
            int expiryMinutes,
            IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
