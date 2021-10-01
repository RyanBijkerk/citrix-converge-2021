using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace citrix.notification
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


        public class Notfication
        {
            public string DestinationAdmin { get; set; }
            public string Component { get; set; }
            public DateTime CreatedDate { get; set; }
            public Guid EventId { get; set; }
            public string Severity { get; set; }
            public string Priority { get; set; }
            public List<NotficationContent> Content { get; set; }

        }

        public class NotficationContent
        {
            public string LanguageTag { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

        }

        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var token = await GetToken();

            var customerId = Environment.GetEnvironmentVariable("Citrix_Customer_Id");

            var uri = $"https://notifications.citrixworkspacesapi.net/{customerId}/notifications/items";

            var notification = new NotficationContent
            {
                LanguageTag = "en-US",
                Title = "Citrix Converge 2021 Notification",
                Description = "This notification is send using C#"
            };

            var body = new Notfication
            {
                DestinationAdmin = "*",
                Component = "Citrix Cloud",
                CreatedDate = DateTime.UtcNow,
                EventId = Guid.NewGuid(),
                Severity = "Information",
                Priority = "Normal",
                Content = new List<NotficationContent>()
                {
                    new NotficationContent
                    {
                        LanguageTag = "en-US",
                        Title = "Citrix Converge 2021 Notification",
                        Description = "This notification is send using C#"
                    }
                }
            };


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("CwsAuth", "Bearer=" + token.AccessToken);

            var content = new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);


            Console.WriteLine(response.StatusCode);
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
