using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;
using DocDes.Service.Features.Commands;
using DocDes.Service.Features.Handlers;
using DocDes.Service.Dtos.Requests;

namespace DocDes.Service.Tests.Features.Handlers;

public class PatchIndividualCommandHandlerTests
{
    private readonly Mock<IRepository<Individual, int>> _individualRepositoryMock;
    private readonly Mock<ILogger<PatchIndividualCommandHandler>> _loggerMock;
    private readonly PatchIndividualCommandHandler _handler;

    public PatchIndividualCommandHandlerTests()
    {
        _individualRepositoryMock = new Mock<IRepository<Individual, int>>();
        _loggerMock               = new Mock<ILogger<PatchIndividualCommandHandler>>();

        _handler = new PatchIndividualCommandHandler(
            _individualRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingIndividual_ShouldUpdateAndReturnResponse()
    {
        // Arrange
        var existing = new Individual
        {
            Id         = 1,
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            ValidFor   = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue }
        };

        var command = new PatchIndividualCommand
        {
            Id        = 1,
            GivenName = "Mehmet"
        };

        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _individualRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.GivenName.Should().Be("Mehmet");
        result.FamilyName.Should().Be("Yılmaz"); // değişmemeli
    }

    [Fact]
    public async Task Handle_NonExistingIndividual_ShouldReturnNull()
    {
        // Arrange
        var command = new PatchIndividualCommand { Id = 99 };

        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Individual?)null);

        // Act
        var result = await _handler.Handle(command, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithValidFor_ShouldUpdateDates()
    {
        // Arrange
        var existing = new Individual
        {
            Id         = 1,
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            ValidFor   = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue }
        };

        var newStart = new DateTime(2024, 1, 1);
        var newEnd   = new DateTime(2025, 12, 31);

        var command = new PatchIndividualCommand
        {
            Id       = 1,
            ValidFor = new TimePeriodRequest { StartDateTime = newStart, EndDateTime = newEnd }
        };

        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _individualRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, TestContext.Current.CancellationToken);

        // Assert
        result.ValidFor.StartDateTime.Should().Be(newStart);
        result.ValidFor.EndDateTime.Should().Be(newEnd);
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateRepositoryOnce()
    {
        // Arrange
        var existing = new Individual
        {
            Id         = 1,
            GivenName  = "Ahmet",
            FamilyName = "Yılmaz",
            ValidFor   = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue }
        };

        var command = new PatchIndividualCommand { Id = 1, GivenName = "Mehmet" };

        _individualRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _individualRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, TestContext.Current.CancellationToken);

        // Assert
        _individualRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Individual>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}