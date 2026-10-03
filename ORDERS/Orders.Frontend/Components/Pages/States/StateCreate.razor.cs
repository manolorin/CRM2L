using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Components.Pages.States;

public partial class StateCreate
{

    private State state = new();
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Parameter] public int CountryId { get; set; }

    private async Task CreateAsync()
    {
        state.CountryId = CountryId;
        var responseHttp = await Repository.PostAsync("/api/states", state);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        Return();
        Snackbar.Add("Registro creado", Severity.Success);
    }

    private void Return()
    {
        MudDialog.Cancel();
        NavigationManager.NavigateTo($"/countries/details/{CountryId}");
    }
}