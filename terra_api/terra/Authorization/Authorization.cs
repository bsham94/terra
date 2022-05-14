using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace terra
{
    public static class Authorization
    {
        public static string GetToken()
        {
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            SigningCredentials signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:54357",
                audience: "http://localhost",
                //Claims are a list of user roles that are authorized.
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
