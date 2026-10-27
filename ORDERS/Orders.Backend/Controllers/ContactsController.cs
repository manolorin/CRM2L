using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.Helpers;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]

public class ContactsController : GenericController<Contact>
{

    private readonly IContactsUnitOfWork _contactsUnitOfWork;
    private readonly string _container;

    private readonly IFileStorage _fileStorage;

    public ContactsController(IGenericUnitOfWork<Contact> unit, IContactsUnitOfWork contactsUnitOfWork, IFileStorage fileStorage) : base(unit)
    {
        _contactsUnitOfWork = contactsUnitOfWork;
        _fileStorage = fileStorage;
        _container = "contacts";
    }


    [HttpPost]
    public override async Task<IActionResult> PostAsync(Contact contact)
    {
        var action = await _contactsUnitOfWork.AddAsync(contact);

        if (action.WasSuccess)
        {

            if (!string.IsNullOrEmpty(contact.Photo))
            {
                var photoContact = Convert.FromBase64String(contact.Photo);
                contact.Photo = await _fileStorage.SaveFileAsync(photoContact, ".jpg", _container);
            }
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public override async Task<IActionResult> PutAsync(Contact contact)
    {
        try
        {
            var action = await _contactsUnitOfWork.GetAsync(contact.Id);
            if (!action.WasSuccess)
            { 
                return NotFound(); 
            }

            if (!string.IsNullOrEmpty(contact.Photo))
            {
                var photoContact = Convert.FromBase64String(contact.Photo);
                contact.Photo = await _fileStorage.SaveFileAsync(photoContact, ".jpg", _container);
            }
        
            action.Result.Id = contact.Id;
            action.Result.Address = contact.Address;
            action.Result.CityId = contact.CityId;
            action.Result.Email = contact.Email;
            action.Result.LastName = contact.LastName;
            action.Result.Name = contact.Name;
            action.Result.ProspectId = contact.ProspectId;
            action.Result.PhoneNumber = contact.PhoneNumber;
            action.Result.Photo = contact.Photo;
          ;


            var action2 = await _contactsUnitOfWork.UpdateAsync(action.Result);

            if (action2.WasSuccess)
            {

                return Ok(action2.Result);
            }
            return BadRequest(action2.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("combo/{prospectId:int}")]
    public async Task<IActionResult> GetComboAsync(int prospectId)
    {
        return Ok(await _contactsUnitOfWork.GetComboAsync(prospectId));
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _contactsUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecords")]
    public override async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _contactsUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _contactsUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var action = await _contactsUnitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }

}
