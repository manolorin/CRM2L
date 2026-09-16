using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatesController : GenericController<State>
{
    private readonly UnitsOfWork.Interfaces.IStatesUnitOfWork _statesUnitOfWork;

    public StatesController(IGenericUnitOfWork<State> unitOfWork, IStatesUnitOfWork statesUnitOfWork) : base(unitOfWork)

    {
        _statesUnitOfWork = statesUnitOfWork;
    }
    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var action = await _statesUnitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        else
        {
            return BadRequest(action.Message);
        }
    }
    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var action = await _statesUnitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        else
        {
            return NotFound(action.Message);
        }
    }
}