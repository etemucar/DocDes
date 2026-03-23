using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;
using DocDes.Service.Features.Commands;

namespace DocDes.Service.Features.Handlers;

public class CreateIndividualCommandHandler : IRequestHandler<CreateIndividualCommand, IndividualResponse>
{
    private readonly IRepository<Party, int>      _partyRepository;
    private readonly IRepository<Individual, int> _individualRepository;
    private readonly ILogger<CreateIndividualCommandHandler> _logger;

    public CreateIndividualCommandHandler(
        IRepository<Party, int>      partyRepository,
        IRepository<Individual, int> individualRepository,
        ILogger<CreateIndividualCommandHandler> logger)
    {
        _partyRepository      = partyRepository;
        _individualRepository = individualRepository;
        _logger               = logger;
    }

    public async Task<IndividualResponse> Handle(CreateIndividualCommand request, CancellationToken cancellationToken)
        {
            // 1. Party oluştur (abstract container)
            var party = new Party();
            await _partyRepository.AddAsync(party, cancellationToken);
    
            // 2. Individual oluştur
            var individual = new Individual
            {
                PartyId        = party.Id,
                GivenName      = request.GivenName,
                FamilyName     = request.FamilyName,
                MiddleName     = request.MiddleName,
                Title          = request.Title,
                Gender         = request.Gender,
                Nationality    = request.Nationality,
                BirthDate      = request.BirthDate,
                PlaceOfBirth   = request.PlaceOfBirth,
                CountryOfBirth = request.CountryOfBirth,
                MaritalStatus  = request.MaritalStatus,
                ValidFor = new TimePeriod
                {
                    StartDateTime = request.ValidFor?.StartDateTime ?? DateTime.MinValue,
                    EndDateTime   = request.ValidFor?.EndDateTime   ?? DateTime.MaxValue
                }
            };
    
            await _individualRepository.AddAsync(individual, cancellationToken);
    
            // SaveChanges → UnitOfWork pipeline behavior ile çağrılır
    
            _logger.LogInformation(
                "Individual oluşturuldu. PartyId: {PartyId}, IndividualId: {IndividualId}",
                party.Id, individual.Id);
    
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