using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]

public class 
    ProspectsController : GenericController<Prospect>
{
    private readonly IProspectsUnitOfWork _prospectsUnitOfWork;

    public ProspectsController(IGenericUnitOfWork<Prospect> unit, IProspectsUnitOfWork prospectsUnitOfWork) : base(unit)
    {
        _prospectsUnitOfWork = prospectsUnitOfWork;
    }

    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> PostAsync(Prospect prospect)
    {
        var action = await _prospectsUnitOfWork.AddAsync(prospect);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [AllowAnonymous]
    [HttpPut]
    public override async Task<IActionResult> PutAsync(Prospect prospect)
    {
        try
        {
            var action = await _prospectsUnitOfWork.UpdateAsync(prospect);

            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }
        catch( Exception e) {
            return BadRequest(e.Message);
        }

        
    }


    [AllowAnonymous]
    [HttpGet("combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _prospectsUnitOfWork.GetComboAsync());
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _prospectsUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecords")]
    public override async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _prospectsUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var action = await _prospectsUnitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }
}
