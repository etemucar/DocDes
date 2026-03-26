using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocDes.Api.Models.TMFOpenApi5;
using DocDes.Service.Features.Commands;
using DocDes.Service.Dtos.Requests;
using AutoMapper;
using DocDes.Service.Features.Queries;
using ECommerce.Api.Extensions;

namespace DocDes.Api.Controllers.TMFOpenApi5;

/// <summary>
/// TMF632 - Party Management API v5 / Individual
/// </summary>
[Authorize]
[Route("party-management/v5/individual")]
[ApiController]
[Produces("application/json")]
public class IndividualController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IndividualController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>Creates an Individual</summary>
    [HttpPost]
    [ProducesResponseType(typeof(IndividualModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] IndividualModel model)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var command = _mapper.Map<CreateIndividualCommand>(model);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<IndividualModel>(result);
        response.Href = Url.BuildHref(result.Id, nameof(GetById));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    /// <summary>Retrieves an Individual by id</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IndividualModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetIndividualQuery { Id = id });

        if (result is null)
            return NotFound();

        var response  = _mapper.Map<IndividualModel>(result);
        response.Href = Url.BuildHref(result.Id, nameof(GetById));

        return Ok(response);
    }

    /// <summary>List or find Individual objects</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IndividualModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int?    offset,
        [FromQuery] int?    limit,
        [FromQuery] string? fields)
    {
        var result = await _mediator.Send(new GetIndividualListQuery
        {
            Offset = offset ?? 0,
            Limit  = limit  ?? 20,
            Fields = fields
        });

        return Ok(result);
    }

    /// <summary>Updates partially an Individual (JSON Merge Patch)</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(IndividualModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] IndividualModel model)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var command = new PatchIndividualCommand
        {
            Id             = id,
            GivenName      = model.GivenName,
            FamilyName     = model.FamilyName,
            MiddleName     = model.MiddleName,
            Title          = model.Title,
            Gender         = model.Gender,
            Nationality    = model.Nationality,
            BirthDate      = model.BirthDate,
            MaritalStatus  = model.MaritalStatus,
            ValidFor = model.ValidFor is not null
                ? new TimePeriodRequest
                {
                    StartDateTime = model.ValidFor.StartDateTime,
                    EndDateTime   = model.ValidFor.EndDateTime
                }
                : null
        };

        var result = await _mediator.Send(command);

        if (result is null)
            return NotFound();
        var response  = _mapper.Map<IndividualModel>(result);
        response.Href = Url.BuildHref(result.Id, nameof(GetById));

        return Ok(response);
    }

    /// <summary>Deletes an Individual</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await _mediator.Send(new DeleteIndividualCommand { Id = id });

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}