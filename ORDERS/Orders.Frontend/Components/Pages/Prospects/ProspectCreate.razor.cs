using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using static MudBlazor.Colors;

namespace Orders.Frontend.Components.Pages.Prospects;

public partial class ProspectCreate
{
    private Prospect prospect = new();
    private List<Country>? countries;
    private List<State>? states;
    private List<City>? cities;
    private bool loading;


    private Country selectedCountry = new();
    private State selectedState = new();
    private City selectedCity = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadCountriesAsync();
    }


    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync($"/api/prospects", prospect);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        ReturnAction();
        Snackbar.Add("Registro creado", Severity.Success);
    }
    private async Task LoadCountriesAsync()
    {
        var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        countries = responseHttp.Response;
    }

    private async Task LoadStatesAsyn(int countryId)
    {
        await Task.Delay(5);
        var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        states = responseHttp.Response;
    }

    private async Task LoadCitiesAsyn(int stateId)
    {
        await Task.Delay(5);
        var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }
        cities = responseHttp.Response;
    }

    private async Task CountryChangedAsync(Country country)
    {
        selectedCountry = country;
        selectedState = new State();
        selectedCity = new City();
        states = null;
        cities = null;
        await LoadStatesAsyn(country.Id);
    }

    private async Task StateChangedAsync(State state)
    {
        selectedState = state;
        selectedCity = new City();
        cities = null;
        await LoadCitiesAsyn(state.Id);
    }

    private void CityChanged(City city)
    {
        selectedCity = city;
        prospect.CityId = city.Id;
    }

    private async Task<IEnumerable<Country>> SearchCountries(string searchText, CancellationToken token)
    {

        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return countries!;
        }

        return countries!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private async Task<IEnumerable<State>> SearchStates(string searchText, CancellationToken token)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return states!;
        }
        
        return states!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private async Task<IEnumerable<City>> SearchCity(string searchText, CancellationToken token)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return cities!;
        }

        return cities!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void InvalidForm()
    {
        Snackbar.Add("Por favor llena todos los campos del formulario.", Severity.Warning);
    }

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/prospects");
    }
}
