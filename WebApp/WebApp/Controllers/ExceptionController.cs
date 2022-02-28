using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExceptionController : ControllerBase
    {

        [HttpGet("errors")]
        public async Task<string> MyProfile()
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            Header temp;
            // In production code, don't destroy the HttpClient through using, but better reuse an existing instance
            /* https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ */
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://login.microsoftonline.com/46e80cf4-6287-447e-a1a2-b85afd349604/oauth2/v2.0/token"))
                {
                    request.Headers.TryAddWithoutValidation("Cookie", "fpc=AleU0WhijvFHuet88wEtv5g2EDOHAQAAAD1kqdkOAAAA3yF9owEAAADbZ6nZDgAAAL__NTMBAAAABmip2Q4AAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");

                    var contentList = new List<string>();
                    contentList.Add($"client_id={Uri.EscapeDataString("482068b7-6a2b-4fdc-8627-c4c79894e0ed")}");
                    contentList.Add($"scope={Uri.EscapeDataString("api://482068b7-6a2b-4fdc-8627-c4c79894e0ed/.default")}");
                    contentList.Add($"client_secret={Uri.EscapeDataString("KBq7Q~xFUc6CBUSEnvKF1PnjS8Z8lPTYzGaxi")}");
                    contentList.Add($"grant_type={Uri.EscapeDataString("client_credentials")}");
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response1 = await httpClient.SendAsync(request);
                    var contents = await response1.Content.ReadAsStringAsync();
                    temp = JsonConvert.DeserializeObject<Header>(contents);
                    // var access_token = contents["access_token"];
                    int x = 6;
                }
            }


            var response = string.Empty;
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", temp.access_token);
                HttpResponseMessage result = await client.GetAsync("https://localhost:6001/api/users/exception");
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;


        }




    }
}
