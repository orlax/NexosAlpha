using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace NexosAlpha
{
    public class App : Application
    {
        public static Image image{get;set;}
        public App()
        {
            // The root page of your application
            MainPage = new mainPage();
            MessagingCenter.Subscribe<Byte[]>(this, "uploadImage", uploadImage);
            MessagingCenter.Subscribe<string>(this, "uploadImage", uploadImagePath);
        }

        private void uploadImagePath(string obj)
        {
            //   
           // var img = FileImageSource.FromFile(obj);
           // System.IO.StreamReader reader = new System.IO.StreamReader(ob);

        }

        private async void uploadImage(byte[] obj)
        {
            try
            {
                //creating message with headers
                var filecontent = new ByteArrayContent(obj);
                filecontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octec-stream");
                //sending via http
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync("http://nexos.oalmario.com/nexusService.svc/recognizeImage", filecontent);
                if (response.IsSuccessStatusCode)  
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("answer was {0}", content);
                    if (content == "\"empty\"")
                    {
                        MessagingCenter.Send<string>("notFound", "notFound");
                        MessagingCenter.Send<string>("stopRunning", "stopRunning");
                    }
                    else
                    {
                        var uri = content.Substring(1, content.Length - 3);
                        uri = uri.Replace("\\/", "/");
                        uri = uri.Replace("\\", "");
                        MessagingCenter.Send<string>("stopRunning", "stopRunning");
                        MessagingCenter.Send<string>(uri, "showVideo");
                     //   Device.OpenUri(new Uri(uri));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessagingCenter.Send<string>("stopRunning", "stopRunning");
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
