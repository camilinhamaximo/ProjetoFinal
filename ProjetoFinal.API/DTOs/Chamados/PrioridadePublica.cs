using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetoFinal.API.DTOs.Chamados;

[JsonConverter(typeof(PrioridadePublicaJsonConverter))]
public enum PrioridadePublica
{
    Baixa,
    Media,
    Alta
}

public sealed class PrioridadePublicaJsonConverter : JsonConverter<PrioridadePublica>
{
    public override PrioridadePublica Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("A prioridade deve ser um texto válido.");
        }

        return reader.GetString() switch
        {
            "Baixa" => PrioridadePublica.Baixa,
            "Média" => PrioridadePublica.Media,
            "Alta" => PrioridadePublica.Alta,
            _ => throw new JsonException("A prioridade informada é inválida.")
        };
    }

    public override void Write(Utf8JsonWriter writer, PrioridadePublica value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            PrioridadePublica.Baixa => "Baixa",
            PrioridadePublica.Media => "Média",
            PrioridadePublica.Alta => "Alta",
            _ => throw new JsonException("A prioridade informada é inválida.")
        });
}
