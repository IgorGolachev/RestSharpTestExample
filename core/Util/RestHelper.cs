using System.Diagnostics;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;

namespace core.Util
{
    public sealed class RestHelper
    {
        private RestClient Client;

        public RestHelper()
        {
            Client = new RestClient(ConfigHelper.BaseURL)
            {
                Pipelined = false,
                Timeout = int.Parse(ConfigHelper.RestSharpTimeOut) // ms
            };
        }

        private void SetHeaders<T>(T request) where T : RestRequest
        {
            request.AddHeader("content-type", "application/json");
        }

        private void Authenticate(Dictionary<string, string> credentials)
        {
            Client.Authenticator = new NtlmAuthenticator(credentials["UserName"], credentials["Password"]);
        }

        private IRestResponse ExecuteGETRequest(string endPoint)
        {
            var GetRequest = new RestRequest(endPoint, Method.GET);
            SetHeaders(GetRequest);
            return Client.Execute(GetRequest);
        }

        public IRestResponse ExecuteGETRequestAsUser(string endPoint, Dictionary<string, string> credentials)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Authenticate(credentials);
            var response = ExecuteGETRequest(endPoint);

            stopwatch.Stop();
            Logger.Log.Info("Get request to " + endPoint + " endpoint took " + stopwatch.ElapsedMilliseconds + " ms");

            return response;
        }

        public IRestResponse ExecutePOSTRequest(string endPoint, string payload)
        {
            var PostRequest = new RestRequest(endPoint, Method.POST);
            SetHeaders(PostRequest);
            PostRequest.AddJsonBody(payload);
            return Client.Execute(PostRequest);
        }

        public IRestResponse ExecutePOSTRequestAsUser(string endPoint, Dictionary<string, string> credentials, string payload)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Authenticate(credentials);
            var response = ExecutePOSTRequest(endPoint, payload);

            stopwatch.Stop();
            Logger.Log.Info("Post request to " + endPoint + " endpoint took " + stopwatch.ElapsedMilliseconds + " ms");

            return response;
        }
    }
}