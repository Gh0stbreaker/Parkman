using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Parkman.Frontend.Models;

namespace Parkman.Frontend.Services;

public static class HttpResponseMessageExtensions
{
    public static async Task<string?> ApplyValidationErrorsAsync(this HttpResponseMessage response, EditContext editContext, ValidationMessageStore messageStore)
    {
        messageStore.Clear();

        if (response.IsSuccessStatusCode)
        {
            editContext.NotifyValidationStateChanged();
            return null;
        }

        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            if (problem?.Errors != null)
            {
                foreach (var kvp in problem.Errors)
                {
                    var field = new FieldIdentifier(editContext.Model, kvp.Key);
                    messageStore.Add(field, kvp.Value);
                }
            }
            editContext.NotifyValidationStateChanged();
            return problem?.Title ?? problem?.Detail;
        }
        catch
        {
            editContext.NotifyValidationStateChanged();
            return $"Request failed with status code {(int)response.StatusCode}.";
        }
    }
}
