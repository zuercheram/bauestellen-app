using Baustellen.App.Shared.Exceptions;
using System.Net.Http.Json;

namespace Baustellen.App.Client.Services;

public class ApiBaseService(HttpClient http)
{
    public async Task<T> GetFromJsonAsync<T>(Uri url)
    {
        try 
        {
            var response = await http.GetFromJsonAsync<T>(url);
            return response == null ? throw new WebApiRequestException("Response content is NULL!") : response;
        }
        catch (OperationCanceledException ex) {
            // TODO: Log error
            throw new WebApiRequestException(ex);     
        }
    }
    
    public async Task PostAsync<T>(Uri url, T content)
    {
        try
        {
            var response = await http.PostAsJsonAsync<T>(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebApiRequestException($"POST to {url} failed with {response.StatusCode}, reason {response.ReasonPhrase}");
            }            
        }
        catch(OperationCanceledException ex)
        {
            // TODO implement log.
            throw new WebApiRequestException(ex);
        }
    }

    public async Task PutAsync<Tval>(Uri url, Tval content)
    {
        try
        {
            var response = await http.PutAsJsonAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebApiRequestException($"PUT to {url} failed with {response.StatusCode}, reason {response.ReasonPhrase}");
            }
        }
        catch (OperationCanceledException ex)
        {
            throw new WebApiRequestException(ex);
        }
    }

    public async Task DeleteAsync(Uri url)
    {
        try
        {
            var response = await http.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebApiRequestException($"DELETE to {url} failed with {response.StatusCode}, reason {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            throw new WebApiRequestException(ex);
        }
    }
}
