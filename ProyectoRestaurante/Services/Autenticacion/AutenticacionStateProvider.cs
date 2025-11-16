using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class AutenticacionStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());

    public Task Login(ClaimsPrincipal principal)
    {
        _usuarioActual = principal;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        return Task.CompletedTask;
    }

    public Task Logout()
    {
        _usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_usuarioActual))
        );
        return Task.CompletedTask;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var usuario = _usuarioActual ?? new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(usuario));
    }
}