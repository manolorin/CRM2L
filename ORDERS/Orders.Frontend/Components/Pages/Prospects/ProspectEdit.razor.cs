using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Frontend.Components.Pages.Prospects;

[Authorize]
public partial class ProspectEdit
{
 
    private Prospect? prospect;
    private List<Country>? countries;
    private List<State>? states;
    private List<City>? cities;
    private bool loading;

     ProspectDTO prospectDTO = new ProspectDTO();


    private Country selectedCountry = new();
    private State selectedState = new();
    private City selectedCity = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private ILoginService LoginService { get; set; } = null!;

    [Parameter] public int Id { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await LoadProspectAsyc();
        await LoadCountriesAsync();
        await LoadStatesAsyn(prospect!.City!.State!.Country!.Id);
        await LoadCitiesAsyn(prospect!.City!.State!.Id);

        selectedCountry = prospect!.City!.State!.Country!;
        selectedState = prospect.City.State;
        selectedCity = prospect.City;
    }

    private async Task LoadProspectAsyc()
    {

        var responseHttp = await Repository.GetAsync<Prospect>($"api/prospects/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("prospects");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            prospect = responseHttp.Response;    

            loading = false;
        }
    }

    private async Task EditAsync()
    {
        prospectDTO.Id = prospect.Id;
        prospectDTO.Name = prospect.Name;
        prospectDTO.Address = prospect.Address;
        prospectDTO.CityId = prospect.CityId;
        var responseHttp = await Repository.PutAsync($"/api/prospects", prospectDTO!);

        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(messageError!, Severity.Error);
            return;
        }


        ReturnAction();
        Snackbar.Add("Prospecto actualizado", Severity.Success);
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
