using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsinaOS.Domain.Enums;
using UsinaOS.DTO.Comun;
using UsinaOS.DTO.OrdemServico.Request;
using UsinaOS.DTO.OrdemServico.Response;
using UsinaOS.Interfaces.OrdemServico;

[Route("api/[controller]")]
[ApiController]
public class OrdemServicoesController : ControllerBase
{
    private readonly IOrdemServicoService _service;
    public OrdemServicoesController(IOrdemServicoService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Administrador,Encarregado,OperadorMaquinas")]
    [HttpGet]
    public async Task<ActionResult> GetOrdemServicos([FromBody] BuscarOrdemServico filtrosOrdemServico)
    {
        
        var resultado = await _service.BuscarOsPorFuncionario(filtrosOrdemServico);
        return StatusCode(200, new ApiResponse<List<OrdemServicoResponse>>
        {
            Message = "Ordem de Serviços encontradas",
            Data = resultado

        });
             
    }

    [Authorize(Roles = "Administrador,Encarregado,OperadorMaquinas")]
    [HttpPut("{status}")]
    public async Task<IActionResult> PutStatusOrdemServico(int numeroOs, StatusOS statusNovo)
    {
        var os = await _service.AtualizarStatusOs(numeroOs, statusNovo);
        return StatusCode(200, new ApiResponse<StatusOrdemServicoResponse>
        {
            Message = "Status alterado com susseso.",
        });
    }


    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPut("{ordemServico}")]
    public async Task<IActionResult> PutOrdemServico(int numeroOs, UpdateOrdemServico informacoesOrdemServico)
    {
        var os = await _service.AtualizarOs(numeroOs, informacoesOrdemServico);
        return StatusCode(200, new ApiResponse<OrdemServicoResponse>
        {
            Message = "Ordem de Serviço atualizada com susseso.",
            Data = os
        });
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPost]
    public async Task<ActionResult> PostOrdemServico(CreateOrdemServico informacoesOrdemServico)
    {
        var os = await _service.CadastrarOs(informacoesOrdemServico);

        return StatusCode(200, new ApiResponse<OrdemServicoResponse>
        {
            Message = "Ordem de Serviço criada com susseso.",
            Data = os
        });
    }
}
