using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;

namespace Orders.Backend.Controllers;

public class GenericController<T> : Controller where T : class
{
    private readonly IGenericUnitOfWork<T> _unitOfWork;

    public GenericController(IGenericUnitOfWork<T> unitOfWork )
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        else {
            return BadRequest(action.Message);
        }
        
    }


    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        else {
            return NotFound();
        }
        
    }

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(T entity)
    {
        var action = await _unitOfWork.AddAsync(entity);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        else {
            return BadRequest(action.Message);
        }
        
    }

    [HttpPut]
    public virtual async Task<IActionResult> PutAsync(T entity)
    {
        var action = await _unitOfWork.UpdateAsync(entity);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        else {
            return BadRequest(action.Message);
        }
    }

    [HttpDelete]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        var action = await _unitOfWork.DeleteAsync(id);
        if (action.WasSuccess)
        {
            return NoContent();    
        }
        else
        {
            return BadRequest(action.Message);
        }
    }   
}
