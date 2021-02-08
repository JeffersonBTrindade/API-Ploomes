using System;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace testeAPIPloomes
{
    class RequestHandler
    {
        private static string PLOOMES_API_PATH = "https://api2.ploomes.com/";
        private static HttpClient ploomesClient;
        private static string userkey = "66907FAF34000610221422A600AF0B07EC777D3AE18FCBA4AC8A1D41AC0F7839D87AAF5AB10FD5D5C106FD60814CB189532E6A7D0459BEEC0EB0E808916C4335";

        public static void instantiatePloomesConnection()
        {
            ploomesClient = new HttpClient();
            ploomesClient.DefaultRequestHeaders.Add("User-key", userkey);
            ploomesClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("aplication/json"));
        }

        public static JArray MakePloomesRequest(string url, Method method, JObject json = null)
        {
            try
            {
                instantiatePloomesConnection();

                System.Threading.Thread.Sleep(1000);
                string response = string.Empty;
                url = PLOOMES_API_PATH + url;

                if (method == Method.GET)
                    response = ploomesClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                else if (method == Method.POST)
                {
                    if (json != null)
                        response = ploomesClient.PostAsync(url, new StringContent(json.ToString())).Result.Content.ReadAsStringAsync().Result;
                    else
                        response = ploomesClient.PostAsync(url, new StringContent(new JObject().ToString())).Result.Content.ReadAsStringAsync().Result;
                }
                else if (method == Method.PATCH)
                {
                    var content = new ObjectContent<JObject>(json, new JsonMediaTypeFormatter());
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
                    response = ploomesClient.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
                }
                else if (method == Method.DELETE)
                {
                    ploomesClient.DeleteAsync(url);
                    return null;
                }

                return JsonConvert.DeserializeObject<JObject>(response)["value"] as JArray;
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine("Error in MakePloomesRequest method --> " + ex.Message);
                throw ex;
            }
        }
    }
}
