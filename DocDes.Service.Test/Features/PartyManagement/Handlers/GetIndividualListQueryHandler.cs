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

public class GetIndividualListQueryHandlerTests
{
    private readonly Mock<IRepository<Individual, int>> _individualRepositoryMock;
    private readonly Mock<ILogger<GetIndividualListQueryHandler>> _loggerMock;
    private readonly GetIndividualListQueryHandler _handler;

    public GetIndividualListQueryHandlerTests()
    {
        _individualRepositoryMock = new Mock<IRepository<Individual, int>>();
        _loggerMock               = new Mock<ILogger<GetIndividualListQueryHandler>>();

        _handler = new GetIndividualListQueryHandler(
            _individualRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllIndividuals()
    {
        // Arrange
        var individuals = new List<Individual>
        {
            new() { Id = 1, GivenName = "Ahmet",  FamilyName = "Yılmaz", ValidFor = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue } },
            new() { Id = 2, GivenName = "Mehmet", FamilyName = "Demir",  ValidFor = new TimePeriod { StartDateTime = DateTime.MinValue, EndDateTime = DateTime.MaxValue } }
        };

        _individualRepositoryMock
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Individual, bool>>>(),
                It.IsAny<Func<IQueryable<Individual>, IOrderedQueryable<Individual>>>(),
                It.IsAny<Func<IQueryable<Individual>, IQueryable<Individual>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(individuals);

        // Act
        var result = await _handler.Handle(new GetIndividualListQuery(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().GivenName.Should().Be("Ahmet");
    }

    [Fact]
    public async Task Handle_EmptyList_ShouldReturnEmptyCollection()
    {
        // Arrange
        _individualRepositoryMock
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Individual, bool>>>(),
                It.IsAny<Func<IQueryable<Individual>, IOrderedQueryable<Individual>>>(),
                It.IsAny<Func<IQueryable<Individual>, IQueryable<Individual>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Individual>());

        // Act
        var result = await _handler.Handle(new GetIndividualListQuery(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEmpty();
    }
}