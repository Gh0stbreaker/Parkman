using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkman.Frontend.Services;
using Parkman.Shared.Enums;
using Parkman.Shared.Models;

namespace Parkman.Frontend.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    public RegisterWithVehicleRequest UserModel { get; private set; } = new();
    public RegisterCompanyRequest CompanyModel { get; private set; } = new();

    public EditContext UserEditContext { get; private set; }
    public ValidationMessageStore UserMessageStore { get; private set; }
    public EditContext CompanyEditContext { get; private set; }
    public ValidationMessageStore CompanyMessageStore { get; private set; }

    private bool _registerAsCompany;
    public bool RegisterAsCompany
    {
        get => _registerAsCompany;
        set => SetProperty(ref _registerAsCompany, value);
    }

    private bool _registerCompanyVehicle;
    public bool RegisterCompanyVehicle
    {
        get => _registerCompanyVehicle;
        set => SetProperty(ref _registerCompanyVehicle, value);
    }

    private bool _showPassword;
    public bool ShowPassword
    {
        get => _showPassword;
        set => SetProperty(ref _showPassword, value);
    }

    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set => SetProperty(ref _isSubmitting, value);
    }

    private string? _successMessage;
    public string? SuccessMessage
    {
        get => _successMessage;
        set => SetProperty(ref _successMessage, value);
    }

    private bool _showSuccessModal;
    public bool ShowSuccessModal
    {
        get => _showSuccessModal;
        set => SetProperty(ref _showSuccessModal, value);
    }

    public readonly VehicleBrand[] VehicleBrands = Enum.GetValues<VehicleBrand>();
    public readonly VehicleType[] VehicleTypes = Enum.GetValues<VehicleType>();
    public readonly VehiclePropulsionType[] VehiclePropulsionTypes = Enum.GetValues<VehiclePropulsionType>();

    public RegisterViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
        UserEditContext = new EditContext(UserModel);
        UserMessageStore = new ValidationMessageStore(UserEditContext);
        CompanyEditContext = new EditContext(CompanyModel);
        CompanyMessageStore = new ValidationMessageStore(CompanyEditContext);
    }

    public async Task RegisterUserAsync()
    {
        IsSubmitting = true;
        SuccessMessage = null;
        UserMessageStore.Clear();

        if (RegisterCompanyVehicle)
        {
            bool validationFailed = false;
            if (string.IsNullOrWhiteSpace(UserModel.CompanyEmail))
            {
                UserMessageStore.Add(new FieldIdentifier(UserModel, nameof(UserModel.CompanyEmail)), new[] { "Company email is required." });
                validationFailed = true;
            }
            if (string.IsNullOrWhiteSpace(UserModel.PairingPassword))
            {
                UserMessageStore.Add(new FieldIdentifier(UserModel, nameof(UserModel.PairingPassword)), new[] { "Pairing password is required." });
                validationFailed = true;
            }
            if (validationFailed)
            {
                UserEditContext.NotifyValidationStateChanged();
                IsSubmitting = false;
                return;
            }
        }

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/auth/register", UserModel);
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
            IsSubmitting = false;
            return;
        }
        var error = await response.ApplyValidationErrorsAsync(UserEditContext, UserMessageStore);

        if (error is null)
        {
            SuccessMessage = "Registration successful!";
            UserModel = new RegisterWithVehicleRequest();
            UserEditContext = new EditContext(UserModel);
            UserMessageStore = new ValidationMessageStore(UserEditContext);
            RegisterCompanyVehicle = false;
            ShowSuccessModal = true;
        }
        IsSubmitting = false;
    }

    public async Task RegisterCompanyAsync()
    {
        IsSubmitting = true;
        SuccessMessage = null;

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/auth/register/company", CompanyModel);
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error?message=Unable%20to%20reach%20the%20server.");
            IsSubmitting = false;
            return;
        }
        var error = await response.ApplyValidationErrorsAsync(CompanyEditContext, CompanyMessageStore);

        if (error is null)
        {
            SuccessMessage = "Registration successful!";
            CompanyModel = new RegisterCompanyRequest();
            CompanyEditContext = new EditContext(CompanyModel);
            CompanyMessageStore = new ValidationMessageStore(CompanyEditContext);
            ShowSuccessModal = true;
        }
        IsSubmitting = false;
    }
}
