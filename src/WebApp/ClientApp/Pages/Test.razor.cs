using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

using MudBlazor;

namespace AccessControl.Client.Pages
{
    public partial class Test
    {
        HubConnection hubConnection = null!;

        [Required]
        public string Name { get; set; } = null!;

        async Task OnSubmit()
        {
            await hubConnection.InvokeAsync("SayHi", Name);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                hubConnection = new HubConnectionBuilder().WithUrl($"{NavigationManager.BaseUri}hubs/test", options =>
                {
                    options.AccessTokenProvider = async () => await AccessTokenProvider.GetAccessTokenAsync();
                }).WithAutomaticReconnect().Build();
                hubConnection.On<string>("Responded", OnReponded);
                hubConnection.Closed += (error) =>
                {
                    if (error is not null)
                    {
                        Snackbar.Add($"{error.Message}", Severity.Error);
                    }

                    Snackbar.Add("Connection closed");
                    return Task.CompletedTask;
                };
                hubConnection.Reconnected += (error) =>
                {
                    Snackbar.Add("Reconnected");
                    return Task.CompletedTask;
                };
                hubConnection.Reconnecting += (error) =>
                {
                    Snackbar.Add("Reconnecting");
                    return Task.CompletedTask;
                };
                await hubConnection.StartAsync();
                Snackbar.Add("Connected");
            }
            catch (Exception exc)
            {
                Snackbar.Add(exc.Message.ToString(), Severity.Error);
            }
        }

        Task OnReponded(string message)
        {
            Snackbar.Add(message, Severity.Info);
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}
