using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Portic.Client;

/// <summary>
/// Represents a non-success HTTP response from the Portic gateway.
/// </summary>
public sealed class PorticClientException : Exception
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private PorticClientException(
        HttpStatusCode statusCode,
        string message,
        string? reasonCode,
        string? detail,
        string? responseBody)
        : base(message)
    {
        StatusCode = statusCode;
        ReasonCode = reasonCode;
        Detail = detail;
        ResponseBody = responseBody;
    }

    /// <summary>The HTTP status code returned by the gateway.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>Problem-details title when the gateway returned a JSON error payload.</summary>
    public string? ReasonCode { get; }

    /// <summary>Problem-details detail text when the gateway returned a JSON error payload.</summary>
    public string? Detail { get; }

    /// <summary>The raw response body, preserved for diagnostics.</summary>
    public string? ResponseBody { get; }

    internal static async Task<PorticClientException> CreateAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        var body = response.Content is null
            ? null
            : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        var problem = TryParseProblem(response.Content?.Headers.ContentType, body);
        var message = problem is null
            ? $"Portic request failed with status code {(int)response.StatusCode} ({response.StatusCode})."
            : $"Portic request failed with status code {(int)response.StatusCode} ({response.StatusCode}): {problem.Title}.";

        return new PorticClientException(
            response.StatusCode,
            message,
            problem?.Title,
            problem?.Detail,
            body);
    }

    private static ProblemPayload? TryParseProblem(MediaTypeHeaderValue? contentType, string? body)
    {
        if (string.IsNullOrWhiteSpace(body) || contentType?.MediaType is null)
        {
            return null;
        }

        if (!contentType.MediaType.Contains("json", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ProblemPayload>(body, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed record ProblemPayload(string? Title, string? Detail);
}
