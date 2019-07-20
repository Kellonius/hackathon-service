using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Hackathon_Service.Models.Epic;
using Newtonsoft.Json;

namespace Hackathon_Service.Services
{
    public class HttpService
    {
        public string apiUrl { get; set; }
        
        public HttpService(string apiUrl)
        {
            this.apiUrl = apiUrl;
        }

        public T Get<T>(string endPoint)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                HttpResponseMessage response = client.GetAsync(endPoint).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}