using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.Security;
using DocDes.Core.Exceptions;
using DocDes.Service.Features.Commands;
using DocDes.Service.Features.Handlers;

namespace DocDes.Service.Tests.Features.Handlers;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IRepository<Party, int>>         _partyRepositoryMock;
    private readonly Mock<IRepository<ApplicationUser, int>> _userRepositoryMock;
    private readonly Mock<IRepository<RefreshToken, int>>  _refreshTokenRepositoryMock;
    private readonly Mock<ITokenService>                   _tokenServiceMock;
    private readonly Mock<IPasswordHasher>                 _passwordHasherMock;
    private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
    private readonly RegisterCommandHandler                _handler;

    public RegisterCommandHandlerTests()
    {
        _partyRepositoryMock        = new Mock<IRepository<Party, int>>();
        _userRepositoryMock         = new Mock<IRepository<ApplicationUser, int>>();
        _refreshTokenRepositoryMock = new Mock<IRepository<RefreshToken, int>>();
        _tokenServiceMock           = new Mock<ITokenService>();
        _passwordHasherMock         = new Mock<IPasswordHasher>();
        _loggerMock                 = new Mock<ILogger<RegisterCommandHandler>>();

        _handler = new RegisterCommandHandler(
            _partyRepositoryMock.Object,
            _userRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _tokenServiceMock.Object,
            _passwordHasherMock.Object,
            _loggerMock.Object);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static RegisterCommand BuildCommand(
        string givenName  = "Jane",
        string familyName = "Doe",
        string identifier = "jane@example.com",
        string password   = "Test1234!",
        int    languageId = 1) => new()
    {
        GivenName  = givenName,
        FamilyName = familyName,
        Identifier = identifier,
        Password   = password,
        LanguageId = languageId
    };

    // AddAsync sonrası EF navigation'larının dolu gelmesini simüle eder
    private void SetupPartyRepository(
        string email        = "jane@example.com",
        string passwordHash = "hashed_password")
    {
        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Callback<Party, CancellationToken>((party, _) =>
            {
                // EF'nin cascade insert sonrası dolduracağı ID'leri simüle et
                var partyRole       = party.PartyRoles!.First();
                var digitalIdentity = partyRole.DigitalIdentity!;
                var appUser         = digitalIdentity.ApplicationUser!;

                partyRole.Id       = 1;
                digitalIdentity.Id = Guid.NewGuid();
                appUser.Id         = 99;

                // Individual dolu olduğu için ResolveUserInfo çalışabilir
                party.Individual = new Individual
                {
                    GivenName  = "Jane",
                    FamilyName = "Doe"
                };
            })
            .Returns(Task.CompletedTask);
    }

    private void SetupUserNotExists()
    {
        _userRepositoryMock
            .Setup(x => x.AnyAsync(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }

    private void SetupUserExists()
    {
        _userRepositoryMock
            .Setup(x => x.AnyAsync(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    private void SetupTokenService(
        string accessToken  = "access_token",
        string refreshToken = "refresh_token")
    {
        _tokenServiceMock
            .Setup(x => x.CreateAccessToken(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
            .Returns(new AccessToken
            {
                Token      = accessToken,
                Expiration = DateTime.UtcNow.AddDays(1)
            });

        _tokenServiceMock
            .Setup(x => x.CreateRefreshToken(It.IsAny<int>()))
            .Returns(new RefreshToken
            {
                Token     = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            });
    }

    private void SetupPasswordHasher(string hash = "hashed_password")
    {
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns(hash);
    }

    // ── Tests ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnAuthResult()
    {
        // Arrange
        SetupUserNotExists();
        SetupPartyRepository();
        SetupTokenService();
        SetupPasswordHasher();

        // Act
        var result = await _handler.Handle(
            BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");
        result.AccessTokenExpiration.Should().BeAfter(DateTime.UtcNow);
        result.RefreshTokenExpiration.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldSaveParty()
    {
        // Arrange
        SetupUserNotExists();
        SetupPartyRepository();
        SetupTokenService();
        SetupPasswordHasher();

        // Act
        await _handler.Handle(BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        _partyRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldSaveRefreshToken()
    {
        // Arrange
        SetupUserNotExists();
        SetupPartyRepository();
        SetupTokenService();
        SetupPasswordHasher();

        // Act
        await _handler.Handle(BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        _refreshTokenRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldHashPassword()
    {
        // Arrange
        SetupUserNotExists();
        SetupPartyRepository();
        SetupTokenService();
        SetupPasswordHasher();

        // Act
        await _handler.Handle(BuildCommand(password: "Test1234!"), TestContext.Current.CancellationToken);

        // Assert
        _passwordHasherMock.Verify(
            x => x.Hash("Test1234!"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldBuildCorrectIndividual()
    {
        // Arrange
        SetupUserNotExists();
        SetupTokenService();
        SetupPasswordHasher();

        Party? capturedParty = null;
        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Callback<Party, CancellationToken>((party, _) =>
            {
                capturedParty = party;
                // navigation simülasyonu
                var partyRole       = party.PartyRoles!.First();
                partyRole.DigitalIdentity!.ApplicationUser!.Id = 1;
                party.Individual = new Individual
                {
                    GivenName  = party.Individual!.GivenName,
                    FamilyName = party.Individual!.FamilyName
                };
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(
            BuildCommand(givenName: "Jane", familyName: "Doe"),
            TestContext.Current.CancellationToken);

        // Assert
        capturedParty.Should().NotBeNull();
        capturedParty!.Individual!.GivenName.Should().Be("Jane");
        capturedParty!.Individual!.FamilyName.Should().Be("Doe");
    }

    [Fact]
    public async Task Handle_EmailIdentifier_ShouldSetEmailOnContactMedium()
    {
        // Arrange
        SetupUserNotExists();
        SetupTokenService();
        SetupPasswordHasher();

        Party? capturedParty = null;
        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Callback<Party, CancellationToken>((party, _) =>
            {
                capturedParty = party;
                var partyRole = party.PartyRoles!.First();
                partyRole.DigitalIdentity!.ApplicationUser!.Id = 1;
                party.Individual = new Individual { GivenName = "Jane", FamilyName = "Doe" };
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(
            BuildCommand(identifier: "jane@example.com"),
            TestContext.Current.CancellationToken);

        // Assert
        var contactMedium = capturedParty!.PartyRoles!.First()
            .DigitalIdentity!.Credentials.First()
            .ContactMedia.First();

        contactMedium.Email.Should().Be("jane@example.com");
        contactMedium.PhoneNumber.Should().BeNull();
    }

    [Fact]
    public async Task Handle_PhoneIdentifier_ShouldSetPhoneOnContactMedium()
    {
        // Arrange
        SetupUserNotExists();
        SetupTokenService();
        SetupPasswordHasher();

        Party? capturedParty = null;
        _partyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Party>(), It.IsAny<CancellationToken>()))
            .Callback<Party, CancellationToken>((party, _) =>
            {
                capturedParty = party;
                var partyRole = party.PartyRoles!.First();
                partyRole.DigitalIdentity!.ApplicationUser!.Id = 1;
                party.Individual = new Individual { GivenName = "Jane", FamilyName = "Doe" };
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(
            BuildCommand(identifier: "05551234567"),
            TestContext.Current.CancellationToken);

        // Assert
        var contactMedium = capturedParty!.PartyRoles!.First()
            .DigitalIdentity!.Credentials.First()
            .ContactMedia.First();

        contactMedium.PhoneNumber.Should().Be("05551234567");
        contactMedium.Email.Should().BeNull();
    }

    [Fact]
    public async Task Handle_DuplicateIdentifier_ShouldThrowConflictException()
    {
        // Arrange
        SetupUserExists();

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(), TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*kayıtlı*");
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldResolveUserNameFromIndividual()
    {
        // Arrange
        SetupUserNotExists();
        SetupPartyRepository();
        SetupTokenService();
        SetupPasswordHasher();

        // Act
        await _handler.Handle(
            BuildCommand(givenName: "Jane", familyName: "Doe"),
            TestContext.Current.CancellationToken);

        // Assert — CreateAccessToken'a "Jane Doe" geçilmeli
        _tokenServiceMock.Verify(
            x => x.CreateAccessToken("Jane Doe", It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldNotSaveRefreshTokenOnConflict()
    {
        // Arrange
        SetupUserExists();

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<ConflictException>();

        // Assert — çakışma durumunda token kaydedilmemeli
        _refreshTokenRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}