using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;

        private ServicoAutenticacao _servicoAutenticacao;

        public AuthController(IConfiguration config, WHEngenhariaContext context, IMapper mapper)
        {
            _configuration = config;
            _context = context;
            _mapper = mapper;
            _servicoAutenticacao = new ServicoAutenticacao(_context, _mapper);
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(string user, string password)
        {
            var result = _servicoAutenticacao.VerificaLogin(user, password);

            if (result != null)
            {
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", result.Id.ToString()),
                    new Claim("Nome", result.Nome),
                    new Claim("Email", result.Email),
                    new Claim("Login", result.Login)
                   };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                             _configuration["Jwt:Audience"], claims,
                             expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
                return Unauthorized();
        }
    }
}
