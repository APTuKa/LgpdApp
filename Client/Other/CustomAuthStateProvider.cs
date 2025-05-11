using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");

        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var claims = ParseClaimsFromJwt(token);
                identity = new ClaimsIdentity(claims, "jwt");
            }
            catch
            {
                // Некорректный токен — считаем пользователя анонимным
            }
        }

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(ClaimsPrincipal user)
    {
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = new List<Claim>();

        if (keyValuePairs.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var role))
        {
            if (role is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                foreach (var r in element.EnumerateArray())
                {
                    claims.Add(new Claim(ClaimTypes.Role, r.GetString()));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
        }

        foreach (var pair in keyValuePairs)
        {
            if (pair.Key == "role") continue; // уже обработано выше
            claims.Add(new Claim(pair.Key, pair.Value.ToString()));
        }

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}