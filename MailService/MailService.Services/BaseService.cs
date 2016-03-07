using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MailService.Services
{
    public class BaseService
    {
        public const string ServerUrl = "http://mailservice.azurewebsites.net/";

        protected string Post(string requestUri, string jsonObj, int facilityId = -1, int inspectorId = -1)
        {
            string jsonResult = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ServerUrl);

                client.Timeout = new TimeSpan(0, 0, 120); // Default timeout

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent jsonContent = new StringContent(jsonObj, System.Text.Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> response = client.PostAsync(requestUri, jsonContent);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    HttpContent content = response.Result.Content;
                    jsonResult = content.ReadAsStringAsync().Result;
                }
            }
            return jsonResult;
        }

        protected void PostAsync(string requestUri, string jsonObj)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ServerUrl);
                client.Timeout = new TimeSpan(0, 0, 120); // Default timeout
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(jsonObj, System.Text.Encoding.UTF8, "application/json");
                client.PostAsync(requestUri, jsonContent);
            }
        }

        protected string Get(string url)
        {
            string response = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ServerUrl);
                client.Timeout = new TimeSpan(0, 0, 120); // Default timeout

                try
                {
                    Task<HttpResponseMessage> taskResponse = client.GetAsync(url);
                    taskResponse.Wait();

                    if (taskResponse.Result.IsSuccessStatusCode)
                    {
                        HttpContent content = taskResponse.Result.Content;
                        response = content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception)
                {
                    // Debug.WriteLine(ex);
                }
            }

            return response;
        }

        protected string Delete(string requestUri)
        {
            string jsonResult = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ServerUrl);
                client.Timeout = new TimeSpan(0, 0, 120); // Default timeout
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> response = client.DeleteAsync(requestUri);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    HttpContent content = response.Result.Content;
                    jsonResult = content.ReadAsStringAsync().Result;
                }
            }
            return jsonResult;
        }
    }
}
