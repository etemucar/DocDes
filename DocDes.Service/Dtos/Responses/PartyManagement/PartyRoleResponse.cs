namespace DocDes.Service.Dtos.Responses;

public class PartyRoleResponse
{
    public int    Id              { get; set; }
    public int    PartyId         { get; set; }
    public string PartyRoleTypeCd { get; set; } = null!;
    public TimePeriodResponse ValidFor { get; set; } = new();
}