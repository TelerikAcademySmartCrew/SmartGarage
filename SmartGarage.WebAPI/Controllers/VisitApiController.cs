using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.WebAPI.Controllers;

[ApiController]
[Route("api/visits")]
public class VisitApiController : ControllerBase
{
    private readonly IVisitService visitService;
    private readonly IVisitMapper visitMapper;

    public VisitApiController(
        IVisitService visitService,
        IVisitMapper visitMapper
        )
    {
        this.visitService = visitService;
        this.visitMapper = visitMapper;
    }

    [HttpGet("users/userId")]
    public async Task<IActionResult> GetUserVisits([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var visits = await this.visitService.GetByUserIdAsync(userId, cancellationToken);
        var viewModel = this.visitMapper.ToViewModel(visits);

        if (viewModel.Any())
        {
            return Ok(viewModel);
        }
        
        return NoContent();
    }

    public async Task<IActionResult> GetVisitById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var visit = await this.visitService.GetByIdAsync(id, cancellationToken);
            var viewModel = this.visitMapper.ToViewModel(visit);

            return Ok(viewModel);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}