using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // Type of encrytion so that the same key is used to encrypt and decrypt info
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(User user)
        {
            // adding the claims
            var claims = new List<Claim>    
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(JwtRegisteredClaimNames.Sid, user.Userid.ToString())
            };

            // create sign in credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // describe what goes in the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(claims),
               Expires = DateTime.Now.AddDays(7), 
               SigningCredentials = creds
            };

            // create token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // create token 
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // return written token once logged in
            return tokenHandler.WriteToken(token);
        }
    }
}