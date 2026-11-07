using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsinaOS.Domain.Entities;
using UsinaOS.DTO.Comun;
using UsinaOS.DTO.Peca.Request;
using UsinaOS.DTO.Peca.Response;
using UsinaOS.Interfaces.Peca;

[Route("api/[controller]")]
[ApiController]
public class PecasController : ControllerBase
{
    private readonly IPecaService _service;
    public PecasController(IPecaService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Administrador,Encarregado,OperadorMaquinas")]
    [HttpGet]
    public async Task<ActionResult> GetPecas()
    {
        var pecas = await _service.ListarPecas();
        return Ok(new ApiResponse<List<PecaResponse>>
        {
            Message = "Funcionarios encontrados com susseso",
            Data = pecas

        });
    }

    [Authorize(Roles = "Administrador,Encarregado,OperadorMaquinas")]
    [HttpGet("{filtro}")]
    public async Task<ActionResult<PecaEntitie>> GetPeca(Guid? id, string? codigoPeca, string? nome)
    {
        var peca = await _service.BuscarPecaPorIdPorCodigoPorNome(id,codigoPeca,nome);
        return StatusCode(210,new ApiResponse<PecaResponse>
        {
            Message = "Funcionario encontrado com susseso",
            Data = peca

        });
        

        
    }
    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPeca(string codigoPeca, UpdatePeca informacoesPeca)
    {
        var clinte = await _service.AtualizarPeca(codigoPeca, informacoesPeca);
        return StatusCode(201, new ApiResponse<PecaResponse>
        {
            Message = "Peça atualizada com susseso.",
            Data = clinte

        });
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPost]
    public async Task<IActionResult> PostPeca(CreatePeca informacoesPeca)
    {
        var clinete = await _service.CadastrarPeca(informacoesPeca);
        return StatusCode(201, new ApiResponse<PecaResponse>
        {
            Message = "Peça cadastrada com susseso.",
            Data = clinete

        });
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePeca(string codigoPeca)
    {
        var peca = await _service.DeletarPeca(codigoPeca);

        if (peca == true)
        {
            return Ok();
        }

        return StatusCode(404);
    }


}
