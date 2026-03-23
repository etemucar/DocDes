using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Service.Features.Commands;
using DocDes.Service.Features.Handlers;

namespace DocDes.Service.Tests.Features.Handlers;

public class CreateIndividualCommandHandlerTests
{
    private readonly Mock<IRepository<Party, int>>      _partyRepositoryMock;
    private readonly Mock<IRepository<Individual, int>> _individualRepositoryMock;
    private readonly Mock<ILogger<CreateIndividualCommandHandler>> _loggerMock;
    private readonly CreateIndividualCommandHandler _handler;

    public CreateIndividualCommandHandlerTests()
    {
        _partyRepositoryMock      = new Mock<IRepository<Party, int>>();
        _individualRepositoryMock = new Mock<IRepository<Individual, int>>();
        _loggerMock               = new Mock<ILogger<CreateIndividualCommandHandler>>();

        _handler = new CreateIndividualCommandHandler(
            _partyRepositoryMock.Object,
            _individualRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreatePartyAndIndividual()
    {
        // Arrange
        var command = new CreateIndividualCommand
        {
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            Gender     = "Male",
            BirthDate  = new DateTime(1990, 1, 1)
        };

        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Callback<Party, CancellationToken>((party, _) => party.Id = 1)
            .Returns(Task.CompletedTask);

        _individualRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Callback<Individual, CancellationToken>((individual, _) => individual.Id = 10)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.GivenName.Should().Be(command.GivenName);
        result.FamilyName.Should().Be(command.FamilyName);
        result.Gender.Should().Be(command.Gender);
        result.BirthDate.Should().Be(command.BirthDate);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallPartyRepositoryOnce()
    {
        // Arrange
        var command = new CreateIndividualCommand
        {
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz"
        };

        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _individualRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _partyRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateIndividualWithCorrectPartyId()
    {
        // Arrange
        var command = new CreateIndividualCommand
        {
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz"
        };

        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Callback<Party, CancellationToken>((party, _) => party.Id = 42)
            .Returns(Task.CompletedTask);

        Individual capturedIndividual = null!;
        _individualRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Callback<Individual, CancellationToken>((individual, _) => capturedIndividual = individual)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedIndividual.Should().NotBeNull();
        capturedIndividual.PartyId.Should().Be(42);
    }

    [Fact]
    public async Task Handle_WithoutValidFor_ShouldSetMinMaxDateTime()
    {
        // Arrange
        var command = new CreateIndividualCommand
        {
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            ValidFor   = null
        };

        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        Individual capturedIndividual = null!;
        _individualRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Callback<Individual, CancellationToken>((individual, _) => capturedIndividual = individual)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedIndividual.ValidFor.StartDateTime.Should().Be(DateTime.MinValue);
        capturedIndividual.ValidFor.EndDateTime.Should().Be(DateTime.MaxValue);
    }

    [Fact]
    public async Task Handle_WithValidFor_ShouldMapDatesCorrectly()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate   = new DateTime(2025, 12, 31);

        var command = new CreateIndividualCommand
        {
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            ValidFor   = new DocDes.Service.Dtos.Requests.TimePeriodRequest
            {
                StartDateTime = startDate,
                EndDateTime   = endDate
            }
        };

        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        Individual capturedIndividual = null!;
        _individualRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Callback<Individual, CancellationToken>((individual, _) => capturedIndividual = individual)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedIndividual.ValidFor.StartDateTime.Should().Be(startDate);
        capturedIndividual.ValidFor.EndDateTime.Should().Be(endDate);
    }
}