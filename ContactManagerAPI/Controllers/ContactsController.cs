using AutoMapper;
using ContactManagerAPI.DTOs;
using ContactManagerBLL.Interfaces;
using ContactManagerBLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactService contactService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetAll()
    {
        var models = await contactService.GetAllAsync();
        return Ok(mapper.Map<IEnumerable<ContactDto>>(models));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetById(int id)
    {
        var model = await contactService.GetByIdAsync(id);
        if (model is null) return NotFound();
        return Ok(mapper.Map<ContactDto>(model));
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContactDto dto)
    {
        var model = mapper.Map<ContactModel>(dto);
        await contactService.AddAsync(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ContactDto dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch");

        var model = mapper.Map<ContactModel>(dto);
        await contactService.UpdateAsync(model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await contactService.DeleteAsync(id);
        return Ok();
    }
}