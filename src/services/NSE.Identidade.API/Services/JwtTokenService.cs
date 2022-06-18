using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Core.Extensions;
using NSE.Identidade.API.Models;
using NSE.WebAPI.Core.Identidade;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.Identidade.API.Services
{
    public class JwtTokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityConfiguration _identityConfiguration;

        public JwtTokenService(UserManager<IdentityUser> userManager, IOptions<IdentityConfiguration> identityConfiguration)
        {
            _userManager = userManager;
            _identityConfiguration = identityConfiguration.Value;
        }

        public async Task<UsuarioRespostaLogin> GerarJwt(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = await ObterClaims(user);

            var tokenSecurity = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = _identityConfiguration.ValidoEm,
                Issuer = _identityConfiguration.Emissor,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_identityConfiguration.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(_identityConfiguration.IssuerSignignKey, SecurityAlgorithms.HmacSha256)
            });

            var tokenString = tokenHandler.WriteToken(tokenSecurity);

            var response = new UsuarioRespostaLogin
            {
                AccessToken = tokenString,
                ExpiresIn = TimeSpan.FromHours(_identityConfiguration.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private async Task<IEnumerable<Claim>> ObterClaims(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnixEpochDate().ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            return claims;
        }
    }
}
