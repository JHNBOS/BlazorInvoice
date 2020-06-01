using Blazored.LocalStorage;

using BlazorInvoice.Data.Services;
using BlazorInvoice.Data.Services.Interfaces;
using BlazorInvoice.Infrastructure.Entities;

using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorInvoice.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
        public ILocalStorageService _localStorageService { get; }
        public IUserService _userService { get; set; }

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, UserService userService)
        {
            _localStorageService = localStorageService;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");

            ClaimsIdentity identity;

            if (accessToken != null && accessToken != string.Empty)
            {
                var user = await _userService.GetUserByToken(accessToken);
                identity = GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(User user)
        {
            await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
            await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync("refreshToken");
            await _localStorageService.RemoveItemAsync("accessToken");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Email != null)
            {
                claimsIdentity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, user.Email),
                                    new Claim(ClaimTypes.Role, user.Role.Name),
                                }, "apiauth_type");
            }

            return claimsIdentity;
        }
    }
}
