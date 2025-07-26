using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Parkman.Shared.Dtos;

namespace Parkman.Frontend.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    private UserProfileDto? _profile;
    public UserProfileDto? Profile
    {
        get => _profile;
        private set => SetProperty(ref _profile, value);
    }

    private List<ReservationDto>? _reservations;
    public List<ReservationDto>? Reservations
    {
        get => _reservations;
        private set => SetProperty(ref _reservations, value);
    }

    public DashboardViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
    }

    public async Task InitializeAsync()
    {
        try
        {
            Profile = await _http.GetFromJsonAsync<UserProfileDto>("api/user/profile");
            Reservations = await _http.GetFromJsonAsync<List<ReservationDto>>("api/reservations");
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
        }
    }

    public async Task CreateReservation()
    {
        try
        {
            await _http.PostAsync("api/reservations", null);
            Reservations = await _http.GetFromJsonAsync<List<ReservationDto>>("api/reservations");
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
        }
    }
}
