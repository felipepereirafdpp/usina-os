using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Comun;
using UsinaOS.DTO.Funcionario.Request;
using UsinaOS.DTO.Funcionario.Response;
using UsinaOS.Interfaces.Funcionario;

[ApiController]
[Route("api/[controller]")]
public class FuncionariosController : ControllerBase
{
    

    private readonly IFuncionarioService _service;

    public FuncionariosController(IFuncionarioService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpGet]
    public async Task<ActionResult> GetFuncionario()
    {

        var funcionarios = await _service.ListarFuncionarios();
        return Ok(new ApiResponse<List<FuncionarioResponse>>
        {
            Message = "Funcionarios encontrados com susseso",
            Data = funcionarios
            
        });
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpGet("{filtro}")]
    public async Task<ActionResult<FuncionarioEntitie>> GetFuncionario(Guid? id, string? cpf)
    {
       
        if (id == null || id == Guid.Empty && cpf == null)
        {
            return BadRequest();
        }
        var funcionario = await _service.BuscarFuncionarioPorIdPorCpf(id , cpf);
      
        return Ok(new ApiResponse<FuncionarioResponse>
        {
            Message = "Funcionario encontrado com susseso",
            Data = funcionario

        });

    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPut]
    public async Task<IActionResult> PutFuncionario(string cpf, UpdateFuncionario Infofuncionario)
    {
        
        var funcionario = await _service.AtualizarFuncionario(cpf, Infofuncionario);

        return Ok(new ApiResponse<FuncionarioResponse>
        {
            Message = "Funcionario atualizado com susseso",
            Data = funcionario

        });
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPost]
    public async Task<ActionResult<FuncionarioEntitie>> PostFuncionario(CreateFuncionario Infofuncionario)
    {
        var funcionario = await _service.CadastrarFuncionario(Infofuncionario);
        return StatusCode(201,new ApiResponse<FuncionarioResponse>
        {
            Message = "Funcionario cadastrado com susseso",
            Data = funcionario

        });
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete]
    public async Task<IActionResult> DeleteFuncionario(string cpf)
    {
        var funcionario = await _service.DeletarFuncionario(cpf);

        if (funcionario == true)
        {
            return Ok();
        }
        
         return StatusCode(404);
        
    }

    
}
