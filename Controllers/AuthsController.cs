using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsinaOS.DTO.Auth.Request;
using UsinaOS.DTO.Auth.Response;
using UsinaOS.DTO.Comun;
using UsinaOS.Interfaces.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IAuthService _service;
   
    public AuthsController(IAuthService service)
    {
        _service = service;
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAuth InformacoesLogin)
    {
        var funcionario = await _service.LoginFuncionario(InformacoesLogin);
        if(funcionario == null)
        {
            return BadRequest();
        }
        return StatusCode(201, new ApiResponse<LoginAuthResponse>
        {
            Message = "Login realizado com susseso",
            Data = funcionario

        });
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> PostAuth(RefreshToken auth)
    {
        var resultado = await _service.RefreshToken(auth);

        return Ok(new ApiResponse<LoginAuthResponse>
        {
            Message = "Tokens renovados com sucesso.",
            Data = resultado
        });
    }

}
