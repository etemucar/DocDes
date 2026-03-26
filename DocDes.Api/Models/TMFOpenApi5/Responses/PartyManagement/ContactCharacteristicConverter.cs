using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DocDes.Api.Models.TMFOpenApi5;

public class ContactCharacteristicConverter : JsonConverter<ContactCharacteristicBase>
{
    public override ContactCharacteristicBase? ReadJson(
        JsonReader reader,
        Type objectType,
        ContactCharacteristicBase? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var type    = jObject["@type"]?.Value<string>();

        ContactCharacteristicBase characteristic = type switch
        {
            "EmailAddress"    => new EmailCharacteristic(),
            "TelephoneNumber" => new TelephoneCharacteristic(),
            "PostalAddress"   => new PostalAddressCharacteristic(),
            _ => throw new JsonSerializationException(
                $"Bilinmeyen ContactCharacteristic tipi: '{type}'. " +
                $"Geçerli tipler: EmailAddress, TelephoneNumber, PostalAddress")
        };

        serializer.Populate(jObject.CreateReader(), characteristic);
        return characteristic;
    }

    public override void WriteJson(
        JsonWriter writer,
        ContactCharacteristicBase? value,
        JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}