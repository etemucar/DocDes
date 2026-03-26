using DocDes.Core.Model;

namespace DocDes.Service.Helpers;

public static class AuthHelper
{
    public static (string userName, string userIdentifier) ResolveUserInfo(DigitalIdentity digitalIdentity)
    {
        var individual = digitalIdentity.PartyRole?.Party?.Individual;

        var userName = individual != null
            ? $"{individual.GivenName} {individual.FamilyName}".Trim()
            : digitalIdentity.Nickname ?? string.Empty;

        var contactMedium = digitalIdentity.Credentials
            .SelectMany(c => c.ContactMedia)
            .FirstOrDefault();

        var userIdentifier = contactMedium?.Email
            ?? contactMedium?.PhoneNumber
            ?? string.Empty;

        return (userName, userIdentifier);
    }
}