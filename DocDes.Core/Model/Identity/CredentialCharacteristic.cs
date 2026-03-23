using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class CredentialCharacteristic : ModelBase<int>
{
    public string Name { get; set; } = null!;   // "passwordHash", "salt", vb.
    public string Value { get; set; } = null!;   // encrypted at-rest

    public Guid CredentialId { get; set; }
    public Credential Credential { get; set; } = null!;
}