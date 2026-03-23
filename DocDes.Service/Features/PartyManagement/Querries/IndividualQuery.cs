using MediatR;
using DocDes.Service.Dtos.Responses;
 
namespace DocDes.Service.Features.PartyManagement.Queries
{
    public class GetIndividualQuery : IRequest<IndividualResponse>
    {
        public Guid Id { get; set; }
    }
 
    public class GetIndividualListQuery : IRequest<IEnumerable<IndividualResponse>>
    {
        public int    Offset { get; set; } = 0;
        public int    Limit  { get; set; } = 20;
        public string Fields { get; set; } = null!;
    }
}
 