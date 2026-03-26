using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;
using DocDes.Service.Features.Queries;
using DocDes.Service.Features.Handlers;

namespace DocDes.Service.Tests.Features.Queries;

public class GetPartyRoleListQueryHandlerTests
{
    private readonly Mock<IRepository<PartyRole, int>> _partyRoleRepositoryMock;
    private readonly Mock<ILogger<GetPartyRoleListQueryHandler>> _loggerMock;
    private readonly GetPartyRoleListQueryHandler _handler;

    public GetPartyRoleListQueryHandlerTests()
    {
        _partyRoleRepositoryMock = new Mock<IRepository<PartyRole, int>>();
        _loggerMock              = new Mock<ILogger<GetPartyRoleListQueryHandler>>();

        _handler = new GetPartyRoleListQueryHandler(
            _partyRoleRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllPartyRoles()
    {
        // Arrange
        var partyRoles = new List<PartyRole>
        {
            new() { Id = 1, PartyId = 10, PartyRoleTypeCd = "Customer",  ValidFor = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue } },
            new() { Id = 2, PartyId = 20, PartyRoleTypeCd = "SiteAdmin", ValidFor = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue } }
        };

        _partyRoleRepositoryMock
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<PartyRole, bool>>>(),
                It.IsAny<Func<IQueryable<PartyRole>, IOrderedQueryable<PartyRole>>>(),
                It.IsAny<Func<IQueryable<PartyRole>, IQueryable<PartyRole>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(partyRoles);

        // Act
        var result = await _handler.Handle(new GetPartyRoleListQuery(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().PartyRoleTypeCd.Should().Be("Customer");
    }

    [Fact]
    public async Task Handle_EmptyList_ShouldReturnEmptyCollection()
    {
        // Arrange
        _partyRoleRepositoryMock
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<PartyRole, bool>>>(),
                It.IsAny<Func<IQueryable<PartyRole>, IOrderedQueryable<PartyRole>>>(),
                It.IsAny<Func<IQueryable<PartyRole>, IQueryable<PartyRole>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PartyRole>());

        // Act
        var result = await _handler.Handle(new GetPartyRoleListQuery(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEmpty();
    }
}