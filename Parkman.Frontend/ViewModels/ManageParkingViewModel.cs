using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Parkman.Shared.Dtos;
using Parkman.Shared.Enums;

namespace Parkman.Frontend.ViewModels;

public class ManageParkingViewModel : ViewModelBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    private CreateParkingLotRequest _lot = new();
    public CreateParkingLotRequest Lot
    {
        get => _lot;
        set => SetProperty(ref _lot, value);
    }

    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set => SetProperty(ref _isSubmitting, value);
    }

    private bool _success;
    public bool Success
    {
        get => _success;
        set => SetProperty(ref _success, value);
    }

    public ManageParkingViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
    }

    public void AddSpot()
    {
        Lot.Spots.Add(new CreateParkingSpotRequest());
    }

    public async Task CreateLot()
    {
        IsSubmitting = true;
        Success = false;
        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/admin/parking/lots", Lot);
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
            IsSubmitting = false;
            return;
        }

        if (response.IsSuccessStatusCode)
        {
            Success = true;
            Lot = new CreateParkingLotRequest();
        }
        IsSubmitting = false;
    }
}
