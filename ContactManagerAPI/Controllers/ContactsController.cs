using AutoMapper;
using ContactManagerAPI.DTOs;
using ContactManagerBLL.Interfaces;
using ContactManagerBLL.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CsvImportResultDto>> UploadCsv([FromForm] FileUploadDto dto, CancellationToken ct)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("File is required.");

        var ext = Path.GetExtension(dto.File.FileName).ToLowerInvariant();
        
        if (ext != ".csv" && ext != ".pdf")
            return BadRequest("Only .csv or .pdf files are supported.");

        if (ext == ".csv")
        {
            using var stream = dto.File.OpenReadStream();
            var (imported, errors) = await contactService.ImportCsvAsync(stream, ct);

            return Ok(new CsvImportResultDto
            {
                Imported = imported,
                Errors = errors
            });
        }

        if (ext == ".pdf")
        {
            return Ok(new
            {
                Message = "PDF uploaded successfully",
                FileName = dto.File.FileName,
                Size = dto.File.Length
            });
        }

        return BadRequest("Unsupported file type.");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await contactService.DeleteAsync(id);
        return Ok();
    }
}