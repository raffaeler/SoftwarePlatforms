using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace L02WebApiClient;

internal class WebClient
{
    // You can either use http or https depending on the web server application
    // Https requires the self-signed certificate installed in Windows
    // 
    // The following URL MUST match the one specified in the launchSettings.json
    // of the web server application
    //
    // The web application and web servers do NOT require administrative privileges.
    // The web server requires administrative privileges to listen below the port 1024
    //
    //private readonly static string _address = "https://localhost:7075";
    private readonly static string _address = "http://localhost:5275";

    private HttpClient _client { get; }
    private JsonSerializerOptions _options;

    public WebClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_address);
        _client.DefaultRequestHeaders.Add("User-Agent", "Demo Http Client");
        _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true,
            NoStore = true,
            MaxAge = new TimeSpan(0),
            MustRevalidate = true
        };

        _options = new()
        {
            PropertyNameCaseInsensitive = true,
        };
    }


    public async Task<Todo[]> MakeGetAPI(string relativeAddress)
    {
        try
        {
            var response = await _client.GetAsync(relativeAddress);
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var todo = JsonSerializer.Deserialize<Todo[]>(content, _options);
            if (todo == null) throw new Exception($"Empty response");
            return todo;
        }
        catch (HttpRequestException err)
        {
            Debug.WriteLine($"Failed with status {err.StatusCode}");
            Debug.WriteLine(err.ToString());
            throw;
        }
        catch (Exception err)
        {
            Debug.WriteLine(err.ToString());
            throw;
        }
    }

    /// <summary>
    /// Unless the server always fails, this call will always succeed
    /// because Polly retries for several time when it fails.
    /// </summary>
    public async Task<bool> MakePostAPI(string relativeAddress, string title, string text)
    {
        bool result;
        try
        {
            //new StringContent($"\"{data}\"", Encoding.UTF8, "application/json"));
            //using var content = JsonContent.Create(data);
            var todo = new { Title = title, Text = text };
            var content = JsonContent.Create(todo);
            using var response = await _client.PostAsync(relativeAddress, content);
            result = response.IsSuccessStatusCode;
        }
        catch (HttpRequestException err)
        {
            Debug.WriteLine($"Failed with status {err.StatusCode}");
            result = false;
        }
        catch (Exception)
        {
            result = false;
        }

        return result;
    }

}