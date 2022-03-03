using System;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using DownLoadFileGraph.MicrosoftGraph;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace DownLoadFileGraph
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            await GetAccessToken();
        }

        public static async System.Threading.Tasks.Task GetAccessToken()
        {
            try
            {
                IConfidentialClientApplication app;
                app = ConfidentialClientApplicationBuilder.Create("7ab4d0d7-72af-4716-b469-e2995f62132f")
                                                          .WithClientSecret("KpW7Q~deOkZhNYSygx4tEa~FkH-lBLDOOReSj")
                                                          .WithAuthority(new Uri("https://login.microsoftonline.com/52ff68c1-439b-4bba-81a4-15c9eab6adc7/oauth2/v2.0/authorize"))
                                                          .Build();
                string[] scope = { "https://graph.microsoft.com/.default" };
                Microsoft.Identity.Client.AuthenticationResult result = await app.AcquireTokenForClient(scope)
                       .ExecuteAsync();

                string accesstoken = result.AccessToken;

                string httpRequestURL = "https://graph.microsoft.com/v1.0/sites/sohodragonlabs.sharepoint.com:/sites/PicLibTest";
                string siteId = string.Empty;
                SiteDetails site = new SiteDetails();
                HttpWebRequest itemRequest = (HttpWebRequest)HttpWebRequest.Create(httpRequestURL);
                itemRequest.Method = "GET";
                itemRequest.Accept = "*/*";
                itemRequest.Headers.Add("Authorization", "Bearer " + accesstoken);
                HttpWebResponse response = (HttpWebResponse)itemRequest.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    site = JsonConvert.DeserializeObject<SiteDetails>(json);
                }
                if (!string.IsNullOrEmpty(site.Id))
                {
                    siteId = site.Id.Split(',')[1];
                }
                Console.WriteLine(siteId);
                HttpProvider httpProvider = new HttpProvider();

                GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(requestMessage =>
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accesstoken);
                    return System.Threading.Tasks.Task.FromResult(0);
                }), httpProvider);

                string driveID = null;
                var drives = await graphClient.Sites[siteId].Drives
                    .Request()
                    .GetAsync();

                foreach (var drive in drives.CurrentPage)
                {
                    if (drive.Name == "PictureLibrary")
                    {
                        driveID = drive.Id;
                    }
                }

                Console.WriteLine(driveID);

                string httpRequestURL1 = "https://graph.microsoft.com/v1.0/sites/" + siteId + "/drives/" + driveID + "/root:/test.jpg";
                string downloadURL = string.Empty;
                DownloadURL fileInfo = new DownloadURL();
                HttpWebRequest itemRequest1 = (HttpWebRequest)HttpWebRequest.Create(httpRequestURL1);
                itemRequest1.Method = "GET";
                itemRequest1.Accept = "*/*";
                itemRequest1.Headers.Add("Authorization", "Bearer " + accesstoken);
                HttpWebResponse response1 = (HttpWebResponse)itemRequest1.GetResponse();
                using (StreamReader reader = new StreamReader(response1.GetResponseStream()))
                {
                    string json1 = reader.ReadToEnd();
                    fileInfo = JsonConvert.DeserializeObject<DownloadURL>(json1);
                }
                Console.WriteLine(fileInfo.MicrosoftGraphDownloadUrl);
                downloadURL = fileInfo.MicrosoftGraphDownloadUrl;



               
                string savePath = @"D:\Mizuho\Console Applications\pictures\FreeImages.jpg";
                WebClient client = new WebClient();
                client.DownloadFile(downloadURL, savePath);
                Console.ReadLine();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
