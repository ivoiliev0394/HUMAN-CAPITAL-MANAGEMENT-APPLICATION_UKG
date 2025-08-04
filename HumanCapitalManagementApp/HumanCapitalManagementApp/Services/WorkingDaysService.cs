using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HumanCapitalManagementApp.Services
{
    public class WorkingDaysService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        // Constructor with dependency injection for HttpClient and IConfiguration
        public WorkingDaysService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // Main method to fetch working days from API Ninjas
        public async Task<int?> GetWorkingDaysAsync(string country, int year, int month)
        {
            string normalizedCountryCode = country.ToLower(); // normalize country code

            // Construct the API request URL with query parameters
            var request = new HttpRequestMessage(
                    HttpMethod.Get,
                 $"https://api.api-ninjas.com/v1/workingdays?country={normalizedCountryCode}&month={month}");

//&year={year}

            // Set the required API key in the headers (you can hardcode it or keep it in appsettings)
            request.Headers.Add("X-Api-Key", _config["ApiNinjas:Key"]);

            // Send the HTTP request
            var response = await _httpClient.SendAsync(request);

            // If the request failed, return null
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error response: {error}");
                return null;
            }

            // Read and parse the JSON response body
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WorkingDaysResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allows mapping "workdays" regardless of casing
            });

            // Return the number of working days, or null if not found
            return result?.NumWorkingDays;
        }

        // Internal class used for deserializing the API response
        private class WorkingDaysResponse
        {
            [JsonPropertyName("num_working_days")]
            public int NumWorkingDays { get; set; }

            [JsonPropertyName("num_non_working_days")]
            public int NumNonWorkingDays { get; set; }

            [JsonPropertyName("working_days")]
            public List<string> WorkingDays { get; set; }
        }
    }
}
