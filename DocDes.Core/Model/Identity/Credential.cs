using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class Credential : ModelBase<Guid>
{
    public CredentialType CredentialType { get; set; }
    public int? TrustLevel { get; set; }
    public Guid DigitalIdentityId { get; set; }

    public DigitalIdentity DigitalIdentity { get; set; } = null!;
    public ICollection<CredentialCharacteristic> Characteristics { get; set; } = new List<CredentialCharacteristic>();
}