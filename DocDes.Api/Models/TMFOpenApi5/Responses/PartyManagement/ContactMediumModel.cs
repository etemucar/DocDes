using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

/// <summary>
/// TM Forum TMF632 Party Management API - ContactMedium (array elemanı)
/// </summary>
public class ContactMediumModel
{
    [JsonProperty("preferred")]
    public bool Preferred { get; set; } = false;

    [Required]
    [JsonProperty("mediumType")]
    public string MediumType { get; set; } = null!; // EmailAddress, TelephoneNumber, PostalAddress

    // Polymorphic deserialize için @type zorunlu
    [JsonProperty("@type")]
    public string Type { get; set; } = "ContactMedium";

    [JsonProperty("validFor")]
    public TimePeriodModel? ValidFor { get; set; }    

    // Characteristic farklı tiplere göre değişir → polymorphic
    [JsonProperty("characteristic")]
    [JsonConverter(typeof(ContactCharacteristicConverter))]
    public ContactCharacteristicBase? Characteristic { get; set; }
}

// --------------------- BASE ---------------------
public abstract class ContactCharacteristicBase
{
    [JsonProperty("@type")]
    public abstract string Type { get; }
}

// --------------------- EMAIL ---------------------
public class EmailCharacteristic : ContactCharacteristicBase
{
    public override string Type => "EmailAddress";

    [JsonProperty("emailAddress")]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
}

// --------------------- TELEPHONE ---------------------
public class TelephoneCharacteristic : ContactCharacteristicBase
{
    public override string Type => "TelephoneNumber";

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonProperty("countryCode")]
    public string? CountryCode { get; set; }

    [JsonProperty("areaCode")]
    public string? AreaCode { get; set; }

    [JsonProperty("localNumber")]
    public string? LocalNumber { get; set; }
}

// --------------------- POSTAL ADDRESS ---------------------
public class PostalAddressCharacteristic : ContactCharacteristicBase
{
    public override string Type => "PostalAddress";

    // TM Forum zorunlu/önerilen – frontend bunları doldurur (gösterim için)
    [JsonProperty("street1")]
    public string? Street1 { get; set; }           // "Örnek Sokak No:5 D:3"

    [JsonProperty("street2")]
    public string? Street2 { get; set; }           // "Yavuztürk Mah."

    [JsonProperty("city")]
    public string City { get; set; } = "İstanbul";

    [JsonProperty("stateOrProvince")]
    public string? StateOrProvince { get; set; }   // "Marmara" veya boş

    [JsonProperty("postcode")]
    public string? Postcode { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; } = "Türkiye";

    // ---------- TÜRKİYE'YE ÖZEL – JsonIgnore ile TM Forum JSON’unda ÇIKMAZ ----------
    [JsonIgnore] public int CityId { get; set; } = 34;         // İstanbul
    [JsonIgnore] public int DistrictId { get; set; }           // Üsküdar ID
    [JsonIgnore] public int? NeighborhoodId { get; set; }      // Yavuztürk Mah. ID
    [JsonIgnore] public int? ExistingAddressId { get; set; }   // var olan adresi seçtiyse
}