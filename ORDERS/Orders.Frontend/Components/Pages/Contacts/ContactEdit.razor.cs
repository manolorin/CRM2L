using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Frontend.Components.Pages.Contacts;

public partial class ContactEdit
{
    private EditContext editContext = null!;
    private Contact contact = new();

    private ContactDTO contactDTO ;

    private Prospect? prospect;

    private List<Country>? countries;
    private List<State>? states;
    private List<City>? cities;
    private bool loading;
    private string? imageUrl;
    private string? titleLabel;

    private Country selectedCountry = new();
    private State selectedState = new();
    private City selectedCity = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public int ContactId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadContactAsync();
        await LoadCountriesAsync();
        await LoadStatesAsyn(contact!.City!.State!.Country!.Id);
        await LoadCitiesAsyn(contact!.City!.State!.Id);

        selectedCountry = contact!.City!.State!.Country!;
        selectedState = contact.City.State;
        selectedCity = contact.City;

        if (!string.IsNullOrEmpty(contact!.Photo))
        {
            imageUrl = contact.Photo;
            contact.Photo = null;
        }

    }

    private async Task LoadContactAsync()
    {
        var responseHttp = await Repository.GetAsync<Contact>($"api/contacts/{ContactId}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo($"/prospects/details/{contact?.ProspectId}");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            contact = responseHttp.Response;

            loading = false;
        }
    }

   

    private async Task EditAsync()
    {
        contactDTO = new ContactDTO()
        {
            Id = contact.Id,
            Address = contact.Address,
            CityId = contact.CityId,
            Email = contact.Email,
            LastName = contact.LastName,
            Name = contact.Name,
            ProspectId = contact.ProspectId,
            PhoneNumber = contact.PhoneNumber,
            Photo = contact.Photo
        };
      
                 

        var responseHttp = await Repository.PutAsync("/api/contacts", contactDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        Return();
        Snackbar.Add("Registro actualizado", Severity.Success);
    }



    private void Return()
    {
        NavigationManager.NavigateTo($"/prospect/details/{contact.ProspectId}");
    }


    private void ImageSelected(string imageBase64)
    {
        contact!.Photo = imageBase64;
        imageUrl = null;
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
        contact.CityId = city.Id;
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

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/");
    }

    private void InvalidForm()
    {
        Snackbar.Add("Por favor llena todos los campos del formulario.", Severity.Warning);
    }

}
