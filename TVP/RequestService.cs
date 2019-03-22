using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TVP
{
    public class RequestService
    {
        private static readonly RequestService instance = new RequestService();

        //private readonly string CONNECT_URL_AUTH = string.Format(@"http://{0}:{1}{2}", "localhost", "8080", @"/xamarin/AuthorizationServer");
        //private readonly string CONNECT_URL_GET = string.Format(@"http://{0}:{1}{2}", "localhost", "8080", @"/xamarin/GetServlet");
        //private readonly string CONNECT_URL_SET = string.Format(@"http://{0}:{1}{2}", "localhost", "8080", @"/xamarin/SetServletForJSON");

        private readonly string CONNECT_URL_AUTH = string.Format(@"http://{0}:{1}{2}", "localhost", "30553", @"/postgresql/AuthorizationServer");
        private readonly string CONNECT_URL_GET = string.Format(@"http://{0}:{1}{2}", "localhost", "30553", @"/postgresql/GetServlet");
        private readonly string CONNECT_URL_SET = string.Format(@"http://{0}:{1}{2}", "localhost", "30553", @"/postgresql/SetServletForJSON");


        private RequestService()
        {

        }
        public static RequestService Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<string> AuthorizationAsync(string id, string pw)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CONNECT_URL_AUTH.ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = new TimeSpan(0, 3, 0); //3분

                    using (var request = new HttpRequestMessage())
                    {
                        //request.RequestUri = new Uri(CONNECT_URL_AUTH.ToString());
                        request.Method = HttpMethod.Post;
                        //request.Headers.Add("id", id);
                        //request.Headers.Add("pw", pw);

                         request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", id, pw))));

                        response = await client.SendAsync(request);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                return "ERROR " + ex.Message.ToString();
            }
            finally
            {
                if (response != null) response.Dispose();
            }
        }

        public async Task<string> GetRequestAsync(string requestParam)
        {
            HttpContent content = new StringContent(requestParam, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri(CONNECT_URL_GET.ToString());
                    client.Timeout = new TimeSpan(0, 3, 0); //3분

                    using (var request = new HttpRequestMessage())
                    {
                        request.RequestUri = new Uri(CONNECT_URL_GET.ToString());
                        request.Method = HttpMethod.Post;
                        request.Headers.Add("jwt", Global.token);
                        //request.Headers.Add("jwt", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJTQ09QRSI6IklOQk9VTkR8T1VUQk9VTkR8UFVUQVdBWXxJTlZFTlRPUlkiLCJpc3MiOiJiY3dtcyIsIlNDT1BFMiI6IklOQk9VTkR8T1VUQk9VTkR8UFVUQVdBWSIsImV4cCI6MTUxNzYzOTQzMH0.mKAIxCGMyreLY0D5GIWgaMocU3vqqPRGGWcjf_o_79FZ1kRz4CUWXMQv5OSBzm_gwg8_GE7u4Khq3FC6ZwTIMfQpkotMV5SJYyMounerBRep2dyZlldWoL6HFozLLa_2yerAhsbNGHTCfmPuridEqR7E85tG70vumBn70sZR0fc");
                        request.Content = content;
     
                        response = await client.SendAsync(request);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                return "ERROR " + ex.Message.ToString();
            }
            finally
            {
                if (response != null) response.Dispose();
            }
        }

        public async Task<string> SetRequestAsync(string requestParam)
        {
            //var postData = new List<KeyValuePair<string, string>>();
            //if (!string.IsNullOrEmpty(requestParam))
            //{
            //    postData.Add(new KeyValuePair<string, string>("requestParam", requestParam));
            //}
            //HttpContent content = new FormUrlEncodedContent(postData); //url길이 제한

            //How to #2
            HttpContent content = new StringContent(requestParam, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CONNECT_URL_SET.ToString());
                    client.Timeout = new TimeSpan(0, 3, 0); //3분

                    using (var request = new HttpRequestMessage())
                    {
                        //request.RequestUri = new Uri(CONNECT_URL_SET.ToString());
                        request.Method = HttpMethod.Post;
                        request.Headers.Add("jwt", Global.token);
                        request.Content = content;
                                                
                        response = await client.SendAsync(request);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                return "ERROR " + ex.Message.ToString();
            }
            finally
            {
                if (response != null) response.Dispose();
            }
        }
    }
}
