﻿using Blazored.LocalStorage;
using DataTranfer.Dtos;
using DefaultValue;
using DefaultValue.ApiRoute;

using DongTa.ResponseMessage;
using DongTa.ResponseResult;
using HttpClientBase;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BhxhWasm.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider {
    public readonly IHttpClientBase client;
    public readonly ISyncLocalStorageService LocalStorage;

    public CustomAuthStateProvider(IHttpClientBase Http, ISyncLocalStorageService LocalStorage)
    {
        this.client = Http;
        this.LocalStorage = LocalStorage;
        var accessToken = LocalStorage.GetItem<string>("accessToken");
        if (accessToken != null)
        {
            client.SetAuthorizationHeaderAsync(accessToken);
            //new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        try
        {
            var result = await client.GetAsync<ResultDto<InfoDto>>(AccountApiRoute.Info);
            if (result != null && result.Dto != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, result.Dto.Username),
                    new(ClaimTypes.Email, result.Dto.Email),
                    new(ClaimTypes.Role, result.Dto.RoleName)
                };
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
            var response = await client.PostAsync<LoginDto, LoginReponse>("login", loginDto);
            if (response != null)
            {
                LocalStorage.SetItem("accessToken", response.AccessToken);
                LocalStorage.SetItem("refreshToken", response.RefreshToken);
                LocalStorage.SetItem("expiresIn", response.ExpiresIn.ToString());
                // Store token
                //message = "Login successful!";
                //Navigation.NavigateTo("/"); // Redirect to home
                client.SetAuthorizationHeaderAsync(response.AccessToken);

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return new Result<bool>(true, InfoMessage.ActionSuccess(ConstName.Login));
            }
            else
            {
                return new Result<bool>(false, InfoMessage.EmailOrPasswordInvalid)
               ;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
        }

        return new Result<bool>(false, InfoMessage.ActionFailed(ConstName.Login));
    }

    public void Logout()
    {
        LocalStorage.RemoveItem("accessToken");
        LocalStorage.RemoveItem("refreshToken");
        LocalStorage.RemoveItem("expiresIn");
        client.SetAuthorizationHeaderAsync("");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<Result<bool>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var responseMessage = await client.PostAsync<RegisterDto, ResultDto<bool>>(AccountApiRoute.Register, registerDto);
            if (responseMessage != null)
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
}