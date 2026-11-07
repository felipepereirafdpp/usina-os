using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsinaOS.DTO.Cliente.Request;
using UsinaOS.DTO.Cliente.Response;
using UsinaOS.DTO.Comun;
using UsinaOS.Interfaces.Cliente;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    public ClientesController(IClienteService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpGet]
    public async Task<ActionResult> GetClientes()
    {
        var clientes = await _service.ListarClientes();
        return Ok(new ApiResponse<List<ClienteResponse>>
        {
            Message = "Clientes pesquisados com susseso",
            Data = clientes
        });
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpGet("{buscar}")]
    public async Task<ActionResult> GetCliente([FromQuery] Guid? idCliente,[FromQuery] string? cnpjCliente)
    {
        var cliente = await _service.BuscarClientePorIdPorCnpj(idCliente, cnpjCliente);
        return StatusCode(201, new ApiResponse<ClienteResponse>
        {
            Message = "Clientes pesquisado com susseso",
            Data = cliente
        });

    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPut]
    public async Task<IActionResult> PutCliente(string cnpjCliente, UpdateCliente InformacoesCliente)
    {
        var cliente = await _service.AtualizarCliente(cnpjCliente, InformacoesCliente);
        
        return StatusCode(201, new ApiResponse<ClienteResponse>
        {
            Message = "Clientes pesquisado com susseso",
            Data = cliente
        });
    }

    [Authorize(Roles = "Administrador,Encarregado")]
    [HttpPost]
    public async Task<ActionResult> PostCliente(CreateCliente InformacoesCliente)
    {
        var cliente = await _service.CadastrarCliente(InformacoesCliente);

        return StatusCode(201, new ApiResponse<ClienteResponse>
        {
            Message = "Clientes cadastrado com susseso",
            Data = cliente
        });
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCliente(string cnpjCliente)
    {
        var cliente = await _service.DeletarCliente(cnpjCliente);

        if (cliente == true)
        {
            return Ok();
        }

        return StatusCode(404);
    }

   
}
