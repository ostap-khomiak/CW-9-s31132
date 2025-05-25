using ABPDCW9.DTOs;
using ABPDCW9.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABPDCW9.Controllers;


[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _service;
    public PrescriptionsController(IDbService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionCreateDTO dto)
    {
        await _service.AddPrescriptionAsync(dto);
        return Ok();
    }
}