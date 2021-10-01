using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace citrix.location
{
    class Program
    {
        public class Token
        {
            [JsonProperty(PropertyName = "token_type")]
            public string Type { get; set; }
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }
            [JsonProperty(PropertyName = "expires_in")]
            public int Expires { get; set; }
        }

        public class ItemsCollection
        {
            [JsonProperty(PropertyName = "items")]
            public IList<Location> Items { get; set; }
        }

        public class Location
        {
            [JsonProperty(PropertyName = "id")]
            public Guid Id { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "internalOnly")]
            public Boolean InternalOnly { get; set; }
            [JsonProperty(PropertyName = "timeZone")]
            public string TimeZone { get; set; }
            [JsonProperty(PropertyName = "readOnly")]
            public Boolean ReadOnly { get; set; }
        }
        
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var token = await GetToken();

            var customerId = Environment.GetEnvironmentVariable("Citrix_Customer_Id");

            var uri = $"https://registry.citrixworkspacesapi.net/{customerId}/resourcelocations";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("CwsAuth", "Bearer=" + token.AccessToken);
            var locations = new ItemsCollection();
            try
            {
                var response = await client.GetAsync(uri);
                var contents = await response.Content.ReadAsStringAsync();
                locations = JsonConvert.DeserializeObject<ItemsCollection>(contents);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw;
            }

            foreach (Location location in locations.Items)
            {
                Console.WriteLine(location.Name);
            }
        }

        private static async Task<Token> GetToken()
        {
            var customerId = Environment.GetEnvironmentVariable("Citrix_Customer_Id");
            var clientId = Environment.GetEnvironmentVariable("Citrix_Client_Id");
            var clientSecret = Environment.GetEnvironmentVariable("Citrix_Client_Secret");

            var uri = $"https://api-us.cloud.com/cctrustoauth2/{customerId}/tokens/clients";

            var parameters = new Dictionary<string, string>();
            parameters["grant_type"] = "client_credentials";
            parameters["client_id"] = clientId;
            parameters["client_secret"] = clientSecret;

            var token = new Token();
            try
            {
                var response = await client.PostAsync(uri, new FormUrlEncodedContent(parameters));
                var contents = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<Token>(contents);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw;
            }

            return token;
        }
    }
}
