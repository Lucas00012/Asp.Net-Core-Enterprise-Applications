using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Identidade.API.Models;
using NSE.Identidade.API.Services;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, JwtTokenService jwtTokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (!result.Succeeded)
            {
                AdicionarErros(result.Errors.Select(e => e.Description));
                return CustomResponse();
            }

            var token = await _jwtTokenService.GerarJwt(user);

            return CustomResponse(token);
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.IsLockedOut)
            {
                AdicionarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            if (!result.Succeeded)
            {
                AdicionarErro("Usuário ou senha incorretos");
                return CustomResponse();
            }

            var user = await _userManager.FindByEmailAsync(usuarioLogin.Email);
            var token = await _jwtTokenService.GerarJwt(user);

            return CustomResponse(token);
        }
    }
}
