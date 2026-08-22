using System.Net.Http.Json;
using System.Text.Json;
using Portic.Sdk.Contracts;

namespace Portic.Client;

/// <summary>
/// Thin transport client for a running Portic gateway. This package owns HTTP ergonomics only; the
/// normalized request/response contracts remain in <c>Portic.Sdk</c>.
/// </summary>
public sealed class PorticClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly Uri MessagesUri = new("/v1/messages", UriKind.Relative);
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates a client over a pre-configured <see cref="HttpClient"/> whose
    /// <see cref="HttpClient.BaseAddress"/> points at a Portic gateway.
    /// </summary>
    public PorticClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    /// <summary>
    /// Sends a normalized chat request to <c>POST /v1/messages</c> and returns the normalized
    /// completion shape from the gateway.
    /// </summary>
    public async Task<ChatCompletion> SendAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_httpClient.BaseAddress is null)
        {
            throw new InvalidOperationException("PorticClient requires HttpClient.BaseAddress to be set.");
        }

        using var response = await _httpClient.PostAsJsonAsync(
            MessagesUri,
            request,
            JsonOptions,
            cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await PorticClientException.CreateAsync(response, cancellationToken).ConfigureAwait(false);
        }

        var completion = await response.Content.ReadFromJsonAsync<ChatCompletion>(JsonOptions, cancellationToken)
            .ConfigureAwait(false);

        if (completion is null)
        {
            throw new InvalidOperationException("Portic returned an empty response body.");
        }

        return completion;
    }
}
