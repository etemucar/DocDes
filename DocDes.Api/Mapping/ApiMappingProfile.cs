using AutoMapper;
using DocDes.Api.Models.TMFOpenApi5;
using DocDes.Service.Features.Commands;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Api.Mapping;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // ── Request mappings (Model → Command) ──────────────────────────

        CreateMap<TimePeriodModel, TimePeriodRequest>();

        CreateMap<ContactMediumModel, ContactMediumRequest>()
            .ForMember(dest => dest.Characteristic, opt => opt.MapFrom(src =>
                src.Characteristic != null
                    ? ObjectToDictionary(src.Characteristic)
                    : new Dictionary<string, object>()));

        CreateMap<RelatedPartyModel, RelatedPartyRequest>()
            .ForMember(dest => dest.PartyOrPartyRole, opt => opt.MapFrom(src => src.PartyOrPartyRole));

        CreateMap<PartyOrPartyRoleModel, PartyOrPartyRoleRequest>();

        CreateMap<IndividualModel, CreateIndividualCommand>()
            .ForMember(dest => dest.ContactMedium, opt => opt.MapFrom(src => src.ContactMedium))
            .ForMember(dest => dest.RelatedParty,  opt => opt.MapFrom(src => src.RelatedParty));

        CreateMap<IndividualModel, PatchIndividualCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Controller'da set edilir

        // ── Response mappings (Response → Model) ────────────────────────

        CreateMap<TimePeriodResponse, TimePeriodModel>();

        CreateMap<IndividualResponse, IndividualModel>()
            .ForMember(dest => dest.Id,       opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Href,     opt => opt.Ignore()) // Controller'da set edilir
            .ForMember(dest => dest.Type,     opt => opt.MapFrom(_ => "Individual"))
            .ForMember(dest => dest.BaseType, opt => opt.MapFrom(_ => "Party"));

        // Request mappings
        CreateMap<CredentialCharacteristicModel, CredentialCharacteristicRequest>();

        CreateMap<CredentialModel, CredentialRequest>()
            .ForMember(dest => dest.ContactMedia, opt => opt.MapFrom(src => src.ContactMedia));

        CreateMap<DigitalIdentityModel, CreateDigitalIdentityCommand>()
            .ForMember(dest => dest.Credentials, opt => opt.MapFrom(src => src.Credentials));

        // Response mappings
        CreateMap<DigitalIdentityResponse, DigitalIdentityModel>()
            .ForMember(dest => dest.Id,   opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Href, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));            
    }

    private static Dictionary<string, object> ObjectToDictionary(object obj)
    {
        var dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        if (obj == null) return dictionary;

        foreach (var prop in obj.GetType().GetProperties())
        {
            var value = prop.GetValue(obj);
            if (value != null)
                dictionary[char.ToLowerInvariant(prop.Name[0]) + prop.Name[1..]] = value;
        }

        return dictionary;
    }
}