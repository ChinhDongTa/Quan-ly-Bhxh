using Blazored.LocalStorage;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.ResponseMessage;
using DongTa.ResponseResult;
using Dtos.Human;
using HttpClientBase;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BhxhWasm.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider {
    private readonly IHttpClientBase clientBase;
    private readonly ISyncLocalStorageService LocalStorage;

    public CustomAuthStateProvider(IHttpClientBase httpClient, ISyncLocalStorageService LocalStorage)
    {
        clientBase = httpClient;
        this.LocalStorage = LocalStorage;
        var accessToken = LocalStorage.GetItem<string>("accessToken");
        if (accessToken != null)
        {
            clientBase.SetAuthorizationHeader(accessToken);
            //new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        try
        {
            var result = await clientBase.GetAsync<ResultDto<InfoDto>>(AccountApiRoute.Info);
            if (result != null && result.Dto != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, result.Dto.Id),
                    new(ClaimTypes.Name, result.Dto.Username),
                    new(ClaimTypes.Email, result.Dto.Email)
                };

                // Fix for CS1503: Convert IEnumerable<string> RoleNames to individual claims
                if (result.Dto.RoleNames != null)
                {
                    claims.AddRange(result.Dto.RoleNames.Select(role => new Claim(ClaimTypes.Role, role)));
                }

                var identity = new ClaimsIdentity(claims, "Token");
                user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            else
            {
                Console.WriteLine($"Error fetching user info");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user info: {ex.Message}");
        }

        return new AuthenticationState(user);
    }

    public async Task<Result<bool>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var response = await clientBase.PostAsync<LoginDto, LoginReponse>("login", loginDto);
            if (response != null)
            {
                SetAuthenticationTokens(response);
                return new Result<bool>(true, InfoMessage.ActionSuccess(ConstName.Login));
            }
            else
            {
                return new Result<bool>(false, InfoMessage.EmailOrPasswordInvalid);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
        }

        return new Result<bool>(false, InfoMessage.ActionFailed(ConstName.Login));
    }

    public async Task<Result<bool>> RefreshTokenAsync()
    {
        var refreshToken = LocalStorage.GetItem<string>("refreshToken");
        if (string.IsNullOrEmpty(refreshToken))
        {
            return new Result<bool>(false, "Token đã hết hạn");
        }

        var response = await clientBase.PostAsync<RefreshTokenRequest, LoginReponse>("refresh", new RefreshTokenRequest(refreshToken));
        if (response != null)
        {
            SetAuthenticationTokens(response);
            return new Result<bool>(true, "Success: Refresh Token");
        }
        ClearToken();
        return new Result<bool>(false, "Không thể lấy lại token");
    }

    public void Logout()
    {
        ClearToken();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<Result<bool>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var responseMessage = await clientBase.PostAsync<RegisterDto, ResultDto<bool>>(AccountApiRoute.Register, registerDto);
            if (responseMessage != null && responseMessage.IsSuccess)
            {
                return await LoginAsync(new LoginDto
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password
                });
            }
            else
            {
                return new Result<bool>(false, InfoMessage.ActionFailed(ConstName.Registration));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
        }
        return new Result<bool>(false, InfoMessage.ActionFailed(ConstName.Registration));
    }

    private void SetAuthenticationTokens(LoginReponse response)
    {
        LocalStorage.SetItem("accessToken", response.AccessToken);
        LocalStorage.SetItem("refreshToken", response.RefreshToken);
        LocalStorage.SetItem("expiresIn", response.ExpiresIn.ToString());
        clientBase.SetAuthorizationHeader(response.AccessToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private void ClearToken()
    {
        LocalStorage.RemoveItem("accessToken");
        LocalStorage.RemoveItem("refreshToken");
        LocalStorage.RemoveItem("expiresIn");
        clientBase.SetAuthorizationHeader();
    }
}

internal record RefreshTokenRequest(string RefreshToken);