using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;
using DocDes.Service.Features.Queries;
using DocDes.Service.Features.Handlers;

namespace DocDes.Service.Tests.Features.Queries;

public class GetPartyRoleQueryHandlerTests
{
    private readonly Mock<IRepository<PartyRole, int>> _partyRoleRepositoryMock;
    private readonly Mock<ILogger<GetPartyRoleQueryHandler>> _loggerMock;
    private readonly GetPartyRoleQueryHandler _handler;

    public GetPartyRoleQueryHandlerTests()
    {
        _partyRoleRepositoryMock = new Mock<IRepository<PartyRole, int>>();
        _loggerMock              = new Mock<ILogger<GetPartyRoleQueryHandler>>();

        _handler = new GetPartyRoleQueryHandler(
            _partyRoleRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingPartyRole_ShouldReturnResponse()
    {
        // Arrange
        var existing = new PartyRole
        {
            Id              = 1,
            PartyId         = 10,
            PartyRoleTypeCd = "Customer",
            ValidFor        = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue }
        };

        _partyRoleRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        var result = await _handler.Handle(new GetPartyRoleQuery { Id = 1 }, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.PartyId.Should().Be(10);
        result.PartyRoleTypeCd.Should().Be("Customer");
    }

    [Fact]
    public async Task Handle_NonExistingPartyRole_ShouldReturnNull()
    {
        // Arrange
        _partyRoleRepositoryMock
            .Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PartyRole?)null);

        // Act
        var result = await _handler.Handle(new GetPartyRoleQuery { Id = 99 }, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldMapValidForCorrectly()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1);
        var end   = new DateTime(2025, 12, 31);

        var existing = new PartyRole
        {
            Id              = 1,
            PartyId         = 10,
            PartyRoleTypeCd = "Customer",
            ValidFor        = new TimePeriod { StartDateTime = start, EndDateTime = end }
        };

        _partyRoleRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        var result = await _handler.Handle(new GetPartyRoleQuery { Id = 1 }, TestContext.Current.CancellationToken);

        // Assert
        result.ValidFor.StartDateTime.Should().Be(start);
        result.ValidFor.EndDateTime.Should().Be(end);
    }
}