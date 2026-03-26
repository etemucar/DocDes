using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Queries;
using DocDes.Core.Repository;
using DocDes.Core.Model;

namespace DocDes.Service.Features.Handlers;

public class GetIndividualQueryHandler : IRequestHandler<GetIndividualQuery, IndividualResponse>
{
    private readonly IRepository<Individual, int> _individualRepository;
    private readonly ILogger<GetIndividualQueryHandler> _logger;

    public GetIndividualQueryHandler(
        IRepository<Individual, int> individualRepository,
        ILogger<GetIndividualQueryHandler> logger)
    {
        _individualRepository = individualRepository;
        _logger               = logger;
    }

    public async Task<IndividualResponse> Handle(GetIndividualQuery request, CancellationToken cancellationToken)
    {
        var individual = await _individualRepository.GetByIdAsync(request.Id, cancellationToken);

        if (individual is null)
            return null!;

        return MapToResponse(individual);
    }

    private static IndividualResponse MapToResponse(Individual individual) => new()
    {
        Id             = individual.Id,
        GivenName      = individual.GivenName,
        FamilyName     = individual.FamilyName,
        MiddleName     = individual.MiddleName,
        Title          = individual.Title,
        Gender         = individual.Gender,
        Nationality    = individual.Nationality,
        BirthDate      = individual.BirthDate,
        PlaceOfBirth   = individual.PlaceOfBirth,
        CountryOfBirth = individual.CountryOfBirth,
        MaritalStatus  = individual.MaritalStatus,
        ValidFor = new TimePeriodResponse
        {
            StartDateTime = individual.ValidFor.StartDateTime,
            EndDateTime   = individual.ValidFor.EndDateTime
        }
    };
}