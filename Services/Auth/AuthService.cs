using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Auth.Request;
using UsinaOS.DTO.Auth.Response;
using UsinaOS.Interfaces.Auth;

namespace UsinaOS.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UsinaOSContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(UsinaOSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }





        private string GerarToken(AuthEntitie auth)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, auth.FuncionarioId.ToString()),
                new Claim(ClaimTypes.Role, auth.Funcionario.Cargo.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)
            );

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpirationMinutes"]!)
                ),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string HashToken(string token)
        {
            return Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(token))
            );
        }






        private async Task<LoginAuthResponse> Atualizatoken(AuthEntitie auth)
        {
            string accessToken = GerarToken(auth);

            string refreshToken = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64)
            );

            DateTime expiracao = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpirationMinutes"]!)
            );

            auth.RefreshTokenHash = HashToken(refreshToken);

            auth.RefreshTokenExpiraEm = DateTime.UtcNow.AddDays(
                int.Parse(_configuration["Jwt:RefreshTokenDays"]!)
            );

            await _context.SaveChangesAsync();

            return new LoginAuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiraEm = expiracao
            };
        }






        public async Task<LoginAuthResponse> LoginFuncionario(LoginAuth dadosLogin)
        {
            if (string.IsNullOrWhiteSpace(dadosLogin.CPF) || string.IsNullOrWhiteSpace(dadosLogin.SenhaHash))
            {
                throw new UnauthorizedAccessException("CPF e senha são obrigatórios.");
            }

            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Cpf == dadosLogin.CPF);


            if (funcionario == null ||
                !BCrypt.Net.BCrypt.Verify(dadosLogin.SenhaHash, funcionario.SenhaHash))
            {
                throw new UnauthorizedAccessException("CPF ou senha inválidos.");
            }

            var auth = await _context.Auth
                .Include(a => a.Funcionario)
                .FirstOrDefaultAsync(a => a.FuncionarioId == funcionario.Id);

            if (auth == null)
                throw new UnauthorizedAccessException("Autenticação não encontrada.");

            return await Atualizatoken(auth);
        }





        public async Task<LoginAuthResponse> RefreshToken(RefreshToken dadosRefresh)
        {
            string refreshTokenHash = HashToken(dadosRefresh.RefreshT);

            var auth = await _context.Auth
                .Include(a => a.Funcionario)
                .FirstOrDefaultAsync(a => a.RefreshTokenHash == refreshTokenHash);

            if (auth == null || auth.RefreshTokenExpiraEm == null || auth.RefreshTokenExpiraEm <= DateTime.UtcNow)

            {
                throw new UnauthorizedAccessException("Refresh token inválido ou expirado.");
            }

            return await Atualizatoken(auth);
        }
    }
}
