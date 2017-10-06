using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using Elmah;
using Newtonsoft.Json;
using PairingTest.Web.Wrappers.Interfaces;

namespace PairingTest.Web.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient _httpClient;
        private string _apiName;

        public void Initialise(string url, string apiName)
        {
            var httpClientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = true
            };

            _httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(url)
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiName = apiName;
        }

        public string GetUrl()
        {
            return _httpClient.BaseAddress.ToString();
        }

        public virtual T GetAsync<T>(string apiMethod)
        {
            HttpResponseMessage response;

            try
            {
                response = _httpClient.GetAsync(apiMethod).Result;

            }
            catch (AggregateException ex)
            {
                var error = new Error(ex);

                ErrorLog.GetDefault(HttpContext.Current).Log(error);

                throw ThrowException();
            }

            return HandleResponse<T>(response);
        }

        public T PutAsync<T>(string apiMethod, object data)
        {
            HttpResponseMessage response;

            try
            {
                var jsonString = JsonConvert.SerializeObject(data);
                var requestMessage = new StringContent(jsonString, Encoding.UTF8, "application/json");

                response = _httpClient.PutAsync(apiMethod, requestMessage).Result;
            }
            catch (AggregateException ex)
            {
                var error = new Error(ex);

                ErrorLog.GetDefault(HttpContext.Current).Log(error);

                throw ThrowException();
            }

            return HandleResponse<T>(response);
        }

        public T DeleteAsync<T>(string apiMethod)
        {
            HttpResponseMessage response;

            try
            {
                response = _httpClient.DeleteAsync(apiMethod).Result;
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

                throw ThrowException();
            }

            return HandleResponse<T>(response);
        }

        private T HandleResponse<T>(HttpResponseMessage response)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }

                var responseMessage = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var error = new Error
                {
                    Time = DateTime.Now,
                    Message = string.Format("{0} api has encountered an error: {1}", _apiName, responseMessage),
                    User = Thread.CurrentPrincipal.Identity.Name
                };

                ErrorLog.GetDefault(HttpContext.Current).Log(error);

                response.EnsureSuccessStatusCode();

                //Should never be hit
                throw new NotSupportedException();
            }
            catch (HttpRequestException ex)
            {
                var error = new Error(ex);

                ErrorLog.GetDefault(HttpContext.Current).Log(error);

                throw ThrowException();
            }
        }


        private HttpException ThrowException()
        {
            return new HttpException(string.Format("{0} is not available at the moment", _apiName));
        }
    }
}
