using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace AuthenticatedWebApi.Security 
{
    public class JWTTokenService : ITokenService
    {
        public JWTTokenService(IConfiguration configuration) 
        {
            _config = configuration;
            InitializeCrypto();
        }

        public bool Authenticate(string user, string password, out string token) 
        {
            bool result = false;
            token = null;

            List<Credential> users = _config.GetSection("Users").Get<List<Credential>>();

            if (users.Where(u => u.Username == user && u.Password == password).Count() > 0) 
            {
                string issuer = Environment.MachineName;
                SigningCredentials credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
                DateTime expiresOn = DateTime.Now.AddMinutes(VALID_FOR_MINUTES);
                Claim[] claims = new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user),
                    new Claim(ClaimTypes.Expiration, expiresOn.ToLongDateString())
                };

                JwtSecurityToken jwtToken = new JwtSecurityToken(issuer, issuer, claims, null, expiresOn, credentials);
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                result = true;
            }
            
            return result;
        }

        public AuthenticationTicket ValidateToken(string token) 
        {
            return null;
        }

        public Microsoft.IdentityModel.Tokens.SymmetricSecurityKey GetSecurityKey() 
        {
            return _securityKey;
        }
        
        private void InitializeCrypto() 
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[KEY_SIZE];
            cryptoProvider.GetBytes(randomBytes, 0, KEY_SIZE);
            _securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(randomBytes);        
        }

        private IConfiguration _config;
        private const int KEY_SIZE = 32;
        private Microsoft.IdentityModel.Tokens.SymmetricSecurityKey _securityKey;
        private const double VALID_FOR_MINUTES = 60.0;
    }    
}