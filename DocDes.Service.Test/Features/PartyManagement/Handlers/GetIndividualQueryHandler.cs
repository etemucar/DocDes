using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;
using DocDes.Service.Features.Queries;
using DocDes.Service.Features.Handlers;

namespace DocDes.Service.Tests.Features.Queries;

public class GetIndividualQueryHandlerTests
{
    private readonly Mock<IRepository<Individual, int>> _individualRepositoryMock;
    private readonly Mock<ILogger<GetIndividualQueryHandler>> _loggerMock;
    private readonly GetIndividualQueryHandler _handler;

    public GetIndividualQueryHandlerTests()
    {
        _individualRepositoryMock = new Mock<IRepository<Individual, int>>();
        _loggerMock               = new Mock<ILogger<GetIndividualQueryHandler>>();

        _handler = new GetIndividualQueryHandler(
            _individualRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingIndividual_ShouldReturnResponse()
    {
        // Arrange
        var existing = new Individual
        {
            Id         = 1,
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            Gender     = "Male",
            ValidFor   = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue }
        };

        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        var result = await _handler.Handle(new GetIndividualQuery { Id = 1 }, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.GivenName.Should().Be("Ahmet");
        result.FamilyName.Should().Be("Yılmaz");
        result.Gender.Should().Be("Male");
    }

    [Fact]
    public async Task Handle_NonExistingIndividual_ShouldReturnNull()
    {
        // Arrange
        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Individual?)null);

        // Act
        var result = await _handler.Handle(new GetIndividualQuery { Id = 99 }, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldMapValidForCorrectly()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1);
        var end   = new DateTime(2025, 12, 31);

        var existing = new Individual
        {
            Id         = 1,
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            ValidFor   = new TimePeriod { StartDateTime = start, EndDateTime = end }
        };

        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        var result = await _handler.Handle(new GetIndividualQuery { Id = 1 }, TestContext.Current.CancellationToken);

        // Assert
        result.ValidFor.StartDateTime.Should().Be(start);
        result.ValidFor.EndDateTime.Should().Be(end);
    }
}