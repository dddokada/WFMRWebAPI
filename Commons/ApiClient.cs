#region License 
// For copyright information, see <Installation_directory>/copyright.txt. 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

using System.Threading.Tasks;

namespace Commons
{


  public class ApiClient
  {

//        public CookieContainer cookies = new CookieContainer();
     //   public HttpClientHandler handler = new HttpClientHandler();
//        handler.CookiesContainer

    public Uri BaseUri { get; private set; }

    private readonly HttpClient _Client;
     //   private var cookies;
       // private var handler;


    public ApiClient(Uri baseUri, string username, string password)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      BaseUri = baseUri;
     var cookies = new CookieContainer();
     var   handler = new HttpClientHandler { CookieContainer = cookies, UseCookies = true };
      _Client = new HttpClient(handler) { BaseAddress = BaseUri };
      Login(username, password);

  

        }

    /// <returns>if the login was successful</returns>
    /// <exception cref="AggregateException">If the server was not found</exception>
    private void Login(string username, string password)
    {
      const string loginUri = "https://walmart-jp-wfmr-c.jdadelivers.com/retail/data/login";
      var loginKeyPair = new KeyValuePair<string, string>("loginName", username);
      var passwordKeyPair = new KeyValuePair<string, string>("password", password);
      var content = new FormUrlEncodedContent(new[] { loginKeyPair, passwordKeyPair });

      Console.WriteLine("Logging in...");
            Console.WriteLine(loginUri);
            Console.WriteLine(loginKeyPair);
            Console.WriteLine(passwordKeyPair);
            Console.WriteLine(content);

            HttpResponseMessage response = _Client.PostAsync(loginUri, content).Result;

            Console.WriteLine("response...");
            Console.WriteLine(response);
            Console.WriteLine("response status...");

            Console.WriteLine(response.StatusCode);


            bool loginWasSuccessful = response.StatusCode == HttpStatusCode.OK;
      if (!loginWasSuccessful)
      {
        throw new Exception("Login failed! Used " + username + "/" + password);
      }
            Console.WriteLine(response.RequestMessage);
            Console.WriteLine("Login successful!");
    }

    public async Task<T> Create<T>(Link link, T content)
    {
      return await Create(new HttpMethod(link.Method), link.Uri, content);
    }

    public async Task<T> Create<T>(HttpMethod method, string uri, T content)
    {
      var requestMessage = new HttpRequestMessage(method, uri);

      HttpResponseMessage response;
      using (Stream requestStream = new MemoryStream())
      {
        requestMessage.Content = new StreamContent(requestStream);
        ApiSerializer.WriteToStream(requestStream, content);

        response = await _Client.SendAsync(requestMessage);
      }

      switch (response.StatusCode)
      {
        case HttpStatusCode.Created:
          break;
        default:
          throw new ApiCreateException<T>(response, content);
      }

      var createdResource = ApiSerializer.ParseResponse<T>(response);

      return createdResource;
    }

    public async Task<IEnumerable<string>> Options(string uri)
    {
      HttpResponseMessage response = await _Client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
      IEnumerable<string> options = response.Content.Headers.GetValues("Allow");

      if (!response.IsSuccessStatusCode)
      {
        throw new ApiException(response);
      }
      return options;
    }

    public async Task<T> Get<T>(string url)
    {


            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            Console.WriteLine("GET!");

            requestMessage.Headers.Add("X-Hello", "world");

            Console.WriteLine(url);
            Console.WriteLine(requestMessage);
            Console.WriteLine(requestMessage.Headers);
            Console.WriteLine(requestMessage.Method);
            Console.WriteLine(requestMessage.Properties);

            Console.WriteLine(_Client.BaseAddress);
            Console.WriteLine(_Client.DefaultRequestHeaders);
            Console.WriteLine("!!!");

            System.Threading.Thread.Sleep(10000);

             HttpResponseMessage responseMessage = await _Client.SendAsync(requestMessage);

            Console.WriteLine("Response");
            Console.WriteLine(responseMessage.IsSuccessStatusCode);
            Console.WriteLine(responseMessage);

            System.Threading.Thread.Sleep(1000);

            if (!responseMessage.IsSuccessStatusCode)
      {

                Console.WriteLine("Error?");
                System.Threading.Thread.Sleep(1000);
                throw new ApiException(responseMessage);
      }

      var response = ApiSerializer.ParseResponse<T>(responseMessage);

            Console.WriteLine(response);
            System.Threading.Thread.Sleep(1000);

            return response;
    }
  }
}