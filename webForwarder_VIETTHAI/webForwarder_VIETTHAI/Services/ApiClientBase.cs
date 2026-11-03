using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using webForwarder_VIETTHAI.Models.Api;

namespace webForwarder_VIETTHAI.Services;

/// <summary>Gọi Web API theo envelope { status, data (chuỗi JSON), errorMsg }.</summary>
public abstract class ApiClientBase(HttpClient http)
{
    protected static readonly JsonSerializerOptions ReadJson = new(JsonSerializerDefaults.Web)
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,   
    };

    protected static readonly JsonSerializerOptions WriteJson = new() { PropertyNamingPolicy = null };

    protected HttpClient Http { get; } = http;

    /// <summary>GET path?a=b... rồi giải mã "data" thành T.</summary>
    protected async Task<T?> GetAsync<T>(string pathWithQuery, CancellationToken ct)
    {
        using var response = await Http.GetAsync(pathWithQuery, ct);
        return await ReadEnvelopeAsync<T>(response, ct);
    }

    /// <summary>POST body JSON rồi giải mã "data" thành T.</summary>
    protected async Task<T?> PostAsync<T>(string path, object body, CancellationToken ct)
    {
        using var response = await Http.PostAsJsonAsync(path, body, WriteJson, ct);
        return await ReadEnvelopeAsync<T>(response, ct);
    }

    /// <summary>POST body form-urlencoded (key=value&amp;...) rồi giải mã "data" thành T.</summary>
    protected async Task<T?> PostFormAsync<T>(string path, IEnumerable<KeyValuePair<string, string>> form, CancellationToken ct)
    {
        using var content = new FormUrlEncodedContent(form);
        using var response = await Http.PostAsync(path, content, ct);
        return await ReadEnvelopeAsync<T>(response, ct);
    }

    /// <summary>PUT body JSON rồi giải mã "data" thành T.</summary>
    protected async Task<T?> PutAsync<T>(string path, object body, CancellationToken ct)
    {
        using var response = await Http.PutAsJsonAsync(path, body, WriteJson, ct);
        return await ReadEnvelopeAsync<T>(response, ct);
    }

    /// <summary>DELETE path?a=b... rồi giải mã "data" thành T.</summary>
    protected async Task<T?> DeleteRequestAsync<T>(string pathWithQuery, CancellationToken ct)
    {
        using var response = await Http.DeleteAsync(pathWithQuery, ct);
        return await ReadEnvelopeAsync<T>(response, ct);
    }

    private static async Task<T?> ReadEnvelopeAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        response.EnsureSuccessStatusCode();

        var envelope = await response.Content.ReadFromJsonAsync<ApiEnvelope>(ReadJson, ct)
            ?? throw new ApiException("API không trả dữ liệu.");

        if (envelope.Status != 0)
         {
            throw new ApiException(string.IsNullOrWhiteSpace(envelope.ErrorMsg) ? "API báo lỗi." : envelope.ErrorMsg);
         }

        if (string.IsNullOrWhiteSpace(envelope.Data))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(envelope.Data, ReadJson);
    }

    /// <summary>"data" có thể là một object hoặc mảng 1 phần tử – lấy phần tử đầu.</summary>
    protected static T? FirstOrSelf<T>(string? data) where T : class
    {
        if (string.IsNullOrWhiteSpace(data)) return null;
        var trimmed = data.TrimStart();
        return trimmed.StartsWith('[')
            ? JsonSerializer.Deserialize<List<T>>(trimmed, ReadJson)?.FirstOrDefault()
            : JsonSerializer.Deserialize<T>(trimmed, ReadJson);
    }
}