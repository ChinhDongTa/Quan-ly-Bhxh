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
        //var accessToken = LocalStorage.GetItem<string>("accessToken");
        var accessToken = LocalStorage.GetItem<string>("accessToken");
        if (!string.IsNullOrEmpty(accessToken))
        {
            clientBase.SetAuthorizationHeader(accessToken);
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var infoDto = GetInfoDtoFromLocalStorage();
        if (infoDto != null)
        {
            var user = CreateClaimsPrincipal(infoDto);
            return new AuthenticationState(user);
        }
        else
        {
            try
            {
                var result = await clientBase.GetAsync<ResultDto<InfoDto>>(AccountApiRoute.Info);
                if (result != null && result.Dto != null)
                {
                    SetInfoDto(result.Dto);
                    var user = CreateClaimsPrincipal(result.Dto);
                    return new AuthenticationState(user);
                }
                else
                {
                    ClearToken();
                    Console.WriteLine($"Error fetching user info");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user info: {ex.Message}");
                ClearToken();
            }
        }

        // Return an unauthenticated state if no valid user information is found
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<Result<bool>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var response = await clientBase.PostAsync<LoginDto, LoginReponse>("login", loginDto);
            if (response != null)
            {
                SetAuthenticationTokens(response);
                return new Result<bool>(true, InfoMessage.ActionSuccess(CRUD.Read, ConstName.Login));
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

        return new Result<bool>(false, InfoMessage.ActionFailed(CRUD.Read, ConstName.Login));
    }

    public async Task<Result<bool>> RefreshTokenAsync()
    {
        var refreshToken = LocalStorage.GetItem<string>("refreshToken");
        if (string.IsNullOrEmpty(refreshToken))
        {
            return new Result<bool>(false, "Error: Token expired. Please log in again.");
        }

        var response = await clientBase.PostAsync<RefreshTokenRequest, LoginReponse>("refresh", new RefreshTokenRequest(refreshToken));
        if (response != null)
        {
            SetAuthenticationTokens(response);
            return new Result<bool>(true, "Success: Refresh Token");
        }
        ClearToken();
        return new Result<bool>(false, "Error: Unable to refresh token. Please log in again.");
    }

    /// <summary>
    /// todo : signout
    /// </summary>
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
                return new Result<bool>(false, InfoMessage.ActionFailed(CRUD.Create, ConstName.Registration));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
        }
        return new Result<bool>(false, InfoMessage.ActionFailed(CRUD.Create, ConstName.Registration));
    }

    private void SetAuthenticationTokens(LoginReponse response)
    {
        LocalStorage.SetItem("accessToken", response.AccessToken);
        LocalStorage.SetItem("refreshToken", response.RefreshToken);
        LocalStorage.SetItem("expiresIn", response.ExpiresIn.ToString());
        clientBase.SetAuthorizationHeader(response.AccessToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private void SetInfoDto(InfoDto infoDto)
    {
        LocalStorage.SetItem("Id", infoDto.Id);
        LocalStorage.SetItem("Email", infoDto.Email);
        LocalStorage.SetItem("Username", infoDto.Username);
        LocalStorage.SetItem("RoleNames", infoDto.RoleNames);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(InfoDto infoDto)
    {
        var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, infoDto.Id),
                    new(ClaimTypes.Name, infoDto.Username),
                    new(ClaimTypes.Email, infoDto.Email)
                };

        if (infoDto.RoleNames != null)
        {
            claims.AddRange(infoDto.RoleNames.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var identity = new ClaimsIdentity(claims, "Token");
        return new ClaimsPrincipal(identity);
    }

    private InfoDto? GetInfoDtoFromLocalStorage()
    {
        var id = LocalStorage.GetItem<string>("Id");
        var email = LocalStorage.GetItem<string>("Email");
        var username = LocalStorage.GetItem<string>("Username");
        var roleNames = LocalStorage.GetItem<IEnumerable<string>>("RoleNames");
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            return null;
        }
        // Kiểm tra định dạng email
        if (!email.Contains('@'))
        {
            return null;
        }
        return new InfoDto
        {
            Id = id,
            Email = email,
            Username = username,
            RoleNames = roleNames
        };
    }

    private void ClearToken()
    {
        LocalStorage.RemoveItem("accessToken");
        LocalStorage.RemoveItem("refreshToken");
        LocalStorage.RemoveItem("expiresIn");
        LocalStorage.RemoveItem("Id");
        //LocalStorage.RemoveItem("id");
        //LocalStorage.RemoveItem("infoDtoId");
        //LocalStorage.RemoveItem("jwt_token");
        LocalStorage.RemoveItem("Email");
        LocalStorage.RemoveItem("Username");
        LocalStorage.RemoveItem("RoleNames");
        clientBase.SetAuthorizationHeader();
    }
}

internal record RefreshTokenRequest(string RefreshToken);