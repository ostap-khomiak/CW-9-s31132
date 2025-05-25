using ABPDCW9.DTOs;
using ABPDCW9.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABPDCW9.Controllers;


[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IDbService _service;
    public PatientsController(IDbService service) => _service = service;

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientGetDTO>> GetPatient(int id)
    {
        var dto = await _service.GetPatientAsync(id);
        return Ok(dto);
    }
}