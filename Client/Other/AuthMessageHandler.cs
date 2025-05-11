using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, ct);
    }
}