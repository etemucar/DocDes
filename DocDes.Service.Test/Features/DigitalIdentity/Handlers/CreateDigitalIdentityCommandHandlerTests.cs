using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.Security;
using DocDes.Core.Enums;
using DocDes.Service.Features.Commands;
using DocDes.Service.Features.Handlers;
using DocDes.Service.Dtos.Requests;

namespace DocDes.Service.Tests.Features.Handlers;

public class CreateDigitalIdentityCommandHandlerTests
{
    private readonly Mock<IRepository<DigitalIdentity, Guid>> _digitalIdentityRepositoryMock;
    private readonly Mock<IRepository<ApplicationUser, int>>  _userRepositoryMock;
    private readonly Mock<IRepository<PartyRole, int>>        _partyRoleRepositoryMock;
    private readonly Mock<IPasswordHasher>                    _passwordHasherMock;
    private readonly Mock<ILogger<CreateDigitalIdentityCommandHandler>> _loggerMock;
    private readonly CreateDigitalIdentityCommandHandler      _handler;

    public CreateDigitalIdentityCommandHandlerTests()
    {
        _digitalIdentityRepositoryMock = new Mock<IRepository<DigitalIdentity, Guid>>();
        _userRepositoryMock            = new Mock<IRepository<ApplicationUser, int>>();
        _partyRoleRepositoryMock       = new Mock<IRepository<PartyRole, int>>();
        _passwordHasherMock            = new Mock<IPasswordHasher>();
        _loggerMock                    = new Mock<ILogger<CreateDigitalIdentityCommandHandler>>();

        _handler = new CreateDigitalIdentityCommandHandler(
            _digitalIdentityRepositoryMock.Object,
            _userRepositoryMock.Object,
            _partyRoleRepositoryMock.Object,
            _passwordHasherMock.Object,
            _loggerMock.Object);
    }

    // ── Helpers ─────────────────────────────────────────────────────────

    private void SetupPartyRole(int partyRoleId = 1, int partyId = 10)
    {
        _partyRoleRepositoryMock
            .Setup(x => x.FindOneAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<PartyRole, bool>>>(),
                It.IsAny<Func<IQueryable<PartyRole>, IQueryable<PartyRole>>?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PartyRole { Id = partyRoleId, PartyId = partyId });
    }

    private CreateDigitalIdentityCommand BuildCommand(
        int partyRoleId = 1,
        string? nickname = "john_doe",
        string password = "Test1234!")
    {
        return new CreateDigitalIdentityCommand
        {
            Nickname    = nickname,
            PartyRoleId = partyRoleId,
            Credentials = new List<CredentialRequest>
            {
                new()
                {
                    CredentialType  = CredentialType.Password,
                    TrustLevel      = 1,
                    Characteristics = new List<CredentialCharacteristicRequest>
                    {
                        new() { Name = "password", Value = password }
                    },
                    ContactMedia = new List<ContactMediumRequest>
                    {
                        new()
                        {
                            MediumType  = "EmailAddress",
                            Preferred   = true,
                            Characteristic = new Dictionary<string, object>
                            {
                                { "emailAddress", "john@example.com" }
                            }
                        }
                    }
                }
            }
        };
    }

    // ── Tests ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateDigitalIdentityAndApplicationUser()
    {
        // Arrange
        SetupPartyRole();
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        var command = BuildCommand();

        // Act
        var result = await _handler.Handle(command, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Nickname.Should().Be(command.Nickname);
        result.PartyRoleId.Should().Be(command.PartyRoleId);
        result.Status.Should().Be(GeneralStatus.Active);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallDigitalIdentityRepositoryOnce()
    {
        // Arrange
        SetupPartyRole();
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        // Act
        await _handler.Handle(BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        _digitalIdentityRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<DigitalIdentity>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallApplicationUserRepositoryOnce()
    {
        // Arrange
        SetupPartyRole();
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        // Act
        await _handler.Handle(BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldHashPassword()
    {
        // Arrange
        SetupPartyRole();
        DigitalIdentity? capturedDigitalIdentity = null;

        _digitalIdentityRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DigitalIdentity>(), It.IsAny<CancellationToken>()))
            .Callback<DigitalIdentity, CancellationToken>((di, _) => capturedDigitalIdentity = di);

        _passwordHasherMock
            .Setup(x => x.Hash("Test1234!"))
            .Returns("hashed_password");

        // Act
        await _handler.Handle(BuildCommand(password: "Test1234!"), TestContext.Current.CancellationToken);

        // Assert
        capturedDigitalIdentity.Should().NotBeNull();

        var passwordHash = capturedDigitalIdentity!.Credentials
            .First()
            .Characteristics
            .First(ch => ch.Name == "passwordHash")
            .Value;

        passwordHash.Should().Be("hashed_password");
        _passwordHasherMock.Verify(x => x.Hash("Test1234!"), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldMapEmailContactMedium()
    {
        // Arrange
        SetupPartyRole(partyId: 10);
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        DigitalIdentity? capturedDigitalIdentity = null;
        _digitalIdentityRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DigitalIdentity>(), It.IsAny<CancellationToken>()))
            .Callback<DigitalIdentity, CancellationToken>((di, _) => capturedDigitalIdentity = di);

        // Act
        await _handler.Handle(BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        var contactMedium = capturedDigitalIdentity!.Credentials
            .First()
            .ContactMedia
            .First();

        contactMedium.Email.Should().Be("john@example.com");
        contactMedium.MediumType.Should().Be(ContactMediumType.EmailAddress);
        contactMedium.IsPreferred.Should().BeTrue();
        contactMedium.PartyId.Should().Be(10);
    }

    [Fact]
    public async Task Handle_PartyRoleNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _partyRoleRepositoryMock
            .Setup(x => x.FindOneAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<PartyRole, bool>>>(),
                It.IsAny<Func<IQueryable<PartyRole>, IQueryable<PartyRole>>?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((PartyRole?)null);

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(partyRoleId: 999),
            TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}